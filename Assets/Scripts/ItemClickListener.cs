using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClickListener : MonoBehaviour
{
    EditorManager editorManager;
    
    // Start is called before the first frame update
    void Start()
    {
        editorManager = FindObjectOfType<EditorManager>();
    }

    void OnMouseDown()
    {
        editorManager.PlacedItemClicked(this.gameObject);  
    }
}
