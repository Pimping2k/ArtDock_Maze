using System;
using Containers;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonController : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button muteMusicBtn;

    private bool isMuted = true;
    
    private void Awake()
    {
        HandleGameStart();

        playBtn.onClick.AddListener(OnPlayButton);
        quitBtn.onClick.AddListener(OnQuitButton);
        muteMusicBtn.onClick.AddListener(OnMuteMusicButton);
    }

    private static void HandleGameStart()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnPlayButton()
    {
        SceneManager.LoadScene(TagsContainer.GAMESCENE);
    }

    private void OnQuitButton()
    {
        Application.Quit();
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
