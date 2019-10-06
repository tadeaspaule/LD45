using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public Color backgroundColor;
    public Color accentColor;

    public Image startScreen;
    public Image endScreen;
    public Image transitionPanel;

    public Image timerFilled;

    void Start()
    {
        startScreen.color = backgroundColor;
        endScreen.color = backgroundColor;
        transitionPanel.color = backgroundColor;
        Camera.main.backgroundColor = backgroundColor;

        timerFilled.color = accentColor;
    }
}
