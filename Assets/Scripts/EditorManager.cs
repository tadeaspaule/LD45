using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorManager : MonoBehaviour
{
    #region Other Managers

    public DialogManager dialogManager;

    #endregion
    
    public ToolsPanel toolsPanel;

    public CustomizePanel customizePanel;

    #region Selected Tool

    GameObject itemToPlace = null; // item you're moving
    GameObject selectedItem = null; // item you're editing

    #endregion

    #region Editing variables

    #endregion

    #region Appearance

    HashSet<string> prettyVersions = new HashSet<string>();

    #endregion

    #region Scenes

    public Transform sceneHolder;
    public Transform menuScene;
    public Transform gameScene;
    Transform currentScene;

    public void SwitchScene(string sceneName)
    {
        foreach (Transform scene in sceneHolder) {
            if (scene.name.Equals(sceneName)) scene.gameObject.SetActive(true);
            else if (!scene.name.Equals("BackgroundClickCatcher")) scene.gameObject.SetActive(false);
        }
        customizePanel.CloseCustomizePanel();
        selectedItem = null;
        itemToPlace = null;
    }

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        currentScene = menuScene;
        dialogManager.OpenDialog("I need to make a main menu first...", "OK! Let's get it done!","UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (itemToPlace != null) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            itemToPlace.transform.position = new Vector3(pos.x,pos.y,0f);

            if (Input.GetMouseButtonDown(0)) {
                itemToPlace = null;
                customizePanel.OpenCustomizeOptions(selectedItem.name);
            }
        }
    }

    void SetSelectedItem(GameObject item)
    {
        if (selectedItem != null) selectedItem.GetComponent<SelectedDisplay>().ToggleActive(false);
        selectedItem = item;
        if (item != null) selectedItem.GetComponent<SelectedDisplay>().ToggleActive(true);
    }


    public void ToolClicked(string name)
    {
        Debug.Log(name);
        GameObject prefab = GetItemPrefab(name);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        itemToPlace = Instantiate(prefab,new Vector3(pos.x,pos.y,0f),Quaternion.identity,currentScene);
        itemToPlace.name = name;
        if (selectedItem != null) customizePanel.CloseCustomizePanel();
        SetSelectedItem(itemToPlace);
    }

    GameObject GetItemPrefab(string name)
    {
        if (prettyVersions.Contains(name)) name += "pretty";
        return Resources.Load<GameObject>($"Tools/{name}");
    }

    public void UnlockPrettyVersion(string itemName)
    {
        prettyVersions.Add(itemName);
        // replace all existing
        List<Vector3> posToInstantiate = new List<Vector3>();
        int newSelected = -1; // in case you replace the selected object, re-select it
        for (int i = 0; i < currentScene.childCount; i++) {
            Transform t = currentScene.GetChild(i);
            if (t.name.Equals(itemName)) {
                Vector3 pos = t.position;
                Destroy(t.gameObject);
                posToInstantiate.Add(pos);
            }
            if (t.gameObject.Equals(selectedItem)) {
                newSelected = i;
            }
        }
        for (int i = 0; i < posToInstantiate.Count; i++) {
            GameObject go = Instantiate(GetItemPrefab(itemName),posToInstantiate[i],Quaternion.identity,currentScene);
            if (i == newSelected) {
                selectedItem = go;
                go.GetComponent<SelectedDisplay>().ToggleActive(true);
            }
        }
    }

    public void PlacedItemClicked(GameObject item)
    {
        if (itemToPlace != null) return; // nothing happens if currently dragging something
        SetSelectedItem(item);
        customizePanel.OpenCustomizeOptions(item.name);
    }

    public void SelectedCustomizeOption(string name)
    {
        switch (name) {
            case "move":
                itemToPlace = selectedItem;
                break;
            case "remove":
                Destroy(selectedItem);
                SetSelectedItem(null);
                itemToPlace = null;
                customizePanel.CloseCustomizePanel();
                break;
            default:
                break;
        }
    }

    public void ClickedBackground()
    {
        // this might unselect item or something like that depending on circumstances
        Debug.Log("Background clicked");
        if (selectedItem != null) {
            customizePanel.CloseCustomizePanel();
            SetSelectedItem(null);
        }
    }

    #region Click Events

    public void ClickedDoIt()
    {
        dialogManager.CloseDialog();
        toolsPanel.AddToolToPanel("button");
    }

    public void ClickedSkipIt()
    {
        dialogManager.CloseDialog();
    }

    #endregion
}
