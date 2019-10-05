using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorManager : MonoBehaviour
{
    #region Other Managers

    public DialogManager dialogManager;
    public GameManager gameManager;

    #endregion
    
    #region GameObjects
    
    public ToolsPanel toolsPanel;
    public CustomizePanel customizePanel;

    public bool hasPlayer = false;
    public bool hasStart = false;
    public bool hasEnd = false;

    GameObject itemToPlace = null; // item you're moving
    GameObject selectedItem = null; // item you're editing

    #endregion
    
    #region Stages

    private class Stage
    {
        public string name;
        public string dialogMode;
        public bool skippable;
        public string dialogHeader;
        public string doItText;
        public string afterSkip;
        public string[] availableTools;
    }

    List<Stage> stageList = new List<Stage>();
    public TextAsset stagesJson;
    int currentStage;

    void StartStage()
    {
        // if relevant, sets up inner dialog
        Stage stage = stageList[currentStage];
        if (stage.skippable) {
            dialogManager.OpenDialog(stage.dialogHeader,stage.doItText,stage.dialogMode);
        }
        else {
            dialogManager.CloseDialog();
            SetupStage(); // non-skippable stage, just setup stuff
        }
    }

    void SetupStage()
    {
        // sets up tools panel and maybe other things
        toolsPanel.DisplayTools(stageList[currentStage].availableTools);
    }

    void SkipStage()
    {
        for (int i = 0; i < stageList.Count; i++) {
            if (stageList[i].name.Equals(stageList[currentStage].afterSkip)) {
                currentStage = i;
            }
        }
        StartStage();
    }

    #endregion
    
    #region Pretty Version related

    HashSet<string> prettyVersions = new HashSet<string>();

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

    #endregion

    #region Scenes

    public Transform sceneHolder;
    public Transform menuScene;
    public Transform gameScene;
    bool isInMenu = false;
    int currentLevel = 0;
    public Transform currentScene;

    public void SwitchScene(bool switchToMenu)
    {
        menuScene.gameObject.SetActive(switchToMenu);
        gameScene.gameObject.SetActive(!switchToMenu);
        if (switchToMenu) currentScene = menuScene;
        else SwitchToLevel(currentLevel);
        customizePanel.CloseCustomizePanel();
        selectedItem = null;
        itemToPlace = null;
    }

    public void SwitchToLevel(int level)
    {
        currentLevel = level;
        currentScene = gameScene.GetChild(level);
    }

    #endregion
    
    #region Unity Methods
    
    // Start is called before the first frame update
    void Start()
    {
        currentScene = gameScene.GetChild(0);
        // SwitchScene(false);
        stageList = JsonReader.readJsonArray<Stage>(stagesJson.ToString());
        currentStage = 0;
        StartStage();
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

    #endregion

    #region Helper Methods

    public void DisableAllEditorUI()
    {
        if (selectedItem != null) selectedItem.GetComponent<SelectedDisplay>().ToggleActive(false);
        customizePanel.CloseCustomizePanel();
    }
    
    void SetSelectedItem(GameObject item)
    {
        if (selectedItem != null) {
            selectedItem.GetComponent<SelectedDisplay>().ToggleActive(false);
            ToggleItemResizers(false);
        }
        selectedItem = item;
        if (item != null) selectedItem.GetComponent<SelectedDisplay>().ToggleActive(true);
    }

    void ToggleItemResizers(bool active)
    {
        // turn off resizers
        foreach (Transform child in selectedItem.transform) {
            if (child.name.Equals("Resizers")) {
                child.gameObject.SetActive(active);
            }
        }
    }

    GameObject GetItemPrefab(string name)
    {
        if (prettyVersions.Contains(name)) name += "pretty";
        Debug.Log($"Getting item with name {name}");
        return Resources.Load<GameObject>($"Tools/{name}");
    }

    #endregion
    
    #region Click Events

    public void ToolClicked(string name)
    {
        if (name.Equals("player")) {
            if (hasPlayer) return;
            hasPlayer = true;
        }
        if (name.Equals("start")) {
            if (hasStart) return;
            hasStart = true;
        }
        if (name.Equals("end")) {
            if (hasEnd) return;
            hasEnd = true;
        }
        Debug.Log($"Clicked tool {name}");
        GameObject prefab = GetItemPrefab(name);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        itemToPlace = Instantiate(prefab,new Vector3(pos.x,pos.y,0f),Quaternion.identity,currentScene);
        itemToPlace.name = name;
        if (selectedItem != null) customizePanel.CloseCustomizePanel();
        SetSelectedItem(itemToPlace);
        toolsPanel.UpdateUseTexts();
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

    public void SelectedCustomizeOption(string name)
    {
        ToggleItemResizers(false);
        switch (name) {
            case "move":
                itemToPlace = selectedItem;
                break;
            case "remove":
                if (selectedItem.name.Equals("player")) hasPlayer = false;
                if (selectedItem.name.Equals("start")) hasStart = false;
                if (selectedItem.name.Equals("end")) hasEnd = false;
                Destroy(selectedItem);
                toolsPanel.UpdateUseTexts();
                SetSelectedItem(null);
                itemToPlace = null;
                customizePanel.CloseCustomizePanel();
                break;
            case "widen":
                selectedItem.transform.GetChild(1).localScale += new Vector3(0.3f,0f,0f);
                break;
            case "shrink":
                selectedItem.transform.GetChild(1).localScale -= new Vector3(0.3f,0f,0f);
                break;
            default:
                break;
        }
    }

    public void PlacedItemClicked(GameObject item)
    {
        if (itemToPlace != null) return; // nothing happens if currently dragging something
        SetSelectedItem(item);
        customizePanel.OpenCustomizeOptions(item.name);
    }

    public void ClickedDoIt()
    {
        dialogManager.CloseDialog();
        SetupStage();
    }

    public void ClickedSkipIt()
    {
        SkipStage();
    }

    public void ClickedNextStage()
    {
        currentStage += 1;
        StartStage();
    }

    #endregion
}
