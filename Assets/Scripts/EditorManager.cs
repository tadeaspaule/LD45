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
    public TimeManager timeManager;

    #endregion
    
    #region GameObjects
    
    public ToolsPanel toolsPanel;
    public CustomizePanel customizePanel;

    public bool hasPlayer = false;
    public bool hasEnd = false;

    GameObject itemToPlace = null; // item you're moving
    GameObject selectedItem = null; // item you're editing

    #endregion
    
    #region Transition

    public Animation transitionAnim;
    public AnimationClip animClip;
    public TextMeshProUGUI transitionText;

    void PlayTransition(int stageIndex)
    {
        switch (stageList[stageIndex].dialogMode) {
            case "art":
                transitionText.text = "Furious drawing";
                break;
            case "programming":
                transitionText.text = "Furious coding";
                break;
            default:
                break;
        }
        transitionAnim.Play();
        StartCoroutine(TimeSpeedupDuringTransition());
    }

    IEnumerator TimeSpeedupDuringTransition()
    {
        timeManager.SetMultiplier(30f);
        yield return new WaitForSeconds(animClip.length);
        timeManager.SetMultiplier(1f);
    }
    
    #endregion
    
    #region Checklist

    public List<int> skippedStages = new List<int>();
    public Transform checklistContainer;
    public GameObject checklistPrefab;

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
        public string[] gainTools;
    }

    List<Stage> stageList = new List<Stage>();
    public TextAsset stagesJson;
    int currentStage;

    List<string> availableTools = new List<string>();

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
        string name = stageList[currentStage].name;
        if (name.Equals("first level")) {
            SwitchScene(false);
            ShowButtonTooltip(playLevelBtn, "To playtest your level, click this button");
            if (!nextStageBtn.activeSelf) {
                // if you skipped the menu sequence
                ShowButtonTooltip(nextStageBtn, "When you're ready, move on by clicking this button");
            }
        }
        else if (name.Equals("more levels")) {
            SwitchScene(false);
            ShowButtonTooltip(changeLevelBtn, "To make more levels, click this button");
            
            // reached the last stage, now allow making new levels and disable next stage btn
            nextStageBtn.SetActive(false);
            ShowTooltip("Make more levels");

            // enable checklist
            foreach (int i in skippedStages) {
                Debug.Log($"Should add to checklist n. {i}");
                GameObject go = Instantiate(checklistPrefab,Vector3.zero,Quaternion.identity,checklistContainer);
                go.GetComponentInChildren<TextMeshProUGUI>().text = stageList[i].name;
                go.GetComponent<Button>().onClick.AddListener(delegate {ClickedChecklistItem(go);});
                go.name = i.ToString();
            }
        }
        // sets up tools panel and maybe other things
        foreach (string tool in stageList[currentStage].gainTools) {
            availableTools.Add(tool);
        }
        toolsPanel.DisplayTools(availableTools.ToArray(), isInMenu);
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
    bool isInMenu = true;
    int currentLevel = 0;
    public Transform currentScene;

    public void SwitchScene(bool switchToMenu)
    {
        Debug.Log("Switching scene");
        menuScene.gameObject.SetActive(switchToMenu);
        gameScene.gameObject.SetActive(!switchToMenu);
        if (switchToMenu) currentScene = menuScene;
        else SwitchToLevel(currentLevel);
        isInMenu = switchToMenu;
        customizePanel.CloseCustomizePanel();
        SetSelectedItem(null);
        itemToPlace = null;
    }

    public void SwitchToLevel(int level)
    {
        currentLevel = level;
        currentScene = gameScene.GetChild(level);
    }

    #endregion
    
    #region Buttons in bottom right
    
    public GameObject nextStageBtn;
    public GameObject playLevelBtn;
    public GameObject changeLevelBtn;

    #endregion
    
    #region Unity Methods
    
    // Start is called before the first frame update
    void Start()
    {
        currentScene = menuScene;
        stageList = JsonReader.readJsonArray<Stage>(stagesJson.ToString());
        currentStage = 0;
        StartStage();
        // uncomment below to test game
        // SwitchScene(false);
        // toolsPanel.AddToolToPanel("player");
        // toolsPanel.AddToolToPanel("platform");
        // toolsPanel.AddToolToPanel("enemyshooting");
        // toolsPanel.AddToolToPanel("enemywalking");
    }

    // Update is called once per frame
    void Update()
    {
        if (itemToPlace != null) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            itemToPlace.transform.position = new Vector3(pos.x,pos.y,0f);

            if (Input.GetMouseButtonDown(0)) {
                // placed down item
                if (!nextStageBtn.activeSelf) {
                    // after first item is placed, enable next stage button
                    ShowButtonTooltip(nextStageBtn, "When you're ready, move on by clicking this button");
                }
                selectedItem = itemToPlace;
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
        }
        selectedItem = item;
        if (item != null) selectedItem.GetComponent<SelectedDisplay>().ToggleActive(true);
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
        if (itemToPlace != null) return;
        if (name.Equals("player")) {
            if (hasPlayer) return;
            hasPlayer = true;
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
        switch (name) {
            case "move":
                itemToPlace = selectedItem;
                break;
            case "delet":
                if (selectedItem.name.Equals("player")) hasPlayer = false;
                if (selectedItem.name.Equals("end")) hasEnd = false;
                Destroy(selectedItem);
                toolsPanel.UpdateUseTexts();
                SetSelectedItem(null);
                itemToPlace = null;
                customizePanel.CloseCustomizePanel();
                break;
            case "expand":
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
        nextStageBtn.SetActive(true);
        Debug.Log("Clicked placed item");
        if (itemToPlace != null) return; // nothing happens if currently dragging something
        Debug.Log("Clicked placed item part 2");
        SetSelectedItem(item);
        customizePanel.OpenCustomizeOptions(item.name);
    }

    void ShowButtonTooltip(GameObject btn, string text)
    {
        btn.SetActive(true);
        ShowTooltip(text);
    }

    void ShowTooltip(string text)
    {
        Debug.Log(text);
    }

    public void ClickedDoIt()
    {
        dialogManager.CloseDialog();
        PlayTransition(currentStage);
        SetupStage();
    } 

    public void ClickedSkipIt()
    {
        skippedStages.Add(currentStage);
        SkipStage();
    }

    public void ClickedChecklistItem(GameObject go)
    {
        PlayTransition(int.Parse(go.name));
        Destroy(go);
    }

    public void ClickedNextStage()
    {
        currentStage += 1;
        StartStage();
    }

    #endregion
}
