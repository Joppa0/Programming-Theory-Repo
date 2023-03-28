using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected AudioClip hitPlayerSound;

    protected Animator animator;
    protected Rigidbody enemyRb;
    protected GameObject player;
    
    protected MainManager mainManager;
    protected PlayerController playerController;

    protected int pointValue = 5;
    protected int life = 1;
    protected int damage = 1;

    protected NavMeshAgent nav;

    public GameObject bulletImpactPrefab;
    public GameObject playerTakeHitEffect;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Gives the enemy a random priority, leading to less blocking between enemies.
        nav.avoidancePriority = UnityEngine.Random.Range(0, 100);
    }

    void Update()
    {
        Move();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destroy the enemy, reduce the player's life and spawn the blood vfx they collide.
            Instantiate(playerTakeHitEffect, player.transform.position, Quaternion.identity);
            SoundManager.instance.PlaySound(hitPlayerSound, 0.1f);
            Destroy(gameObject);
            playerController.life -= damage;
        }

        // If the enemy collides with a bullet, destroy the bullet and enemy, add points to the instance and spawn the vfx.
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(bulletImpactPrefab, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            life--;

            if (life < 1)
            {
                Destroy(gameObject);
                mainManager.AddPoint(pointValue);
            }
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Console.WriteLine("Collided");
        }
    }

    // Moves the enemy towards player as long as game isn't over.
    protected void Move()
    {
        if (!mainManager.m_GameOver)
        {
            // Sets the destination as the player's position, meaning the zombie will move towards the player.

            nav.SetDestination(player.transform.position);
        }
        else
        {
            nav.ResetPath();
        }
    }
}
