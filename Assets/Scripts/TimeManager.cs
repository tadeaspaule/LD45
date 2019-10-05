using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float currentTime;
    float endTime;
    
    // Start is called before the first frame update
    void Start()
    {
        currentTime = Time.time;
        endTime = Time.time + 50 * 60;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
    }

    public int GetSecondsLeft()
    {
        return (int)Mathf.Round(endTime - currentTime);
    }
}
