using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClickListener : BaseClickListener
{
    ToolsManager toolsManager;

    void Start()
    {
        toolsManager = FindObjectOfType<ToolsManager>();
    }
    
    void OnMouseDown()
    {
        if (IsPointerOverUIObject()) return;
        toolsManager.ClickedBackground();
    }
}
