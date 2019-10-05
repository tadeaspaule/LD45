using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    bool inJump = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name.Equals("end")) {
            Debug.Log("Reached the end");
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log($"Collided with {other.gameObject.name} at {Time.time}");
        if (other.gameObject.name.StartsWith("platform")) {
            inJump = false;
        }
    }

    #region Movement

    public void MoveLeft()
    {
        Move(-1f);
    }

    public void MoveRight()
    {
        Move(1f);
    }

    public void Move(float modifier)
    {
        transform.position += new Vector3(2.5f*modifier*Time.deltaTime,0f,0f);
    }

    public void Jump()
    {
        if (inJump) return;
        rb.AddForce(new Vector2(0f,75f));
        inJump = true;
    }

    #endregion
}
