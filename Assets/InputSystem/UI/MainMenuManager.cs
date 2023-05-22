using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _mainMenuCanvasGO;

    private bool isPaused;

    [SerializeField] private GameObject _menuFirst;

    // Start is called before the first frame update
    void Start()
    {
        _mainMenuCanvasGO.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.MenuOpenCloseInput)
        {
            {
                if (!isPaused)
                {
                    Pause();
                }
                else
                {
                    Unpause();
                }
            }
        }
    }

    #region Pause/Unpause Functions

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        OpenMainMenu();
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        CloseMainMenu();
    }
    #endregion

    #region Canvas Activations/Deactivations Functions

    private void OpenMainMenu()
    {
        _mainMenuCanvasGO.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_menuFirst);
    }

    private void CloseMainMenu()
    {
        _mainMenuCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    #endregion

    private void OpenOnePlayerHandle()
    {
       SceneManager.LoadScene(1);
        _mainMenuCanvasGO.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OpenTwoPlayerHandle()
    {
        SceneManager.LoadScene(2);
        _mainMenuCanvasGO.SetActive(false);
        Time.timeScale = 1f;
    }


    #region Main Menu Button Actions

    public void onOnePlayerPress()
    {
        OpenOnePlayerHandle();
    }

    public void onTwoPlayerPress()
    {
        OpenTwoPlayerHandle();
    }

    public void onQuitPress()
    {
        if (UnityEditor.EditorApplication.isPlaying == true) {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        Application.Quit();
    }

    #endregion

}
