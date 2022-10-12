using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
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

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = transform.position.x + (Random.Range(-1f, 1f) * magnitude);
            float z = transform.position.z + (Random.Range(-1f, 1f) * magnitude);

            transform.position = new Vector3(x, originalPos.y, z);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
