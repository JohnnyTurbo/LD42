using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject mainLine;
    public float horizontalOffset;
    public float maxSpped;

    new Rigidbody2D rigidbody2D;
    
    TrailRenderer mainLineTrail;

    private void Awake()
    {
        mainLine = GameObject.Find("Line");
        rigidbody2D = GetComponent<Rigidbody2D>();
        mainLineTrail = mainLine.GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        Vector2 targetPos = new Vector2(mainLine.transform.position.x + horizontalOffset, rigidbody2D.position.y);
        //rigidbody2D.position = Vector2.MoveTowards(rigidbody2D.position, targetPos, maxSpped);
        rigidbody2D.position = targetPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Planet")
        {
            Debug.Log("Collision with planet", gameObject);
            GameController.instance.LifeLost();
        }
        else if(collision.gameObject.tag == "Powerup")
        {
            Debug.Log("Collision with powerup", gameObject);
            GameController.instance.CollectPowerUp();
            Destroy(collision.gameObject);
        }
    }
}
