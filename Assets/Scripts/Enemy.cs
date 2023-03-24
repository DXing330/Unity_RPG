using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Mover
{
    public int exp_value = 1;
    // Logic.
    // How close until the enemy chases.
    public float trigger_length = 0.5f;
    // How far the enemy chases before returning.
    public float chase_length = 2;
    private bool chasing;
    private bool colliding_with_player;
    private Transform player_transform;
    private Vector3 starting_position;
    private NavMeshAgent agent;

    // Hitbox.
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        player_transform = GameManager.instance.player.transform;
        starting_position = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {
        // Push the enemy around if they have been pushed.
        if (push_direction.magnitude > 0)
        {
            UpdateMotor(Vector3.zero);
        }
        // Check if the player is in range.
        if (Vector3.Distance(player_transform.position, transform.position) < chase_length)
        {
            if (Vector3.Distance(player_transform.position, transform.position) < trigger_length)
            {
                chasing = true;
            }

            if (chasing)
            {
                if (!colliding_with_player)
                {
                    agent.SetDestination(player_transform.position);
                }
            }
            else
            {
                agent.SetDestination(starting_position);
            }
        }
        else
        {
            agent.SetDestination(starting_position);
            chasing = false;
        }

        colliding_with_player = false;
        boxCollider.OverlapCollider(filter,hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            if (hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                colliding_with_player = true;
            }

            // Clear the array after you're done.
            hits[i] = null;
        }
    }

    protected override void Death()
    {
        GameManager.instance.GrantExp(exp_value);
        Destroy(gameObject);
    }
}
