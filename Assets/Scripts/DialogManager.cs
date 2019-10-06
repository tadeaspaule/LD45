using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{    
    public GameObject dialogOuter;
    public TextMeshProUGUI headerTMPro;
    public TextMeshProUGUI doItTMPro;
    public DialogOption doItOption;
    public DialogOption skipItOption;

    #region Dialog Face images

    [System.Serializable]
    public class ModeSpritePair
    {
        public Sprite sprite;
        public string mode;
    }
    public ModeSpritePair[] pairs;

    Sprite doItSprite;
    
    public Sprite skipItSprite;
    public Sprite defaultSprite;

    #endregion
    
    string mode; // changes what face is displayed when Do It hover
    
    public void OpenDialog(string header, string doItText, string mode)
    {
        this.mode = mode;
        foreach (ModeSpritePair msp in pairs) {
            if (msp.mode.Equals(mode)) {
                doItSprite = msp.sprite;
                break;
            }
        }
        headerTMPro.text = header;
        doItTMPro.text = doItText;

        // update Image resources based on mode

        // reset
        doItOption.MouseLeft();
        skipItOption.MouseLeft();
        dialogOuter.SetActive(true);
    }

    public void CloseDialog()
    {
        dialogOuter.SetActive(false);
    }

    public Sprite GetDoItSprite()
    {
        return doItSprite;
    }

    public Sprite GetSkipItSprite()
    {
        return skipItSprite;
    }

    public Sprite GetDefaultSprite()
    {
        // this is showing when not hovering on either option
        return defaultSprite;
    }
}
