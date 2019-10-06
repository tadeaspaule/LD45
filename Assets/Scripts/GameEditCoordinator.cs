using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEditCoordinator : MonoBehaviour
{
    public GameManager gameManager;
    public EditorManager editorManager;
    public TimeManager timeManager;
    public Animation toolPanelAnims;
    public TextMeshProUGUI btnText;
    bool isEditing = true;

    bool preventAllClicks = false;

    public void ClickedSwitchButton()
    {
        if (preventAllClicks) return;
        if (isEditing) {
            if (!editorManager.HasPlayer()) return;
            // switching to game mode
            editorManager.DisableAllEditorUI();
            editorManager.gameObject.SetActive(false);
            gameManager.gameObject.SetActive(true);
            gameManager.Setup(editorManager.currentScene);
            toolPanelAnims.Play("closetools");
            btnText.text = "Back to editing";
        }
        else {
            // switching to edit mode
            gameManager.SwitchToEdit();
            editorManager.gameObject.SetActive(true);
            gameManager.gameObject.SetActive(false);
            toolPanelAnims.Play("opentools");
            btnText.text = "Play the level";
        }
        isEditing = !isEditing;
    }

    public void ClickedNextStage()
    {
        if (preventAllClicks) return;
        if (isEditing) editorManager.ClickedNextStage();
    }

    public void ClickedOpenLevelSelect()
    {
        if (preventAllClicks) return;
        if (isEditing) editorManager.OpenLevelSelect();
    }

    public void ClickedPublishGame()
    {
        preventAllClicks = true;
        editorManager.CloseLevelSelect();
        editorManager.customizePanel.CloseCustomizePanel();
        editorManager.gameObject.SetActive(false);
        gameManager.gameObject.SetActive(false);
        timeManager.SetMultiplier(80f);
    }
}
