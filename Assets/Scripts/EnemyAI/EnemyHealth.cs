﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public float _MaxHealth = 100;

    public float MaxHealth { get { return _MaxHealth; } set { _MaxHealth = value; } }
    public float Health { get { return health; } }
    public Teams team { get { return Teams.enemyTeam; } }

    private float health;

    public GameObject resourceDrop;

    private bool hasSpawned;

    public bool TakeDamage(float damage, Teams attackerTeam)
    {
        if (team == attackerTeam) 
        {
            Debug.Log("Friendly Hit :: EnemyHealth");

            // Friendly fire
            return false;
        }
        if (health - damage <= 0)
        {
            Debug.Log("Dead Hit :: EnemyHealth");
            UIManager.instance.DisplayToolTip("Enemy Defeated", 1);
            //Die
            if (!hasSpawned)
            {
                Instantiate(resourceDrop, transform.position, transform.rotation);
                GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>().Enemies.Remove(gameObject); //Remove enemy from enemies list so spawner knows it ded
                GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>().Avoids.Remove(gameObject.transform); //Remove enemy from enemies list so spawner knows it ded
                hasSpawned = true;
            }
            Destroy(gameObject);
            return true;
        }
        else
        {
            Debug.Log("Enemy Hit :: EnemyHealth");
            health -= damage;
            return false;
        }
    }

    public void Heal(float healAmount)
    {
        if (health + healAmount >= MaxHealth)
        {
            health = MaxHealth;
        }
        else
        {
            health += healAmount;
        }
    }

    void Awake()
    {
        health = MaxHealth;
        hasSpawned = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
