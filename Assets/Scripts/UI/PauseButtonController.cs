using System;
using Containers;
using Managers;
using PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour
{
    [SerializeField] private PauseController pauseController;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button muteMusicBtn;

    private bool isMuted = true;
    
    private void Awake()
    {
        continueBtn.onClick.AddListener(OnContinue);
        quitBtn.onClick.AddListener(OnQuit);
        mainMenuBtn.onClick.AddListener(OnMainMenu);
        muteMusicBtn.onClick.AddListener(OnMuteMusicButton);
    }

    private void OnContinue()
    {
        pauseController.ForceUnpause();
    }

    private void OnQuit()
    {
        Application.Quit();
    }

    private void OnMainMenu()
    {
        SceneManager.LoadScene(TagsContainer.MAINMENU);
    }
    
    private void OnMuteMusicButton()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            MusicManager.Instance.UnMuteMusic();
        }
        else
        {
            MusicManager.Instance.MuteMusic();
        }
    }
}