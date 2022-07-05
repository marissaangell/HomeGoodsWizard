using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : ManagedMenu
{
    [Header("Internal Panel References")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject controlsPanel;

    public override void ControlledOpen()
    {
        base.ControlledOpen();
        DisplayMainPanel();
    }

    public void DisplayMainPanel()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void OpenOptionsPanel()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void OpenControlsPanel()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
