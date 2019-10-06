using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    float speed = 2f;
    bool goingRight = true;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Setup();     
    }

    void OnCollisionEnter2D(Collision2D other) {
        BaseCollisionChecks(other);
        if (other.gameObject.tag.Equals("obstacle")) {
            Flip();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        BaseTriggerChecks(other);
        if (other.gameObject.tag.Equals("platformedge")) {
            Flip();
        }
    }

    void Flip()
    {
        goingRight = !goingRight;
    }

    public override void Die()
    {
        Debug.Log("Normal Enemy die popped");
    }

    public override void Act()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y); // fixes a weird bug where player is dragged to the right
        float mult = goingRight ? 1f : -1f;
        transform.position += new Vector3(speed*mult*Time.deltaTime,0f,0f);
    }
}
