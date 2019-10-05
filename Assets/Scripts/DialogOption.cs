using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogOption : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image dialogFace;
    public DialogManager dialogManager;

    void Start()
    {
        MouseLeft();
    }
    
    public void MouseEntered()
    {
        transform.localScale = new Vector3(1.1f,1.1f,1.1f);
        text.color = Color.white;
        if (this.name.Equals("DoItButton")) {
            dialogFace.sprite = dialogManager.GetDoItSprite();
        }
        else {
            dialogFace.sprite = dialogManager.GetSkipItSprite();
        }
    }

    public void MouseLeft()
    {
        transform.localScale = Vector3.one;
        text.color = new Color(0.8f,0.8f,0.8f);
        dialogFace.color = Color.white;
    }
}
