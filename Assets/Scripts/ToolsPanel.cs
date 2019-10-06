using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToolsPanel : MonoBehaviour
{
    #region Other Managers
    
    public EditorManager editorManager;

    #endregion

    #region GameObjects

    public Transform toolsPanel;
    public GameObject selectionPrefab;
    public TextMeshProUGUI hoverText;

    #endregion

    #region  Hover text

    [System.Serializable]
    public class NameIDPair
    {
        public string showName;
        public string internalName;
    }
    public NameIDPair[] names;

    public void UpdateHoverText(string txt)
    {
        if (txt.Length == 0) {
            hoverText.text = "";
            return;
        }
        foreach (NameIDPair nameIDPair in names) {
            if (nameIDPair.internalName.Equals(txt)) {
                hoverText.text = nameIDPair.showName;
                break;
            }
        }
    }

    #endregion
    
    public void AddToolToPanel(string name)
    {
        Sprite img = Resources.Load<Sprite>($"ToolIcons/{name}");
        GameObject go = Instantiate(selectionPrefab,Vector3.zero,Quaternion.identity,toolsPanel);
        go.GetComponent<Button>().onClick.AddListener(delegate {editorManager.ToolClicked(name);});
        go.GetComponent<Image>().sprite = img;
        go.name = name;
        UpdateUseTexts();
    }

    List<string> menuOnlyTools = new List<string>(new string[]{"button","buttonpretty"});
    
    public void DisplayTools(string[] tools, bool isInMenu)
    {
        Reset();
        foreach (string tool in tools) {
            if (menuOnlyTools.Contains(tool) == isInMenu) AddToolToPanel(tool);
        }
    }

    public void Reset()
    {
        foreach (Transform tool in toolsPanel) {
            Destroy(tool.gameObject);
        }
    }

    public void UpdateUseTexts()
    {
        foreach (Transform tool in toolsPanel) {
            if (tool.name.StartsWith("player")) {
                int count = editorManager.hasPlayer ? 1 : 0;
                tool.GetComponent<SelectionItem>().SetUses(1-count,1);
            }
            else if (tool.name.StartsWith("end")) {
                int count = editorManager.hasEnd ? 1 : 0;
                tool.GetComponent<SelectionItem>().SetUses(1-count,1);
            }
        }
    }
}
