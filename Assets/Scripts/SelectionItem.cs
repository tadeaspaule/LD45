using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionItem : MonoBehaviour
{    
    Image image;
    ToolsPanel toolsPanel;
    public TextMeshProUGUI usesText;
    public GameObject infinityImg;
    
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        toolsPanel = FindObjectOfType<ToolsPanel>();
        MouseLeft();
    }

    public void SetUses(int current, int max)
    {
        infinityImg.SetActive(false);
        usesText.gameObject.SetActive(true);
        usesText.text = $"{current}/{max}";
    }

    public void MouseEntered()
    {
        transform.localScale = new Vector3(1.5f,1.5f,1f);
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
