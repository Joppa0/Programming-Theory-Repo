using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private PlayerController player;

    private float speed = 20.0f;

    private float maxPosition = 20;

    private bool bulletCanHeatSeek = false;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //ABSTRACTION
    private void Move()
    {
        float rotationSpeed = 0.1f;
        float lowestDistance = 100;
        float distance = 0;
        Vector3 enemyPos;
        Vector3 moveDir = new Vector3(0, 0, 0);

        //Makes bullets seek out the enemy if the bullet is able to heat seek
        if (player.HasHeatSeeking)
        {
            StartCoroutine(TimeUntilHeatSeeking());

            if (bulletCanHeatSeek)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < enemies.Length; i++)
                {
                    distance = Vector3.Distance(transform.position, enemies[i].transform.position);
                    if (distance < lowestDistance)
                    {
                        lowestDistance = distance;
                        enemyPos = enemies[i].transform.position;
                        moveDir = (enemies[i].transform.position - transform.position).normalized;
                    }
                }
                float rotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;

                if (lowestDistance < 2)
                {
                    transform.rotation = Quaternion.Euler(90, rotation, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, rotation, 0), rotationSpeed);
                }
            }
            lowestDistance = 100;
        }

        //Moves the bullet forward
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        Vector3 playerPos = GameObject.Find("Player").transform.position;

        //Destroys bullets who move too far away from player
        if (transform.position.x > maxPosition + playerPos.x || transform.position.x < -maxPosition + playerPos.x || transform.position.z > maxPosition + playerPos.z || transform.position.z < -maxPosition + playerPos.z)
        {
            Destroy(gameObject);
        }
    }

    //Sets a cooldown after which bullets can start heatseeking
    IEnumerator TimeUntilHeatSeeking()
    {
        yield return new WaitForSeconds(0.1f);
        bulletCanHeatSeek = true;
    }
}
