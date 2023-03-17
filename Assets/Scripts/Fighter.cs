using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public info.
    public int health = 10;
    public int max_health = 10;
    public int defense = 0;
    public int damage_reduction = 0;
    public float recovery_speed = 0.2f;

    // Iframes.
    protected float i_frames = 0.25f;
    protected float last_i_frame;

    // Push.
    protected Vector3 push_direction;

    // All fighters can take damage and die.
    protected virtual void ReceiveDamage(Damage damage)
    {
        if (Time.time - last_i_frame > i_frames)
        {
            last_i_frame = Time.time;
            damage.damage_amount -= defense;
            damage.damage_amount = Mathf.RoundToInt(damage.damage_amount * (1 - (damage_reduction/(100 + damage_reduction))));
            if (damage.damage_amount < 1)
            {
                damage.damage_amount = 1;
            }
            health -= damage.damage_amount;
            push_direction = (transform.position - damage.origin).normalized * damage.push_force;

            GameManager.instance.ShowText(damage.damage_amount.ToString(), 20, Color.red, transform.position, Vector3.up*25, 1.0f);

            if (health <= 0)
            {
                health = 0;
                Death();
            }
        }
    }

    // Fighters can also be healed, by fountains or other things.
    protected virtual void ReceiveHealing(int healing)
    {
        if (health < max_health)
        {
            health += healing;
            GameManager.instance.ShowText(healing.ToString(), 20, Color.green, transform.position, Vector3.up*25, 1.0f);
            if (health > max_health)
            {
                health = max_health;
            }
        }
    }

    protected virtual void Death()
    {

    }
}
