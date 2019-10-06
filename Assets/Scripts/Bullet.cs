using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("player")) {
            transform.parent.parent.GetComponent<EnemyBase>().gameManager.PlayerDied();
            return;
        }
        else if (other.gameObject.name.StartsWith("platform")) {
            Destroy(this.gameObject);
        }
    }
}
