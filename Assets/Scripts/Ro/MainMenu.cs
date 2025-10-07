using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private Stack<GameObject> lastGameObject = new Stack<GameObject>();
    private Stack<GameObject> lastSelected = new Stack<GameObject>();
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private GameObject pauseMenu;

    public static MainMenu instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void OpenMenu(MenuSO menu)
    {
        OpenMenu(menu.menus);
    }

    public void OpenMenu(Menus menu)
    {
        switch (menu)
        {
            case Menus.Settings:
                settingsMenu.SetActive(true);
                lastGameObject.Push(settingsMenu);
                break;
            case Menus.Credits:
                creditsScreen.SetActive(true);
                lastGameObject.Push(creditsScreen);
                break;
            case Menus.PauseMenu:
                pauseMenu.SetActive(true);
                lastGameObject.Push(pauseMenu);
                break;
            default:
                break;
        }
    }

    public void OpenPauseMenu(UnityEngine.InputSystem.InputAction.CallbackContext cont)
    {
        OpenMenu(Menus.PauseMenu);
    }

    public void AddButton(GameObject button)
    {
        lastSelected.Push(button);
    }

    public void BackButton(UnityEngine.InputSystem.InputAction.CallbackContext cont)
    {
        if (lastGameObject.Count > 0)
        {
            GameObject lastMenu = lastGameObject.Pop();
            lastMenu.SetActive(false);
        }
        if (lastSelected.Count > 0)
        {
            GameObject selected = lastSelected.Pop();
            EventSystem.current.SetSelectedGameObject(selected);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}

public enum Menus
{
    Settings,
    Credits,
    PauseMenu
}
