using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : EnemyBase
{
    float timeSinceShoot;
    float waitBetweenShots = 2f; // seconds
    public Transform shotsContainer;
    public GameObject projectilePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Setup();     
    }

    void OnCollisionEnter2D(Collision2D other) {
        // ???
    }

    void OnTriggerEnter2D(Collider2D other) {
        BaseTriggerChecks(other);
        // ???
    }

    public override void Act()
    {
        timeSinceShoot += Time.deltaTime;
        if (timeSinceShoot >= waitBetweenShots) {
            timeSinceShoot = 0;
            Shoot();
        }
        foreach (Transform shot in shotsContainer) {
            shot.position += new Vector3(0f,3f*Time.deltaTime,0f);
        }
        // keeps itself upright
        transform.rotation = Quaternion.identity;
    }

    void Shoot()
    {
        GameObject go = Instantiate(projectilePrefab,Vector3.zero,Quaternion.identity,shotsContainer);
        go.transform.localPosition = Vector3.zero;
    }
}
