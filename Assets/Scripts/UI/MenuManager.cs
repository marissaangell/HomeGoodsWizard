using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("Managed Menus")]
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private RecipeBookUI recipeBookUI;
    [SerializeField] private GameCompleteScreen gameCompleteScreen;

    [Header("Player Component References")]
    [SerializeField] private CanvasGroup playerHUD;
    [SerializeField] private PlayerInput playerInput;

    private ManagedMenu openMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.ControlledClose();
        recipeBookUI.ControlledClose();

        Debug.Log("Menu Manager init");
        Time.timeScale = 1;
        playerInput.SwitchCurrentActionMap("Player");
    }

    void OnPause()
    {
        if (openMenu != null 
            && openMenu.allowCloseOnPause) { CloseMenu(openMenu); } // close active menu
        else if (openMenu == null)         { OpenMenu(pauseMenu); } // open pause menu (if no active menu)
    }

    void OnFurnitureCatalog()
    {
        if (openMenu == null) { OpenMenu(recipeBookUI); } // open recipe menu 
        else if (openMenu == recipeBookUI) { CloseMenu(recipeBookUI); }
    }

    private void OpenMenu(ManagedMenu menu, bool pauseTime = true)
    {
        // Set time scale & cursor state
        if (pauseTime) { Time.timeScale = 0; }
        Cursor.lockState = CursorLockMode.None;

        // Hide Player HUD and switch to UI action map
        playerHUD.gameObject.SetActive(false);
        playerInput.SwitchCurrentActionMap("UI");

        // Open the specified menu
        menu.ControlledOpen();
        openMenu = menu;
    }

    private void CloseMenu(ManagedMenu menu)
    {
        // Set time scale & cursor state
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;

        // Show Player HUD and switch to Player action map
        playerHUD.gameObject.SetActive(true);
        playerInput.SwitchCurrentActionMap("Player");

        // Close the menu that's open
        openMenu.ControlledClose();
        openMenu = null;
    }

    public void RequestOpen(ManagedMenu menu)
    {
        Debug.Log("Received open menu request for: " + menu.name);
        // Deny request if Pause Menu or Recipe Book is open
        if (openMenu != null && (openMenu.Equals(pauseMenu) || openMenu.Equals(recipeBookUI)))
            return;

        // Close any open menu
        if (openMenu != null)
            CloseMenu(openMenu);

        // Open the new menu
        OpenMenu(menu);
    }

    public void RequestClose(ManagedMenu menu)
    {
        if (openMenu.Equals(menu))
        {
            CloseMenu(menu);
        }
    }

    public void CompleteGame()
    {
        OpenMenu(gameCompleteScreen);
    }

    private void OnDisable()
    {
        RecordKeeper.SaveCurrentGameState();
    }
}

[System.Serializable]
public abstract class ManagedMenu : MonoBehaviour
{
    [Header("Managed Menu (Inherited)")]
    [SerializeField] protected MenuManager menuManager;
    [SerializeField] protected GameObject menuUIRoot;
    [SerializeField] public bool allowCloseOnPause = true;
    public bool IsOpen { get; protected set; }

    public virtual void SetMenuManager(MenuManager newManager)
    {
        menuManager = newManager;
    }

    public virtual void ControlledOpen()
    {
        menuUIRoot.gameObject.SetActive(true);
        IsOpen = true;
    }

    public virtual void ControlledClose()
    {
        menuUIRoot.gameObject.SetActive(false);
        IsOpen = false;
    }

    public void RequestCloseMenu()
    {
        menuManager.RequestClose(this);
    }
}
