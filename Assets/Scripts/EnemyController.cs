using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Rigidbody enemyRb;
    protected GameObject player;
    public PlayerController playerController;

    // Start is called before the first frame update
    protected void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    protected void Update()
    {
        Move();
    }

    protected void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Destroy(other.gameObject);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            playerController.life--;
            Debug.Log(playerController.life);
        }
    }

    protected virtual void Move()
    {
        float speed = 2.0f;

        Vector3 lookdirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookdirection * speed);
    }
}
