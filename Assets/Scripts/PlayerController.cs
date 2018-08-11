using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject mainLine;
    public float horizontalOffset;

    float playerSpeed;
    new Rigidbody2D rigidbody2D;
    
    LineController mainLineController;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //transform.position += (playerSpeed * Time.deltaTime * Vector3.right);
        rigidbody2D.position = new Vector3(mainLine.transform.position.x + horizontalOffset, transform.position.y, transform.position.z);
    }
}
