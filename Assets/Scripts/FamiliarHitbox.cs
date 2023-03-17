using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarHitbox : Collideable
{
    public int familiar_level;
    private float push_force = 1.0f;
    private float push_force_gain = 0.2f;
    private float hit_cooldown = 0.25f;
    private float hit_cooldown_decrease = 2.0f;
    private float last_hit;

    public void Awake()
    {
        Update_Level();
    }

    public void Update_Level()
    {
        familiar_level = GameManager.instance.familiar.familiar_level;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Enemy" && Time.time - last_hit > (hit_cooldown + (hit_cooldown_decrease/familiar_level)))
        {
            last_hit = Time.time;
            // Make a damage object and send it to the player.
            Damage damage = new Damage
            {
                damage_amount = familiar_level,
                origin = transform.position,
                push_force = push_force + (push_force_gain * familiar_level)
            };
            coll.SendMessage("ReceiveDamage", damage);
            GameManager.instance.ShowText("Hehe Fresh Blood.", 15, Color.red, transform.position, Vector3.up*25, 1.0f);
        }
    }
}