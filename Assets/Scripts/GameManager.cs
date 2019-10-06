using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameEditCoordinator gameEditCoordinator;
    [HideInInspector]
    public int level;
    public Transform levelContainer;
    
    Player player;
    Vector3 originalPlayerPosition;

    private class EnemyWithPosition
    {
        public EnemyBase enemy;
        public Vector3 originalPosition;

        public EnemyWithPosition(EnemyBase enemy, Vector3 pos)
        {
            this.enemy = enemy;
            this.originalPosition = pos;
        }
    }

    List<EnemyWithPosition> enemies = new List<EnemyWithPosition>();
    List<Platform> platforms = new List<Platform>();

    List<string> leftMoves = new List<string>(new string[]{"left", "a"});
    List<string> rightMoves = new List<string>(new string[]{"right", "d"});
    List<string> upMoves = new List<string>(new string[]{"up", "space", "w"});
    
    // Start is called before the first frame update
    void OnEnable()
    {
        // 
    }

    public void Setup(Transform levelContainer)
    {
        this.levelContainer = levelContainer;
        foreach (Transform child in levelContainer) {
            if (child.gameObject.name.StartsWith("player")) {
                player = child.GetComponentInChildren<Player>();
                player.SwitchToGame();
                originalPlayerPosition = child.position;
                player.gameManager = this;
                Debug.Log("Found player");
            }
            else if (child.gameObject.name.StartsWith("enemy")) {
                EnemyBase enemy = child.GetComponentInChildren<EnemyBase>();
                enemy.gameManager = this;
                enemies.Add(new EnemyWithPosition(enemy,child.position));
                enemy.SwitchToGame();
                Debug.Log("Found an enemy");
            }
            // else if (child.gameObject.name.StartsWith("platform")) {
            //     child.GetComponentInChildren<Platform>().SwitchToGame();
            // }
        }
    }

    public void SwitchToEdit()
    {
        ResetPositions();
        player.SwitchToEdit();
        foreach (EnemyWithPosition ewp in enemies) {
            ewp.enemy.SwitchToEdit();
        }
        // delete all bones spawned by dying skeletons
        foreach (Transform child in levelContainer) {
            if (child.gameObject.name.Equals("bone")) {
                Destroy(child.gameObject);
            }
        }
        
        // foreach (Platform platform in platforms) {
        //     platform.SwitchToEdit();
        // }
    }

    public void EnemyDied(EnemyBase enemy)
    {
        Debug.Log($"Enemy died called with object {enemy.gameObject.name}");
        enemy.Die();
        // enemies.RemoveAll(ep => ep.enemy.Equals(enemy));
        enemy.gameObject.SetActive(false);
    }

    public void PlayerDied()
    {
        Debug.Log("Player died");
        player.inJump = false;
        // reset positions
        ResetPositions();
    }

    public void ResetPositions()
    {
        player.transform.position = originalPlayerPosition;
        Rigidbody2D rbp = player.gameObject.GetComponent<Rigidbody2D>();
        rbp.inertia = 0f;
        rbp.velocity = Vector2.zero;
        foreach (EnemyWithPosition ewp in enemies) {
            ewp.enemy.gameObject.SetActive(true);
            Rigidbody2D rb = ewp.enemy.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null) {
                rb.inertia = 0f;
                rb.velocity = Vector2.zero;
            }
            ewp.enemy.transform.position = ewp.originalPosition + new Vector3(0f,0.3f,0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool foundPlayerMovement = false;
        foreach (string keyName in leftMoves) {
            if (Input.GetKey(keyName)) {
                player.MoveLeft();
                foundPlayerMovement = true;
                break;
            }
        }
        foreach (string keyName in rightMoves) {
            if (Input.GetKey(keyName)) {
                player.MoveRight();
                foundPlayerMovement = true;
                break;
            }
        }
        foreach (string keyName in upMoves) {
            if (Input.GetKey(keyName)) {
                foundPlayerMovement = true;
                player.Jump();
                break;
            }
        }
        if (!foundPlayerMovement) player.NotMoving();
        foreach (EnemyWithPosition ewp in enemies) {
            if (ewp.enemy.gameObject != null && ewp.enemy.gameObject.activeSelf) ewp.enemy.Act();
        }
    }    
}
