using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    protected Rigidbody enemyRb;
    protected GameObject player;
    
    protected MainManager mainManager;
    protected PlayerController playerController;

    protected int pointValue = 5;
    protected int life = 1;
    protected int damage = 1;

    protected NavMeshAgent nav;

    public GameObject bulletImpactPrefab;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected void Update()
    {
        Move();
    }

    protected void OnTriggerEnter(Collider other)
    {
        //If the enemy collides with a bullet, destroy the bullet and enemy, add points to the instance and spawn the vfx
        if (other.CompareTag("Bullet"))
        {
            Instantiate(bulletImpactPrefab, transform.position, transform.rotation);
            Destroy(other.gameObject);
            life--;

            if (life < 1)
            {
                Destroy(gameObject);
                mainManager.AddPoint(pointValue);
            }
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destroy the enemy and reduce the player's life if the two collide
            Destroy(gameObject);
            playerController.life -= damage;
        }
    }

    //Move function can be overriden by child classes to change the speed
    protected virtual void Move()
    {
        if (!mainManager.m_GameOver)
        {
            //Sets the speed of the zombie and the destination as the player's position, meaning the zombie will move towards the player
            nav.speed = 3.0f;

            nav.SetDestination(player.transform.position);
        }
    }
}
