using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : EnemyBase
{
    float timeSinceShoot;
    float waitBetweenShots = 2f; // seconds
    public Transform shotsContainer;
    public GameObject projectilePrefab;
    bool shootingRight = true;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Setup();
    }

    void OnCollisionEnter2D(Collision2D other) {
        base.BaseCollisionChecks(other);
    }

    void OnTriggerEnter2D(Collider2D other) {
        BaseTriggerChecks(other);
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
    }

    public override void Die(){}

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
}
