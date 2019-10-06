using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CustomizePanel : MonoBehaviour
{
    public Transform customizeOptionsContainer;
    public TextMeshProUGUI customizeHeader;
    public Animation panelAnimation;
    
    public GameObject customizePrefab;

    bool customizeOpen = false;

    public void CloseCustomizePanel()
    {
        if (!customizeOpen) return;
        panelAnimation.Play("closecustomize");
        customizeOpen = false;
    }

    public void OpenCustomizePanel()
    {
        panelAnimation.Play("opencustomize");
        customizeOpen = true;
    }

    public void UpdateCustomizeHoverText(string txt)
    {
        customizeHeader.text = ToTitle(txt);
    }
    
    public void OpenCustomizeOptions(string itemName)
    {
        // delete any customize options that might be left over in the container
        foreach (Transform child in customizeOptionsContainer) {
            Destroy(child.gameObject);
        }
        
        List<string> options = new List<string>();
        options.Add("move");
        options.Add("delete");
        // add more options
        if (itemName.StartsWith("platform")) {
            options.Add("expand");
            options.Add("shrink");
        }
        foreach (string op in options) {
            Sprite img = Resources.Load<Sprite>($"CustomizeOptions/{op}");
            GameObject optionAdded = Instantiate(customizePrefab,Vector3.zero,Quaternion.identity,customizeOptionsContainer);
            optionAdded.GetComponent<Image>().sprite = img;
            optionAdded.name = op;
        }
        if (!customizeOpen) {
            panelAnimation.Play("opencustomize");
            customizeOpen = true;
        }
    }

    string ToTitle(string txt)
    {
        if (txt.Length == 0) return "";
        return txt[0].ToString().ToUpper() + txt.Substring(1);
    }
}
