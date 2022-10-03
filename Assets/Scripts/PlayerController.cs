using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float verticalInput;

    public float horizontalInput;

    public int life = 5;

    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //Gets the vertical and horizontal input to see if the player is trying to move the character
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");

        //Gets the direction to move in
        Vector3 direction = new Vector3(horizontalInput * moveSpeed, 0, verticalInput * moveSpeed);

        //Transforms the local coordinates of the direction to world coordinates so that the character always moves in the same direction, regardless of it's rotation
        Vector3 worldCord = transform.InverseTransformDirection(direction);

        //Moves player character at a fixed rate
        transform.Translate(worldCord * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        //Fires ray from the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Plane to intersect with the ray
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        //Gets the distance of the ray if it has has intersected with a collider
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            //Gets the target point at the distance along the ray
            Vector3 target = ray.GetPoint(distance);

            //The direction to rotate towards
            Vector3 direction = target - transform.position;

            //Gets desired rotation
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            //Rotates transform towards mouse
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        //Instantiates a bullet if left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
}
