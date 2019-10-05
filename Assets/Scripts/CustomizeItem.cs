using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeItem : MonoBehaviour
{    
    Image image;
    ToolsManager tm;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        tm = FindObjectOfType<ToolsManager>();
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
        tm.UpdateCustomizeHoverText(this.name);
    }

    public void MouseLeft()
    {
        transform.localScale = Vector3.one;
        image.color = new Color(0.8f,0.8f,0.8f);
        tm.UpdateCustomizeHoverText("");
    }

    public void OptionClicked()
    {
        tm.SelectedCustomizeOption(this.gameObject.name);
    }
}
