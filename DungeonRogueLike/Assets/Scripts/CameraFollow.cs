using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 Pos;
    public Transform player;
    public float distance = 3;
    public float height = 2;
    public float smoothTime = 0.25f;

    Vector3 currentVelocity;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        

    }

    void LateUpdate()
    {
        transform.position = player.position + Pos;
        Vector3 target = player.position + (-player.transform.forward * distance);
        target += Vector3.up * height;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, smoothTime);
        transform.LookAt(player);
    }
}
