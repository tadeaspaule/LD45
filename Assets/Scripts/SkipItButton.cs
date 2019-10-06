using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkipItButton : MonoBehaviour
{
    TimeManager tm;
    TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TimeManager>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"The Jam ends in <color=red>{tm.GetSecondsLeft()}<color=white> seconds! Skip it!";
    }
}
