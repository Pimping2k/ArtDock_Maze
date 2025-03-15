using Containers;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuButtonController : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button muteMusicBtn;
    [SerializeField] private Button infoBtn;
    [SerializeField] private Button authorBtn;
    [SerializeField] private GameObject infoContainer;
    
    private bool isMuted = true;
    private bool infoActivated = false;
    private void Awake()
    {
        HandleGameStart();

        playBtn.onClick.AddListener(OnPlayButton);
        quitBtn.onClick.AddListener(OnQuitButton);
        muteMusicBtn.onClick.AddListener(OnMuteMusicButton);
        infoBtn.onClick.AddListener(OnInfoButton);
        authorBtn.onClick.AddListener(OnAuthorButton);
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
    
    private void OnAuthorButton()
    {
        Application.OpenURL("https://www.linkedin.com/in/andrey-savastin-b368771a3/");
    }

    private void OnInfoButton()
    {
        infoActivated = !infoActivated;
        infoContainer.SetActive(infoActivated);
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
