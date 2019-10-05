using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float speed = 2f;
    Rigidbody2D rb;
    bool goingRight = true;
    float originalGravity;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        SwitchToEdit();
    }

    public void SwitchToEdit()
    {
        rb.gravityScale = 0f;
    }

    public void SwitchToGame()
    {
        rb.gravityScale = originalGravity;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag.Equals("obstacle")) {
            Flip();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("platformedge")) {
            Flip();
        }
    }

    void Flip()
    {
        goingRight = !goingRight;
    }

    public void Move()
    {
        float mult = goingRight ? 1f : -1f;
        transform.position += new Vector3(1.5f*mult*Time.deltaTime,0f,0f);
    }
}
