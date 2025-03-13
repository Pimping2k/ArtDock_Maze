using System;
using Containers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;

    private void Awake()
    {
        HandleGameStart();

        playBtn.onClick.AddListener(OnPlayButton);
        quitBtn.onClick.AddListener(OnQuitButton);
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
}
