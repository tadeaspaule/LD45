using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDisplay : MonoBehaviour
{
    public GameObject selectionDisplay;
    
    public void ToggleActive(bool active)
    {
        selectionDisplay.SetActive(active);
    }
}
