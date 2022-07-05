using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractUI : MonoBehaviour
{
    [Header("Interact Prompt")]
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private Image toolIcon;

    [Header("Can't Interact UI")]
    [SerializeField] private Sprite _cantInteractSprite;

    [Header("Toolbar")]
    [SerializeField] private TextMeshProUGUI activeToolText;

    private Tool _activeToolRef;

    private void Start()
    {
        HideInteractPrompt();
    }

    public void ShowInteractPrompt()
    {
        promptText.enabled = true;
        toolIcon.enabled = true;
    }

    public void HideInteractPrompt()
    {
        promptText.enabled = false;
        toolIcon.enabled = false;
    }

    public void ShowCantInteractUI(string prompt, bool prependActiveToolText = false)
    {
        string result = "Can't ";
        if (prependActiveToolText)
        {
            result += _activeToolRef.verbText + " ";
        }
        result += prompt;

        promptText.text = result;
        toolIcon.sprite = _cantInteractSprite;

        ShowInteractPrompt();
    }

    public void UpdatePromptText(string prompt, bool prependActiveToolText = false)
    {
        if (prependActiveToolText)
        {
            prompt = _activeToolRef.verbText + " " + prompt;
        }

        promptText.text = prompt;
    }

    public void UpdateActiveTool(Tool activeTool)
    {
        _activeToolRef = activeTool;

        //Update Toolbar
        activeToolText.text = activeTool.type.ToString();

        //Update Prompt
        toolIcon.sprite = activeTool.sprite;
    }



}
