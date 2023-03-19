using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;    // The target to follow
    public GameObject cameraObject;   // The camera object to move
    public float smoothing = 5f;  // The smoothing factor

    private Vector3 offset;  // The initial offset between the camera and the target

    void Start()
    {
        // Calculate the offset between the camera and the target
        offset = cameraObject.transform.position - target.position;
    }

    void FixedUpdate()
    {
        // Calculate the target position based on the target's position and the offset
        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera towards the target position
        cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}
