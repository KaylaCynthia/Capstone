using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pausePanel;

    [Header("Dialogue System")]
    [SerializeField] private InkFileManager inkFileManager;

    private bool isPaused = false;
    private bool isSettings = false;
    private string currentSceneName;

    private static GameManager instance;
    public static GameManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        StartGame();
    }

    public void StartGame()
    {
        if (inkFileManager != null)
        {
            ChatDialogueManager dialogueManager = ChatDialogueManager.GetInstance();
            if (dialogueManager != null)
            {
                dialogueManager.StartConversation("diluc_intro", "ChatAreaDiluc");
            }
        }
    }

    public void OnConversationComplete()
    {
        Debug.Log("Conversation completed!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentSceneName == "GameScene")
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        if (pausePanel == null) return;

        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Settings()
    {
        if (settingsPanel == null) return;

        isSettings = !isSettings;
        settingsPanel.SetActive(isSettings);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}