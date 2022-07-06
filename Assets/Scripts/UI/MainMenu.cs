using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

    [Header("Save/Load References")]
    public Button loadButton;
    //public GameObject overwriteSavePanel;

    [Header("First Selected Panel Elements")]
    public Selectable mainPanelSelectable;
    public Selectable optionsPanelSelectable;
    public Selectable controlsPanelSelectable;
    public Selectable creditsPanelSelectable;

    //private SoundManager sound;

    private readonly int DEFAULT_SAVE_SLOT = 0;

    public void Start()
    {
        Time.timeScale = 1;

        //sound = FindObjectOfType<SoundManager>();
        //sound.Play("Music-MainMenu");

        loadButton.interactable = SaveSystem.SaveDataExists(DEFAULT_SAVE_SLOT);

        DisplayMainPanel();
    }

    public void DisplayMainPanel()
    {
        HideAllPanels();

        mainPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        if (mainPanelSelectable != null) mainPanelSelectable.Select();
    }

    public void BackToMainPanel()
    {
        DisplayMainPanel();
    }

    /*
     * Start Buttons
     */

    public void StartNewGame()
    {
        RecordKeeper.ResetState();

        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("Cabin");
    }

    /*public void NewGame()
    {
        *//*sound.Play("UI_Select");

        if (SaveSystem.SaveDataExists()) //display a dialogue asking if the player wants to overwrite their save data
        {
            overwriteSavePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            overwritePanelSelectable.Select();
        }
        else
            StartNewGame();*//*
        StartNewGame();
    }*/

    public void LoadGame()
    {
        /*sound.Play("UI_Select");
        sound.Stop("Music-MainMenu");*/

        //lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        //load save data
        SaveData saveData = SaveSystem.GetStoredSaveData(DEFAULT_SAVE_SLOT);
        bool success = RecordKeeper.RestoreSavedState(saveData.SaveState);

        //if save data couldn't be loaded, unlock cursor
        if (!success) { Cursor.lockState = CursorLockMode.None; }
    }




    /*
     * Options Menu
     */
    public void OpenOptions()
    {
        HideAllPanels();
        optionsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        if (optionsPanelSelectable != null) optionsPanelSelectable.Select();
    }


    /*
     * Controls Menu
     */
    public void OpenControls()
    {
        HideAllPanels();
        controlsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        if (controlsPanelSelectable != null) controlsPanelSelectable.Select();
    }


    /*
     * Credits Panel
     */
    public void OpenCredits()
    {
        HideAllPanels();
        creditsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        if (creditsPanelSelectable != null) creditsPanelSelectable.Select();
    }



    /*
     * Quit Button
     */

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }



    private void HideAllPanels()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
}
