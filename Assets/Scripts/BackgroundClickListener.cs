using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClickListener : MonoBehaviour
{
    ToolsManager toolsManager;

    void Start()
    {
        toolsManager = FindObjectOfType<ToolsManager>();
    }
    
    void OnMouseDown()
    {
        toolsManager.ClickedBackground();
    }
}
