using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClickListener : MonoBehaviour
{
    EditorManager editorManager;
    public bool useParent;
    
    // Start is called before the first frame update
    void Start()
    {
        editorManager = FindObjectOfType<EditorManager>();
    }

    void OnMouseDown()
    {
        ItemClicked();
    }

    public void ItemClicked()
    {
        Debug.Log("Clicked an item");
        if (useParent) editorManager.PlacedItemClicked(transform.parent.gameObject); 
        else editorManager.PlacedItemClicked(this.gameObject);
    }
}
