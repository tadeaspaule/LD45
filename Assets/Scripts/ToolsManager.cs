using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolsManager : MonoBehaviour
{
    GameObject selectedTool = null;
    public TextMeshProUGUI testText;

    public Transform toolsPanel;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedTool != null) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedTool.transform.position = new Vector3(pos.x,pos.y,0f);

            if (Input.GetMouseButtonDown(0)) {
                selectedTool = null;
            }
        }
    }


    public void ToolClicked(string name)
    {
        Debug.Log(name);
        GameObject prefab = Resources.Load<GameObject>($"Tools/{name}");
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedTool = Instantiate(prefab,new Vector3(pos.x,pos.y,0f),Quaternion.identity);
    }
}
