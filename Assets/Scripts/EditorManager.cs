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
    public ColorManager colorManager;

    #endregion
    
    #region GameObjects
    
    public ToolsPanel toolsPanel;
    public CustomizePanel customizePanel;

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
        StartCoroutine(UpdateToolsAfterAnim(stageIndex));
    }

    IEnumerator TimeSpeedupDuringTransition()
    {
        timeManager.SetMultiplier(30f);
        yield return new WaitForSeconds(animClip.length);
        timeManager.SetMultiplier(1f);
    }

    IEnumerator UpdateToolsAfterAnim(int index)
    {
        yield return new WaitForSeconds(animClip.length);
        foreach (string tool in stageList[index].gainTools) {
            AddNewTool(tool);
        }
        foreach (string tool in stageList[index].loseTools) {
            availableTools.Remove(tool);
        }
        toolsPanel.DisplayTools(availableTools.ToArray(), isInMenu);
    }
    
    #endregion
    
    #region Checklist

    public List<int> skippedStages = new List<int>();
    public Transform checklistContainer;
    public GameObject checklistPrefab;
    public GameObject checklistHeader;

    #endregion
    
    #region Levels

    public GameObject levelWindow;
    public Transform levelsContainer;
    public Transform addLevelButton;
    public GameObject levelSelectPrefab;
    
    public void SwitchToLevel(int level)
    {
        currentLevel = level;
        currentScene = gameScene.GetChild(level);
        for (int i = 0; i < gameScene.childCount; i++) {
            gameScene.GetChild(i).gameObject.SetActive(false);
        }
        gameScene.GetChild(level).gameObject.SetActive(true);
        CloseLevelSelect();
        // update tools        
        toolsPanel.DisplayTools(availableTools.ToArray(), isInMenu);
    }

    public void OpenLevelSelect()
    {
        for (int i = 0; i < levelsContainer.childCount; i++) {
            Image img = levelsContainer.GetChild(i).GetComponent<Image>();
            if (i == currentLevel) {
                img.color = colorManager.accentColor;
            }
            else {
                img.color = new Color(0f,0f,0f,0.1f);
            }
        }
        levelWindow.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        levelWindow.SetActive(false);
    }

    public void CreateLevel()
    {
        int i = levelsContainer.childCount;
        GameObject go = Instantiate(levelSelectPrefab,Vector3.zero,Quaternion.identity,levelsContainer);
        go.GetComponent<Button>().onClick.AddListener(delegate {SwitchToLevel(i-1);});
        go.name = i.ToString();
        go.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
        addLevelButton.SetAsLastSibling();
        GameObject level = new GameObject($"level{i}");
        level.transform.parent = gameScene;
        level.transform.position = Vector3.zero;
        SwitchToLevel(i-1);
    }

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
        public string[] loseTools;
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
            checklistContainer.gameObject.SetActive(true);
            checklistHeader.SetActive(true);
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
            AddNewTool(tool);
        }
        foreach (string tool in stageList[currentStage].loseTools) {
            Debug.Log($"Removing {tool}");
            availableTools.Remove(tool);
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

    #region Announcer

    public Transform announceContainer;
    public GameObject announcePrefab;

    IEnumerator DelayedDestroy(GameObject go)
    {
        yield return new WaitForSeconds(2f);
        Destroy(go);
    }

    bool updatedPlatforms = false;
    
    void AddNewTool(string tool)
    {
        availableTools.Add(tool);
        string name = "";
        foreach (ToolsPanel.NameIDPair nidp in toolsPanel.names) {
            if (nidp.internalName.Equals(tool)) name = nidp.showName;
        }
        GameObject go = Instantiate(announcePrefab,Vector3.zero,Quaternion.identity,announceContainer);
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = name;
        StartCoroutine(DelayedDestroy(go));

        if (tool.Contains("pretty")) {
            selectedItem = null;
            itemToPlace = null;
            foreach (Transform child in menuScene) {
                if ((child.name + "pretty").Equals(tool)) {
                    GameObject prefab = GetItemPrefab(tool);
                    GameObject newV = Instantiate(prefab,Vector3.zero,Quaternion.identity,currentScene);
                    newV.name = tool;
                    newV.transform.position = child.position;
                    Destroy(child.gameObject);
                }
            }
            foreach (Transform level in gameScene) {
                foreach (Transform child in level) {
                    if ((child.name + "pretty").Equals(tool)) {
                        GameObject prefab = GetItemPrefab(tool);
                        GameObject newV = Instantiate(prefab,Vector3.zero,Quaternion.identity,currentScene);
                        newV.name = tool;
                        newV.transform.position = child.position;
                        Destroy(child.gameObject);
                    }
                }
            }
        }
        else if (tool.Contains("-skeleton")) {
            selectedItem = null;
            itemToPlace = null;
            foreach (Transform level in gameScene) {
                foreach (Transform child in level) {
                    if ((child.name + "-skeleton").Equals(tool)) {
                        GameObject prefab = GetItemPrefab(tool);
                        GameObject newV = Instantiate(prefab,Vector3.zero,Quaternion.identity,level);
                        newV.name = tool;
                        newV.transform.position = child.position;
                        Destroy(child.gameObject);
                    }
                }
            }
        }
        else if (tool.Contains("platform") && tool.Length > 8 && !updatedPlatforms) {
            // upgraded platform
            selectedItem = null;
            itemToPlace = null;
            updatedPlatforms = true;
            foreach (Transform level in gameScene) {
                foreach (Transform child in level) {
                    if (child.name.Equals("platform")) {
                        GameObject prefab = GetItemPrefab(tool);
                        GameObject newV = Instantiate(prefab,Vector3.zero,Quaternion.identity,level);
                        newV.name = tool;
                        newV.transform.position = child.position;
                        Destroy(child.gameObject);
                    }
                }
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
        StartEverything(true);        
    }

    void StartEverything(bool isDebug)
    {
        if (isDebug) {
            playLevelBtn.gameObject.SetActive(true);
            // here add stuff to test
            SwitchScene(false);
            availableTools.Add("playerpretty");
            availableTools.Add("platformhalloween1");
            availableTools.Add("enemywalking");
            availableTools.Add("enemywalking-skeleton");
            toolsPanel.DisplayTools(availableTools.ToArray(), isInMenu);
        }
        else {
            StartStage();
        }
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
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            SetSelectedItem(null);
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
        else customizePanel.CloseCustomizePanel();
    }

    GameObject GetItemPrefab(string name)
    {
        Debug.Log($"Getting item with name {name}");
        return Resources.Load<GameObject>($"Tools/{name}");
    }

    #endregion
    
    #region Click Events

    public void ToolClicked(string name)
    {
        if (itemToPlace != null) return;
        if (name.StartsWith("player") && HasPlayer()) {
            foreach (Transform child in currentScene) {
                if (child.name.StartsWith("player")) SetSelectedItem(child.gameObject);
                customizePanel.OpenCustomizeOptions(child.gameObject.name);
            }
            return;
        }
        if (name.StartsWith("end") && HasEnd()) {
            foreach (Transform child in currentScene) {
                if (child.name.StartsWith("player")) SetSelectedItem(child.gameObject);
                customizePanel.OpenCustomizeOptions(child.gameObject.name);
            }
            return;
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

    public bool HasPlayer()
    {
        foreach (Transform child in currentScene) {
            if (child.gameObject.name.StartsWith("player")) return true;
        }
        return false;
    }

    public bool HasEnd()
    {
        foreach (Transform child in currentScene) {
            if (child.gameObject.name.StartsWith("end")) return true;
        }
        return false;
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
            case "delete":
                Destroy(selectedItem);
                toolsPanel.UpdateUseTexts();
                SetSelectedItem(null);
                itemToPlace = null;
                customizePanel.CloseCustomizePanel();
                break;
            case "expand":
                selectedItem.GetComponent<Resizer>().Expand();
                break;
            case "shrink":
                selectedItem.GetComponent<Resizer>().Shrink();
                break;
            case "flip":
                EnemyBase eb = selectedItem.GetComponent<EnemyBase>();
                if (eb == null) eb = selectedItem.GetComponentInChildren<EnemyBase>();
                eb.Flip();
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
    } 

    public void ClickedSkipIt()
    {
        skippedStages.Add(currentStage);
        SkipStage();
    }

    public void ClickedChecklistItem(GameObject go)
    {
        int index = int.Parse(go.name);
        foreach (string tool in stageList[index].loseTools) {
            availableTools.Remove(tool);
        }
        PlayTransition(index);
        Destroy(go);
    }

    public void ClickedNextStage()
    {
        currentStage += 1;
        StartStage();
    }

    #endregion
}
