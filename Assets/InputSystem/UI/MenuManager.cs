using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _mainMenuCanvasGO;

    private bool isPaused;
    private bool isMuted;

    [SerializeField] private GameObject _menuFirst;

    // Start is called before the first frame update
    void Start()
    {
        _mainMenuCanvasGO.SetActive(false);
        isMuted = false;
        AudioListener.pause = isMuted;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.MenuOpenCloseInput)
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

    #region Pause/Unpause Functions

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        isMuted = true;
        AudioListener.pause = isMuted;

        OpenMainMenu();
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        isMuted = false;
        AudioListener.pause = isMuted;

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

    private void OpenMainMenuHandle()
    {
        SceneManager.LoadScene(0);
        _mainMenuCanvasGO.SetActive(false);
    }
    #endregion

    #region Main Menu Button Actions

    public void onResumePress()
    {
        Unpause();
    }

    public void onMainMenuPress()
    {
        OpenMainMenuHandle();
    }
    #endregion

}
