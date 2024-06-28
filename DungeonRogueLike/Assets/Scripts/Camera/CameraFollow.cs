using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        

    }

    void LateUpdate()
    {
        float x = player.position.x + offset.x;
        float y = player.position.y + offset.y; 
        float z = player.position.z + offset.z;

        Vector3 newPosition = new Vector3(x, y, z);

        transform.position = newPosition;
    }
}
