﻿using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 50;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            Destroy(gameObject);
        }
    }
}