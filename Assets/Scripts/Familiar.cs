using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Passive ally that assists the hero.
public class Familiar : MonoBehaviour
{
    public int familiar_level;
    // Will need to stick around the player.
    private Transform player_transform;
    public float max_distance_from_player = 0.6f;
    // If chasing then they'll try to catch up to the player.
    private bool chasing;
    // Otherwise they'll fly around the player.
    private float rotate_speed = 1.0f;
    private float rotate_gain_per_level = 0.2f;
    private float radius = 0.3f;
    private Vector3 center;
    private float angle;
    // Has a chance of healing the hero, with their blood magic.
    private int heal_counter = 0;
    public ContactFilter2D filter;
    // Customizable stats that the player can put stat points into whenever the familiar levels up.
    public int rotate_speed_increase = 0;
    public int heal_threshold_increase = 0;

    protected void Start()
    {
        player_transform = GameManager.instance.player.transform;
        center = player_transform.position;
    }

    private void FixedUpdate()
    {
        angle += (rotate_speed + (rotate_gain_per_level * familiar_level) + (rotate_speed_increase/10)) * Time.deltaTime;
        var offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * radius;
        transform.position = center + offset;
        heal_counter += 1;
        if (heal_counter >= 1000 && (GameManager.instance.player.health - heal_threshold_increase) * 3 < GameManager.instance.player.max_health)
        {
            HealMaster();
        }
        else if (heal_counter >= 1000 && GameManager.instance.player.health >= GameManager.instance.player.max_health)
        {
            int talk = Random.Range(0, 10);
            heal_counter = 0;
            if (talk == 0)
            {
                GameManager.instance.ShowText("Find Enemies! Make Blood!", 15, Color.red, transform.position, Vector3.up*25, 1.0f);
            }
            if (talk == 1)
            {
                Damage damage = new Damage
                {
                    damage_amount = familiar_level * familiar_level,
                    origin = transform.position,
                    push_force = 0
                };
                GameManager.instance.player.SendMessage("ReceiveDamage", damage);
                GameManager.instance.ShowText("Blood from Master.", 15, Color.red, transform.position, Vector3.up*25, 1.0f);
            }
        }
    }

    protected void HealMaster()
    {
        heal_counter = 0;
        GameManager.instance.player.SendMessage("ReceiveHealing", familiar_level);
        GameManager.instance.ShowText("Blood for Master.", 15, Color.white, transform.position, Vector3.up*25, 0.7f);
    }

    protected void UpdateMotor(Vector3 input)
    {
        transform.Translate(0, input.y * Time.deltaTime, 0);
        transform.Translate(input.x * Time.deltaTime, 0, 0);
    }

    public void SetLevel(int level)
    {
        familiar_level = level;
        FamiliarHitbox hitbox = GetComponentInChildren<FamiliarHitbox>();
        hitbox.Update_Level();
    }

    public void SetStats(FamiliarStatsWrapper loaded_stats)
    {
        rotate_speed_increase = loaded_stats.rotate_speed_increase;
        heal_threshold_increase = loaded_stats.heal_threshold_increase;
    }
}
