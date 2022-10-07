using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float yOffset = 10f;
    [SerializeField] float moveSpeed = 2f;

    private Func<Vector3> GetCameraFollowPositionFunc;

    public void Setup(Func<Vector3> GetCameraFollowPositionFunc)
    {
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    private void LateUpdate()
    {
        //Locks camera on y position
        transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
    }

    
    private void MoveCamera()
    {
        //Moves the camera to follow the player with a short distance between them
        Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();

        Vector3 cameraMoveDir = (player.transform.position - transform.position).normalized;

        float distance = Vector3.Distance(player.transform.position, transform.position);
       
        transform.position = transform.position + cameraMoveDir * distance * moveSpeed * Time.deltaTime;
    }
}
