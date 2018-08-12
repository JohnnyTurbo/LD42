using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour {

    public GameObject mainCam;
    public Vector3 offsetFromCam;

    private void FixedUpdate()
    {
        transform.position = mainCam.transform.position + offsetFromCam;
    }
}
