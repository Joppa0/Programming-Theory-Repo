using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private PlayerController player;

    private float speed = 20.0f;

    private float maxPosition = 20;

    private float rotationSpeed = 0.05f;
    private float rotationSpeedClose = 1f;
    private Vector3 enemyPos;
    private Vector3 moveDir = new Vector3(0, 0, 0);
    [SerializeField] private float lowestDistance = 100;
    [SerializeField] private float distance;

    private bool bulletCanHeatSeek = false;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //ABSTRACTION
    private void Move()
    {
        if (player.hasHeatSeeking)
        {
            StartCoroutine(TimeUntilHeatSeeking());

            if (bulletCanHeatSeek)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < enemies.Length; i++)
                {
                    distance = Vector3.Distance(transform.position, enemies[i].transform.position);
                    Debug.Log(distance);
                    if (distance < lowestDistance)
                    {
                        lowestDistance = distance;
                        enemyPos = enemies[i].transform.position;
                        moveDir = (enemies[i].transform.position - transform.position).normalized;
                    }
                }
                float rotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;

                if (distance < 3)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), rotationSpeedClose);
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), rotationSpeed);
                }
            }
            lowestDistance = 100;
        }

        //Moves the bullet forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        Vector3 playerPos = GameObject.Find("Player").transform.position;

        //Destroys bullets who move too far away from player
        if (transform.position.x > maxPosition + playerPos.x || transform.position.x < -maxPosition + playerPos.x || transform.position.y > maxPosition + playerPos.z || transform.position.z < -maxPosition + playerPos.z)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator TimeUntilHeatSeeking()
    {
        yield return new WaitForSeconds(0.25f);
        bulletCanHeatSeek = true;
    }
}
