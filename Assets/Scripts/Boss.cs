using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float[] fireballSpeed = {1.0f, -1.0f};
    public float distance = 0.4f;
    public Transform[] fireballs;
    public int mana_crystals = 5;

    private void Update()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time*fireballSpeed[i])*distance, Mathf.Sin(Time.time*fireballSpeed[i])*distance, 0);
        }
    }

    protected override void Death()
    {
        base.Death();
        GameManager.instance.GrantMana(mana_crystals);
    }
}
