using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public BoxCollider2D gameCollider;
    
    void Start()
    {
    }
    
    public void SwitchToEdit()
    {
        gameCollider.enabled = false;
    }

    public void SwitchToGame()
    {
        gameCollider.enabled = true;
    }
}
