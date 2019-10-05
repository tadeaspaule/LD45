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

    public GameObject customizePanel;
    public Animation a;

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
                Debug.Log("letting go");
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


    public void ToolClicked(string name)
    {
        Debug.Log(name);
        GameObject prefab = Resources.Load<GameObject>($"Tools/{name}");
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        itemToPlace = Instantiate(prefab,new Vector3(pos.x,pos.y,0f),Quaternion.identity);
    }

    public void UpdateHoverText(string txt)
    {
        hoverText.text = txt;
    }

    public void PlacedItemClicked(GameObject item)
    {
        if (itemToPlace != null) return; // nothing happens if currently dragging something
        selectedItem = item;
        a.Play("opencustomize");
    }

    public void ClickedBackground()
    {
        // this might unselect item or something like that depending on circumstances
        Debug.Log("Background clicked");
        if (selectedItem != null) {
            a.Play("closecustomize");
            selectedItem = null;
        }
    }
}
