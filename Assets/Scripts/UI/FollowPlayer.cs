using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private float yOffset = 3;
    private float zOffset = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayerObject();
    }

    //Sets position to player position with offset
    void FollowPlayerObject()
    {
        Vector3 position = player.transform.position;

        position.y += yOffset;
        position.z += zOffset;

        transform.position = position;
    }
}
