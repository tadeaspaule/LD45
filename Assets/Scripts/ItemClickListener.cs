using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClickListener : MonoBehaviour
{
    ToolsManager toolsManager;
    
    // Start is called before the first frame update
    void Start()
    {
        toolsManager = FindObjectOfType<ToolsManager>();
    }

    void OnMouseDown()
    {
        toolsManager.PlacedItemClicked(this.gameObject);  
    }
}
