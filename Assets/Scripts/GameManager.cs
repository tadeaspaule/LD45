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
        public Enemy enemy;
        public Vector3 originalPosition;

        public EnemyWithPosition(Enemy enemy, Vector3 pos)
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
            if (child.gameObject.name.Equals("player")) {
                player = child.GetComponent<Player>();
                player.SwitchToGame();
                originalPlayerPosition = child.position;
                player.gameManager = this;
                Debug.Log("Found player");
            }
            else if (child.gameObject.name.StartsWith("enemy")) {
                Enemy enemy = child.GetComponent<Enemy>();
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
        // foreach (Platform platform in platforms) {
        //     platform.SwitchToEdit();
        // }
    }

    public void EnemyDied(Enemy enemy)
    {
        // enemies.RemoveAll(ep => ep.enemy.Equals(enemy));
        enemy.gameObject.SetActive(false);
    }

    public void PlayerDied()
    {
        // reset positions
        ResetPositions();
    }

    void ResetPositions()
    {
        player.transform.position = originalPlayerPosition;
        Rigidbody2D rbp = player.gameObject.GetComponent<Rigidbody2D>();
        rbp.inertia = 0f;
        rbp.velocity = Vector2.zero;
        foreach (EnemyWithPosition ewp in enemies) {
            ewp.enemy.gameObject.SetActive(true);
            Rigidbody2D rb = ewp.enemy.gameObject.GetComponent<Rigidbody2D>();
            rb.inertia = 0f;
            rb.velocity = Vector2.zero;
            ewp.enemy.transform.position = ewp.originalPosition + new Vector3(0f,0.3f,0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (string keyName in leftMoves) {
            if (Input.GetKey(keyName)) {
                player.MoveLeft();
                break;
            }
        }
        foreach (string keyName in rightMoves) {
            if (Input.GetKey(keyName)) {
                player.MoveRight();
                break;
            }
        }
        foreach (string keyName in upMoves) {
            if (Input.GetKey(keyName)) {
                player.Jump();
                break;
            }
        }
        foreach (EnemyWithPosition ewp in enemies) {
            if (ewp.enemy.gameObject.activeSelf) ewp.enemy.Move();
        }
    }    
}
