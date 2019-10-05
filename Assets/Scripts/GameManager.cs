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
                Debug.Log("Found player");
                break;
            }
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
    }

    
}
