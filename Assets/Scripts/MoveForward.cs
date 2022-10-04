using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private float speed = 20.0f;

    private float maxPosition = 75.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //ABSTRACTION
    private void Move()
    {
        //Moves the bullet forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        //Destroys out of bounds bullets
        if (transform.position.x > maxPosition || transform.position.x < -maxPosition || transform.position.y > maxPosition || transform.position.z < -maxPosition)
        {
            Destroy(gameObject);
        }
    }
}
