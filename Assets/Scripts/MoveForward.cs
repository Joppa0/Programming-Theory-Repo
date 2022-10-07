using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private PlayerController player;

    private float speed = 20.0f;

    private float maxPosition = 20;

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
        Vector3 playerPos = GameObject.Find("Player").transform.position;

        //Moves the bullet forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        //Heat seeking function goes here
        //for loop checks for closest enemy
        //If the distance is lower than the current value, set distance to that enemy as new value
        //Translates bullet towards closest enemy
        
        //Destroys bullets who move too far away from player
        if (transform.position.x > maxPosition + playerPos.x || transform.position.x < -maxPosition + playerPos.x || transform.position.y > maxPosition + playerPos.z || transform.position.z < -maxPosition + playerPos.z)
        {
            Destroy(gameObject);
        }
    }
}
