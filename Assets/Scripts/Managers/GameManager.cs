using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private NameDataUI nameDataUI;

    [Header("Dependencies")]
    [SerializeField] private DayManager dayManager;
    [SerializeField] private AppSystemManager appSystemManager;
    [SerializeField] private GameStateManager gameStateManager;

    private bool isPaused = false;
    private bool isSettings = false;
    private string currentSceneName;
    private bool hasPlayerSetName = false;

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
        InitializeDaySystem();
        InitializeAdditionalManagers();

        CheckPlayerName();
    }

    private void InitializeAdditionalManagers()
    {
        if (FirstDayManager.GetInstance() == null)
        {
            GameObject firstDayObj = new GameObject("FirstDayManager");
            firstDayObj.transform.SetParent(transform);
            firstDayObj.AddComponent<FirstDayManager>();
        }

        if (ChatAreaUnlockManager.GetInstance() == null)
        {
            GameObject unlockObj = new GameObject("ChatAreaUnlockManager");
            unlockObj.transform.SetParent(transform);
            unlockObj.AddComponent<ChatAreaUnlockManager>();
        }
    }

    private void CheckPlayerName()
    {
        string savedName = PlayerPrefs.GetString("PlayerName", "");
        if (PlayerData.IsValidPlayerName(savedName))
        {
            hasPlayerSetName = true;
            StartCoroutine(StartGameWithTransition());
        }
        else
        {
            ShowNameInputPanel();
        }
    }

    private void ShowNameInputPanel()
    {
        if (nameDataUI != null)
        {
            Debug.Log("Showing name input panel");
            nameDataUI.Show(OnNameConfirmed, OnNamePanelClosed);
        }
        else
        {
            Debug.LogError("NameDataPanel reference is not set in GameManager!");
            UseDefaultNameAndStart();
        }
    }

    private void OnNameConfirmed()
    {
        Debug.Log("Player name confirmed");
        hasPlayerSetName = true;
        StartCoroutine(StartGameWithTransition());
    }

    private void OnNamePanelClosed()
    {
        if (!hasPlayerSetName)
        {
            Debug.Log("Name panel closed, using default name");
            UseDefaultNameAndStart();
        }
    }

    private void UseDefaultNameAndStart()
    {
        PlayerDataManager.GetInstance().SetPlayerName("Player");
        hasPlayerSetName = true;
        StartCoroutine(StartGameWithTransition());
    }

    private void InitializeDaySystem()
    {
        if (dayManager != null)
        {
            dayManager.SetDay(1);
        }
    }

    private IEnumerator StartGameWithTransition()
    {
        if (dayManager != null)
        {
            yield return StartCoroutine(dayManager.ShowFirstDayTransition());
        }

        yield return null;

        if (appSystemManager != null)
        {
            appSystemManager.LaunchEchocordImmediately();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentSceneName == "GameScene")
            {
                if (appSystemManager != null && appSystemManager.IsAppOpen)
                {
                    appSystemManager.ReturnToHomeScreen();
                }
                else
                {
                    Pause();
                }
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

    public void ShowNameChangePanel()
    {
        if (nameDataUI != null)
        {
            nameDataUI.Show();
        }
    }

    public bool HasPlayerSetName()
    {
        return hasPlayerSetName;
    }

    public string GetPlayerName()
    {
        return PlayerDataManager.GetInstance().PlayerData.PlayerName;
    }
}