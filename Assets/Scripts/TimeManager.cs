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
    public Image clockImage;

    // start sequence related
    public AnimationClip startAnim;
    public TextMeshProUGUI startSecondsTxt;

    // end sequence related
    public AnimationClip endAnim;
    public Animation startEndAnimation;

    const int timeLimit = 15 * 60;

    public void SetMultiplier(float m)
    {
        Debug.Log("Multiplier changes");
        multiplier = m;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        startSecondsTxt.text = timeLimit.ToString();
        StartCoroutine(StartCountdown());
        StartCoroutine(Setup());
    }

    IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(startAnim.length - 2.5f);
        measuringTime = true;
        startTime = Time.time;
        currentTime = Time.time;
        endTime = Time.time + timeLimit;
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
        currentTime += Time.deltaTime*multiplier;
        secondsText.text = GetSecondsLeft().ToString();
        startSecondsTxt.text = GetSecondsLeft().ToString();
        clockImage.fillAmount = 1 - (currentTime-startTime) / (endTime-startTime);
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
