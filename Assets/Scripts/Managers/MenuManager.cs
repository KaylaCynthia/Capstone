using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private static MenuManager instance;
    public static MenuManager GetInstance() => instance;
    private bool isSettings = false;
    private bool isCredits = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameContent");
    }

    public void Settings()
    {
        if (settingsPanel == null) return;

        isSettings = !isSettings;
        settingsPanel.SetActive(isSettings);
    }

    public void Credits()
    {
        if (creditsPanel == null) return;

        isCredits = !isCredits;
        creditsPanel.SetActive(isCredits);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}