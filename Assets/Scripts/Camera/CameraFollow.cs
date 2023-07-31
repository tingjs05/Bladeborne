using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // game object to follow
    public GameObject targetObject;
    public float followSpeed = 1.5f;

    // define variables
    private Vector3 targetPosition;

    void Update()
    {
        // update target position
        targetPosition = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y, transform.position.z);

        // move camera if not in target position
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.Slerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
