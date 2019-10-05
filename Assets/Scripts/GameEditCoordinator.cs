using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEditCoordinator : MonoBehaviour
{
    public GameManager gameManager;
    public EditorManager editorManager;
    public Animation toolPanelAnims;
    public TextMeshProUGUI btnText;
    bool isEditing = true;

    public void ClickedSwitchButton()
    {
        if (isEditing) {
            if (!editorManager.hasPlayer) return;
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
            editorManager.gameObject.SetActive(true);
            gameManager.gameObject.SetActive(false);
            toolPanelAnims.Play("opentools");
            btnText.text = "Play the level";
        }
        isEditing = !isEditing;
    }
}
