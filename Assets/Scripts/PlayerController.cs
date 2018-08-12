using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject mainLine;
    public float horizontalOffset;
    public float maxSpped;

    new Rigidbody2D rigidbody2D;
    
    TrailRenderer mainLineTrail;
    SpriteRenderer mySpriteRenderer;
    Collider2D myCollider2D;

    bool isInvincible;

    private void Awake()
    {
        mainLine = GameObject.Find("Line");
        rigidbody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        mainLineTrail = mainLine.GetComponent<TrailRenderer>();
        mySpriteRenderer = transform.Find("CharAnimator").GetComponent<SpriteRenderer>();
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
            if (isInvincible)
            {
                Debug.Log("But you're invincible", gameObject);
                Physics2D.IgnoreCollision(myCollider2D, collision.collider, true);
            }
            else
            {
                GameController.instance.LifeLost();
            }
        }
        else if(collision.gameObject.tag == "Boundary")
        {
            Debug.Log("Player is out of bounds!", gameObject);
            GameController.instance.LifeLost();
        }
        else if(collision.gameObject.tag == "Powerup")
        {
            Debug.Log("Collision with powerup", gameObject);
            GameController.instance.CollectPowerUp();
            Destroy(collision.gameObject);
        }
    }

    public void MakeInvincible(float invincibilityTime)
    {
        StartCoroutine(Invincibility(invincibilityTime, true));
    }

    public void UsePowerup(float invincibilityTime)
    {
        StopAllCoroutines();
        StartCoroutine(Invincibility(invincibilityTime, true));
    }

    IEnumerator Invincibility(float invincibilityTime, bool isFlashing)
    {
        float flashRate = 0.07f;

        isInvincible = true;
        float endTime = Time.time + invincibilityTime;
        while(Time.time < endTime)
        {
            if (isFlashing)
            {
                mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
            }
            yield return new WaitForSeconds(flashRate);
        }
        mySpriteRenderer.enabled = true;
        isInvincible = false;
        Debug.Log("end invincibility");
    }
}
