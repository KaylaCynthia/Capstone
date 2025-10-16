using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppSystemManager : MonoBehaviour
{
    [System.Serializable]
    public class AppInfo
    {
        public string appName;
        public GameObject appPanel;
        public Button appButton;
        public AppType appType;
    }

    public enum AppType
    {
        Echocord,
        Settings,
        MiniGame,
        Exercise,
        Work,
        Sleep,
        Bank,
        Other
    }

    [Header("Home Screen")]
    [SerializeField] private GameObject homeScreen;

    [Header("All Applications")]
    [SerializeField] private List<AppInfo> applications = new List<AppInfo>();

    [Header("Dependencies")]
    [SerializeField] private GameStateManager gameStateManager;

    private GameObject currentOpenApp;
    private AppType currentAppType;
    private bool isFirstLaunch = true;

    private static AppSystemManager instance;
    public static AppSystemManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        InitializeAllApps();
    }

    private void InitializeAllApps()
    {
        homeScreen.SetActive(false);

        foreach (AppInfo app in applications)
        {
            if (app.appButton != null)
                app.appButton.onClick.AddListener(() => LaunchApp(app.appName));
        }
    }

    public void LaunchEchocordImmediately()
    {
        AppInfo echocordApp = applications.Find(app => app.appType == AppType.Echocord);

        if (echocordApp != null)
        {
            Debug.Log("Launching Echocord immediately after day transition");
            currentOpenApp = echocordApp.appPanel;
            currentAppType = AppType.Echocord;

            echocordApp.appPanel.SetActive(true);

            StartCoroutine(StartFirstConversationDelayed());
        }
        else
        {
            Debug.LogError("Echocord app not found in applications list!");
        }
    }

    public void LaunchApp(string appName)
    {
        AppInfo targetApp = applications.Find(app => app.appName == appName);

        if (targetApp == null)
        {
            Debug.LogWarning($"App not found: {appName}");
            return;
        }

        if (currentOpenApp != null)
        {
            CloseCurrentApp();
        }

        homeScreen.SetActive(false);
        targetApp.appPanel.SetActive(true);
        currentOpenApp = targetApp.appPanel;
        currentAppType = targetApp.appType;

        HandleAppSpecificInitialization(targetApp);
    }

    private void HandleAppSpecificInitialization(AppInfo app)
    {
        switch (app.appType)
        {
            case AppType.Echocord:
                if (!isFirstLaunch)
                {
                    gameStateManager.StartConversationForCurrentDay();
                }
                break;

            case AppType.Exercise:
            case AppType.Work:
            case AppType.Sleep:
            case AppType.Bank:
                break;
        }

        Debug.Log($"Launched {app.appName} ({app.appType})");
    }

    private System.Collections.IEnumerator StartFirstConversationDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        GameStateManager.GetInstance().StartFirstConversation();
        isFirstLaunch = false;
    }

    public void ReturnToHomeScreen()
    {
        if (currentOpenApp == null) return;

        CloseCurrentApp();
        homeScreen.SetActive(true);
    }

    private void CloseCurrentApp()
    {
        currentOpenApp.SetActive(false);
        currentOpenApp = null;
    }

    public bool IsAppOpen => currentOpenApp != null;

    public void RegisterApp(string appName, GameObject appPanel, Button appButton, AppType appType)
    {
        applications.Add(new AppInfo
        {
            appName = appName,
            appPanel = appPanel,
            appButton = appButton,
            appType = appType
        });

        appPanel.SetActive(false);
        appButton.onClick.AddListener(() => LaunchApp(appName));
    }
}