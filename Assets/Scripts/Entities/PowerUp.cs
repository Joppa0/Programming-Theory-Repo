using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PowerUp : MonoBehaviour
{
    private MainManager mainManager;
    private PlayerController playerController;
    private GameObject player;

    [SerializeField] int powerupID;
    private float moveSpeed = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        MoveToPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Checks which powerup the player has collided with, then calls the appropriate function
            switch (powerupID)
            {
                case 0:
                    playerController.OnHealthPickupEnter();
                    break;

                case 1:
                    playerController.OnSpeedBoostEnter();
                    break;

                case 2:
                    playerController.OnTripleShotEnter();
                    break;

                case 3:
                    playerController.OnSlowMotionEnter();
                    break;

                case 4:
                    playerController.OnHeatSeekingEnter();
                    break;
                case 5:
                    playerController.OnHealthIncreaseEnter();
                    break;
            }
            Destroy(gameObject);
        }
    }

    private void MoveToPlayer()
    {
        if (!mainManager.m_GameOver)
        {
            Vector3 moveDir = (player.transform.position - transform.position).normalized;
            moveDir.y = 0;

            int layer = 7;

            int layerMask = 1 << layer;

            float radius = 0.6f;

            Vector3 finalPos = transform.position + (moveDir * moveSpeed * Time.deltaTime);

            Collider[] collisions = Physics.OverlapSphere(finalPos, radius, layerMask);

            if (collisions.Length == 0)
            {
                transform.Translate(moveDir * moveSpeed * Time.deltaTime);
            }
        }
    }
}