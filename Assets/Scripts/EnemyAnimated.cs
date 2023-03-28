using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimated : Mover
{
    public int exp_value = 1;
    public bool facing_right = true;
    protected bool moving = false;
    protected bool dead = false;
    protected float last_alive;
    protected Animator animator;
    protected bool chasing;
    protected bool horizontal_flip = false;
    protected bool colliding_with_player;
    protected NavMeshAgent agent;
    // Hitbox and targetting.
    public ContactFilter2D filter;
    protected Collider2D[] hits = new Collider2D[10];
    public BoxCollider2D hitbox;
    public CircleCollider2D detection_range;
    protected Transform target_transform = null;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        // Push the enemy around if they have been pushed.
        if (push_direction.magnitude > 0 && !dead)
        {
            UpdateMotor(Vector3.zero);
        }
        if (target_transform == null)
        {
            if (chasing)
            {
                chasing = false;
            }
            detection_range.OverlapCollider(filter, hits);
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j] == null)
                {
                    continue;
                }
                if (hits[j].tag == "Fighter")
                {
                    target_transform = hits[j].transform;
                    return;
                }
                hits[j] = null;
            }
        }
        // Check if the player is in range.
        else if (target_transform != null)
        {
            if (!chasing)
            {
                chasing = true;
            }

            if (chasing && !dead)
            {
                if (!colliding_with_player)
                {
                    agent.SetDestination(target_transform.position);
                    if (facing_right)
                    {
                        if (target_transform.position.x - transform.position.x < 0 && !horizontal_flip)
                        {
                            horizontal_flip = true;
                            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                        }
                        else if (target_transform.position.x - transform.position.x > 0 && horizontal_flip)
                        {
                            horizontal_flip = false;
                            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                        }
                    }
                    else
                    {
                        if (target_transform.position.x - transform.position.x > 0 && !horizontal_flip)
                        {
                            horizontal_flip = true;
                            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                        }
                        else if (target_transform.position.x - transform.position.x < 0 && horizontal_flip)
                        {
                            horizontal_flip = false;
                            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                        }
                    }
                }
            }
        }

        colliding_with_player = false;
        hitbox.OverlapCollider(filter,hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            if (hits[i].tag == "Fighter")
            {
                colliding_with_player = true;
            }

            // Clear the array after you're done.
            hits[i] = null;
        }
        // While chasing the player, use the move animation.
        if (chasing && !moving && !dead)
        {
            moving = true;
            animator.SetBool("Moving", moving);
        }
        // If they reach the player, stop using the move animation and switch to the attack animation.
        if (colliding_with_player && moving && !dead)
        {
            Attack();
        }
        if (dead)
        {
            if (Time.time - last_alive > 3.0f)
            {
                GameManager.instance.GrantExp(exp_value);
                Destroy(gameObject);
            }
        }
    }

    protected virtual void Attack()
    {
        moving = false;
        animator.SetBool("Moving", moving);
        animator.SetTrigger("Attack");
    }

    protected override void Death()
    {
        if (!dead)
        {
            dead = true;
            last_alive = Time.time;
            animator.SetBool("Moving", false);
            animator.SetTrigger("Dead");
        }
    }
}
