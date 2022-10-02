using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 1;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health <= 0)
            {
                print(health);
                Kill();
            }
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}