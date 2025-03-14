using System;
using Containers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button mainMenuBtn;

    private void Awake()
    {
        continueBtn.onClick.AddListener(OnContinue);
        quitBtn.onClick.AddListener(OnQuit);
        mainMenuBtn.onClick.AddListener(OnMainMenu);
    }

    private void OnContinue()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnQuit()
    {
        Application.Quit();
    }

    private void OnMainMenu()
    {
        SceneManager.LoadScene(TagsContainer.MAINMENU);
    }
}