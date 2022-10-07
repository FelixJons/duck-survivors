using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
                // Die
            }
        }
    }
}
