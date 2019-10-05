using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClickListener : BaseClickListener
{
    EditorManager editorManager;

    void Start()
    {
        editorManager = FindObjectOfType<EditorManager>();
    }
    
    void OnMouseDown()
    {
        if (IsPointerOverUIObject()) return;
        editorManager.ClickedBackground();
    }
}
