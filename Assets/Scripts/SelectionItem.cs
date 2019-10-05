using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionItem : MonoBehaviour
{    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseEntered()
    {
        transform.localScale = new Vector3(1.1f,1.1f,1.1f);
    }

    public void MouseLeft()
    {
        transform.localScale = Vector3.one;
    }
}
