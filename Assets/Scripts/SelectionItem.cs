using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionItem : MonoBehaviour
{    
    Image image;
    ToolsPanel toolsPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        toolsPanel = FindObjectOfType<ToolsPanel>();
        MouseLeft();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseEntered()
    {
        transform.localScale = new Vector3(1.1f,1.1f,1.1f);
        image.color = Color.white;
        toolsPanel.UpdateHoverText(this.name);
    }

    public void MouseLeft()
    {
        transform.localScale = Vector3.one;
        image.color = new Color(0.8f,0.8f,0.8f);
        toolsPanel.UpdateHoverText("");
    }
}
