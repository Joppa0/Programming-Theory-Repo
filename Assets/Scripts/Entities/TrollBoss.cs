using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrollBoss : EnemyController
{
    protected SpawnManager spawnManager;

    protected override void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        playerAudio = GameObject.Find("Player").GetComponent<AudioSource>();

        pointValue *= 50;
        life = 25;
        damage = 4;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destroy the enemy, reduce the player's life and spawn the blood vfx they collide

            Instantiate(playerTakeHitEffect, player.transform.position, Quaternion.identity);
            playerAudio.PlayOneShot(hitPlayerSound, 0.1f);
            Destroy(gameObject);
            playerController.life -= damage;
        }

        //If the enemy collides with a bullet, destroy the bullet and enemy, add points to the instance and spawn the vfx and the health increase powerup
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(bulletImpactPrefab, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            life--;

            if (life < 1)
            {
                Destroy(gameObject);
                spawnManager.SpawnHealthIncrease(transform.position);
                mainManager.AddPoint(pointValue);
            }
        }
    }
}
