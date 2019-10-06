using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemyBase
{    
    float speed = 2f;
    bool goingRight = true;

    void Start()
    {
        base.Setup();
    }
    
    public override void Die()
    {
        Debug.Log("Skeleton die");
        foreach (Transform child in transform) {
            GameObject bone = Instantiate(child.gameObject,Vector3.zero,Quaternion.identity,gameManager.levelContainer);
            bone.AddComponent<Rigidbody2D>();
            bone.AddComponent<BoxCollider2D>();
            bone.name = "bone";
            bone.transform.position = transform.position;
            bone.transform.localScale = transform.parent.localScale;
        }
    }

    public override void Act()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y); // fixes a weird bug where player is dragged to the right
        float mult = goingRight ? 1f : -1f;
        transform.position += new Vector3(speed*mult*Time.deltaTime,0f,0f);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (inEdit) return;
        BaseCollisionChecks(other);
        if (other.gameObject.tag.Equals("obstacle")) {
            Flip();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (inEdit) return;
        BaseTriggerChecks(other);
        if (other.gameObject.tag.Equals("platformedge")) {
            Flip();
        }
    }

    public override void Flip()
    {
        goingRight = !goingRight;
        if (goingRight) {
            transform.rotation = Quaternion.identity;
        }
        else {
            transform.rotation = Quaternion.Euler(0f,180f,0f);
        }
    }
}
