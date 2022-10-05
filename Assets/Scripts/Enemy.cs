using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb2;
    [SerializeField] private Transform player;
    [SerializeField] private int health = 1;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float attackSpeed = 1f;
    private float attackTimer;
    [SerializeField] private int damage = 1;
    private bool isTouchingPlayer;
    private bool isFacingRight = true;

    private void OnEnable()
    {
        attackTimer = 0;
        isTouchingPlayer = false;
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

        float angleBetweenEnemyAndPlayer = Mathf.Atan2(player.position.y - transform.position.y,
            player.position.x - transform.position.x);

        print(angleBetweenEnemyAndPlayer);

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
        print("Flip flip");
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
        transform.position = Vector2.MoveTowards(transform.position, player.position,
            movementSpeed * Time.deltaTime);
    }
}