using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolsManager : MonoBehaviour
{
    
    public TextMeshProUGUI testText;

    #region Tools Panel

    public Transform toolsPanel;
    public GameObject selectionPrefab;
    public TextMeshProUGUI hoverText;

    #endregion

    #region Customize Panel

    public Transform customizeOptionsContainer;
    public TextMeshProUGUI customizeHeader;
    public Animation a;
    
    public GameObject customizePrefab;

    #endregion

    #region Selected Tool

    GameObject itemToPlace = null; // item you're moving
    GameObject selectedItem = null; // item you're editing

    #endregion

    #region Editing variables

    #endregion

    
    // Start is called before the first frame update
    void Start()
    {
        AddToolToPanel("button");
    }

    // Update is called once per frame
    void Update()
    {
        if (itemToPlace != null) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            itemToPlace.transform.position = new Vector3(pos.x,pos.y,0f);

            if (Input.GetMouseButtonDown(0)) {
                itemToPlace = null;
                OpenCustomizeOptions(selectedItem.name);
            }
        }
    }

    public void AddToolToPanel(string name)
    {
        Sprite img = Resources.Load<Sprite>($"ToolIcons/{name}");
        GameObject go = Instantiate(selectionPrefab,Vector3.zero,Quaternion.identity,toolsPanel);
        go.GetComponent<Button>().onClick.AddListener(delegate {ToolClicked(name);});
        go.GetComponent<Image>().sprite = img;
        go.name = name;
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
        GameObject prefab = Resources.Load<GameObject>($"Tools/{name}");
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        itemToPlace = Instantiate(prefab,new Vector3(pos.x,pos.y,0f),Quaternion.identity);
        if (selectedItem != null) a.Play("closecustomize");
        SetSelectedItem(itemToPlace);
    }

    public void UpdateHoverText(string txt)
    {
        hoverText.text = txt;
    }

    public void UpdateCustomizeHoverText(string txt)
    {
        customizeHeader.text = txt;
    }

    public void PlacedItemClicked(GameObject item)
    {
        if (itemToPlace != null) return; // nothing happens if currently dragging something
        SetSelectedItem(item);
        OpenCustomizeOptions(item.name);
    }

    void OpenCustomizeOptions(string itemName)
    {
        // delete any customize options that might be left over in the container
        foreach (Transform child in customizeOptionsContainer) {
            Destroy(child.gameObject);
        }
        
        List<string> options = new List<string>();
        options.Add("move");
        options.Add("remove");
        // add more options
        foreach (string op in options) {
            Sprite img = Resources.Load<Sprite>($"CustomizeOptions/{op}");
            GameObject optionAdded = Instantiate(customizePrefab,Vector3.zero,Quaternion.identity,customizeOptionsContainer);
            optionAdded.GetComponent<Image>().sprite = img;
            optionAdded.name = op;
        }
        a.Play("opencustomize");
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
                a.Play("closecustomize");
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
            a.Play("closecustomize");
            SetSelectedItem(null);
        }
    }
}
