using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public EditorManager editorManager;
    public GameManager gameManager;
    
    float startTime;
    public float currentTime;
    float endTime;
    public float multiplier = 1f;

    bool measuringTime = false;

    public TextMeshProUGUI secondsText;

    // start sequence related
    public AnimationClip startAnim;
    public TextMeshProUGUI startSecondsTxt;

    // end sequence related
    public AnimationClip endAnim;
    public Animation startEndAnimation;

    public Animation timeBlinkAnimation;

    const int timeLimit = 15 * 60;

    const int everyN = 5;
    Color normalCol = new Color(51f/255f,51f/255f,51f/255f,100f/255f);
    Color redColor = new Color(1f,51f/255f,51f/255f,1f);

    bool dontBlink = false;

    public void SetMultiplier(float m)
    {
        if (m > 1f) {
            // set to red during the transition time sink
            secondsText.color = redColor;
            dontBlink = true;
        }
        else {
            dontBlink = false;
            secondsText.color = normalCol;
        }
        multiplier = m;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (startEndAnimation.gameObject.activeSelf) {
            // playing normally, so play all the usual animations
            startSecondsTxt.text = timeLimit.ToString();
            StartCoroutine(StartCountdown());
            StartCoroutine(Setup());
        }
        else {
            // in debug mode, start countdown immediately
            SetupTimes();
        }
        
    }

    void SetupTimes()
    {
        measuringTime = true;
        startTime = Time.time;
        currentTime = Time.time;
        endTime = Time.time + timeLimit;
    }

    IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(startAnim.length - 2.5f);
        SetupTimes();
    }

    IEnumerator Setup()
    {
        yield return new WaitForSeconds(startAnim.length + 0.5f);
        editorManager.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!measuringTime) return;
        float oldModulo = currentTime % everyN;
        currentTime += Time.deltaTime*multiplier;
        float newModulo = currentTime % everyN;
        if (newModulo < oldModulo && !dontBlink) {
            Debug.Log($"Old modulo is {oldModulo}, new one is {newModulo}");
            timeBlinkAnimation.Play("timeblink");
        }
        secondsText.text = GetSecondsLeft().ToString();
        startSecondsTxt.text = GetSecondsLeft().ToString();
    }

    public int GetSecondsLeft()
    {
        return (int)Mathf.Round(endTime - currentTime);
    }

    public void PlayEnd()
    {
        startEndAnimation.gameObject.SetActive(true);
        startEndAnimation.transform.GetChild(0).gameObject.SetActive(false); // turn off start screen just in case
        editorManager.gameObject.SetActive(false);
        gameManager.gameObject.SetActive(false);
        startEndAnimation.Play("outroanimation");
        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame()
    {
        // TODO MAYBE if there's time just exit to menu
        yield return new WaitForSeconds(endAnim.length + 0.5f);
        Application.Quit();
    }
}
