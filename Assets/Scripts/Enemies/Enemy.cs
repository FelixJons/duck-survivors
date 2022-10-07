using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] private int health = 1;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int damage = 1;

    private float attackTimer;
    private Rigidbody2D rb2;
    private bool isTouchingPlayer;
    private bool isFacingRight;

    private void OnEnable()
    {
        attackTimer = 0;
        isTouchingPlayer = false;
        // Must be true as of now since the enemies spawns facing right
        isFacingRight = true;
    }

    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health <= 0)
            {
                print(health);
                Destroy(gameObject);
            }
        }
    }

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        // Returns the angle between the zero vector and the vector that ends at vector made up by the
        // parameters (x,y). Can handle perpendicular angles and avoid division by zero exceptions. 
        float angleBetweenEnemyAndPlayer = Mathf.Atan2(player.position.y - transform.position.y,
            player.position.x - transform.position.x);
        
        switch (Mathf.Abs(angleBetweenEnemyAndPlayer))
        {
            case > Mathf.PI / 2.0f when isFacingRight:
            case <= Mathf.PI / 2.0f when !isFacingRight:
                Flip();
                break;
        }

        if (!isTouchingPlayer)
        {
            Move();
        }
    }

    private void Flip()
    {
        transform.eulerAngles = isFacingRight ? new Vector3(0, 180) : new Vector3(0, 0);
        isFacingRight = !isFacingRight;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            if (attackTimer >= attackSpeed)
            {
                attackTimer = 0;
                other.gameObject.GetComponent<Player>().Health -= damage;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    private void Move()
    {
        rb2.velocity = (player.position - transform.position).normalized * movementSpeed;
    }
}