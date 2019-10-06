using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    CanvasGroup cg;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        MouseLeft();
    }
    
    public void MouseEntered()
    {
        cg.alpha = 1f;
    }

    public void MouseLeft()
    {
        cg.alpha = 0.7f;
    }
}
