using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Vector3 cameraOffset;
    public Transform objectToFollow;

    void LateUpdate()
    {
        Vector3 newPos = new Vector3(objectToFollow.position.x, 0, 0);
        transform.position = newPos + cameraOffset;
    }
}
