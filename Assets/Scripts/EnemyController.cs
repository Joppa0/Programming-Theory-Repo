using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Rigidbody enemyRb;
    protected GameObject player;

    public MainManager mainManager;
    public PlayerController playerController;

    private int pointValue = 5;

    // Start is called before the first frame update
    protected void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
    }

    // Update is called once per frame
    protected void Update()
    {
        Move();
    }

    protected void OnTriggerEnter(Collider other)
    {
        //If the enemy collides with a bullet, destroy the bullet and enemy, and add points to the instance
        Destroy(gameObject);
        Destroy(other.gameObject);
        mainManager.AddPoint(pointValue);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destroy the enemy and reduce the player's life if the two collide
            Destroy(gameObject);
            playerController.life--;
        }
    }

    //Move function can be overriden by child classes to change the speed
    protected virtual void Move()
    {
        if (!mainManager.m_GameOver)
        {
            float speed = 2.0f;

            //Finds the vector3 direction to look in
            Vector3 lookdirection = (player.transform.position - transform.position).normalized;

            //Gets desired rotation
            float rotation = Mathf.Atan2(lookdirection.x, lookdirection.z) * Mathf.Rad2Deg;

            //Rotates transform towards player
            transform.rotation = Quaternion.Euler(0, rotation, 0);

            //Moves enemy forward, meaning it's always towards the player
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
