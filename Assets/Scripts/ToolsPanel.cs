using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToolsPanel : MonoBehaviour
{
    public EditorManager editorManager;

    public Transform toolsPanel;
    public GameObject selectionPrefab;
    public TextMeshProUGUI hoverText;
    
    public void AddToolToPanel(string name)
    {
        Sprite img = Resources.Load<Sprite>($"ToolIcons/{name}");
        GameObject go = Instantiate(selectionPrefab,Vector3.zero,Quaternion.identity,toolsPanel);
        go.GetComponent<Button>().onClick.AddListener(delegate {editorManager.ToolClicked(name);});
        go.GetComponent<Image>().sprite = img;
        go.name = name;
        UpdateUseTexts();
    }

    public void DisplayTools(string[] tools)
    {
        Reset();
        foreach (string tool in tools) {
            AddToolToPanel(tool);
        }
    }

    public void Reset()
    {
        foreach (Transform tool in toolsPanel) {
            Destroy(tool.gameObject);
        }
    }

    public void UpdateHoverText(string txt)
    {
        hoverText.text = txt;
    }

    public void UpdateUseTexts()
    {
        foreach (Transform tool in toolsPanel) {
            if (tool.name.Equals("player")) {
                int count = editorManager.hasPlayer ? 1 : 0;
                tool.GetComponent<SelectionItem>().SetUses(1-count,1);
            }
            else if (tool.name.Equals("start")) {
                int count = editorManager.hasStart ? 1 : 0;
                tool.GetComponent<SelectionItem>().SetUses(1-count,1);
            }
            else if (tool.name.Equals("end")) {
                int count = editorManager.hasEnd ? 1 : 0;
                tool.GetComponent<SelectionItem>().SetUses(1-count,1);
            }
        }
    }
}
