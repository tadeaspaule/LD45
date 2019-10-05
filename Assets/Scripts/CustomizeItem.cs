using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeItem : MonoBehaviour
{    
    Image image;
    CustomizePanel customizePanel;
    EditorManager editor;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        customizePanel = FindObjectOfType<CustomizePanel>();
        editor = FindObjectOfType<EditorManager>();
        MouseLeft();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseEntered()
    {
        transform.localScale = new Vector3(1.5f,1.5f,1f);
        image.color = Color.white;
        customizePanel.UpdateCustomizeHoverText(this.name);
    }

    public void MouseLeft()
    {
        transform.localScale = Vector3.one;
        image.color = new Color(0.8f,0.8f,0.8f);
        customizePanel.UpdateCustomizeHoverText("");
    }

    public void OptionClicked()
    {
        editor.SelectedCustomizeOption(this.gameObject.name);
    }
}
