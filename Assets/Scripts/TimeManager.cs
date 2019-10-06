using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    float startTime;
    public float currentTime;
    float endTime;
    public float multiplier = 1f;

    public TextMeshProUGUI secondsText;
    public Image clockImage;

    public void SetMultiplier(float m)
    {
        Debug.Log("Multiplier changes");
        multiplier = m;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        currentTime = Time.time;
        endTime = Time.time + 15 * 60;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime*multiplier;
        secondsText.text = GetSecondsLeft().ToString();
        clockImage.fillAmount = 1 - (currentTime-startTime) / (endTime-startTime);
    }

    public int GetSecondsLeft()
    {
        return (int)Mathf.Round(endTime - currentTime);
    }
}
