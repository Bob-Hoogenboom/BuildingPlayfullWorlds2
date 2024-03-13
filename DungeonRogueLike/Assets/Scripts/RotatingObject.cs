using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
