using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject pausePanel;

    private bool isPaused = false;
    private bool isSettings = false;

    private string currentSceneName;
    private DialogueManager dialogueManager;
    private InkyStories inkyStories;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        inkyStories = FindAnyObjectByType<InkyStories>();
        if (currentSceneName == "GameContent" && dialogueManager != null)
        {
            TextAsset inkJSON = inkyStories.inkStoryFiles[0];
            dialogueManager.StartDialogue(inkJSON);
        }
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