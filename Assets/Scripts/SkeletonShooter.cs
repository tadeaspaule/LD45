using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonShooter : EnemyBase
{    
    float timeSinceShoot;
    float waitBetweenShots = 2f; // seconds
    public Transform shotsContainer;
    public GameObject projectilePrefab;
    bool shootingRight = true;

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
        timeSinceShoot += Time.deltaTime;
        if (timeSinceShoot >= waitBetweenShots) {
            timeSinceShoot = 0;
            Shoot();
        }
        foreach (Transform shot in shotsContainer) {
            Vector3 shotMove = new Vector3(3f*Time.deltaTime,0f,0f);
            if (!shootingRight) shotMove*=-1f;
            shot.position += shotMove;
        }
        // keeps itself upright
        transform.rotation = Quaternion.identity;
    }

    public override void Flip()
    {
        shootingRight = !shootingRight;
        if (shootingRight) {
            transform.rotation = Quaternion.identity;
        }
        else {
            transform.rotation = Quaternion.Euler(0f,180f,0f);
        }
    }

    void Shoot()
    {
        GameObject go = Instantiate(projectilePrefab,Vector3.zero,Quaternion.identity,shotsContainer);
        go.transform.localPosition = Vector3.zero;
    }

    void OnCollisionEnter2D(Collision2D other) {
        BaseCollisionChecks(other);
    }

    void OnTriggerEnter2D(Collider2D other) {
        BaseTriggerChecks(other);
    }
}
