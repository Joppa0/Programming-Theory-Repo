using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Rigidbody enemyRb;
    protected GameObject player;
    public PlayerController playerController;
    public int pointValue = 5;

    public MainManager mainManager;

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
        Destroy(gameObject);
        Destroy(other.gameObject);
        mainManager.AddPoint(pointValue);
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
