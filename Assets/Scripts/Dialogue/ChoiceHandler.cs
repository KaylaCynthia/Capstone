using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ink.Runtime;

[System.Serializable]
public class ChoiceHandler
{
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button inputMessageButton;
    [SerializeField] private GameObject[] choiceButtons;

    private TextMeshProUGUI[] choicesText;
    private Animator choicePanelAnimator;
    private TextMeshProUGUI inputMessageText;
    private System.Action<int> onChoiceSelected;
    private List<Choice> currentChoices;
    private string currentChatAreaForChoices;
    private string currentActiveChatArea;
    private bool isInitialized = false;
    private bool isClosing = false;

    private const string STAY_CLOSED_TRIGGER = "Stay_Closed";
    private const string SHOW_CHOICE1_TRIGGER = "Show_Choice1";
    private const string SHOW_CHOICE2_TRIGGER = "Show_Choice2";
    private const string SHOW_CHOICE3_TRIGGER = "Show_Choice3";
    private const string CLOSE_CHOICE1_TRIGGER = "Close_Choice1";
    private const string CLOSE_CHOICE2_TRIGGER = "Close_Choice2";
    private const string CLOSE_CHOICE3_TRIGGER = "Close_Choice3";
    private const string IS_OPEN_BOOL = "IsOpen";

    public bool IsChoicePanelOpen => choicePanelAnimator != null && choicePanelAnimator.GetBool(IS_OPEN_BOOL);

    public void Initialize(System.Action<int> choiceCallback, ChatUI chatUI)
    {
        if (isInitialized) return;

        onChoiceSelected = choiceCallback;

        if (choicePanel == null)
        {
            Debug.LogError("ChoicePanel is not assigned in ChoiceHandler!");
            return;
        }

        choicePanelAnimator = choicePanel.GetComponent<Animator>();
        if (choicePanelAnimator == null)
        {
            Debug.LogError("ChoicePanel has no Animator component!");
            return;
        }

        if (chatUI != null)
        {
            currentActiveChatArea = chatUI.GetCurrentChatAreaName();
        }

        if (choiceButtons != null && choiceButtons.Length > 0)
        {
            choicesText = new TextMeshProUGUI[choiceButtons.Length];
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (choiceButtons[i] != null)
                {
                    choicesText[i] = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    choiceButtons[i].SetActive(false);
                }
                else
                {
                    Debug.LogWarning($"Choice button at index {i} is null!");
                }
            }
        }
        else
        {
            Debug.LogError("ChoiceButtons array is empty or null!");
        }

        if (inputMessageButton != null)
        {
            inputMessageButton.onClick.RemoveAllListeners();
            inputMessageButton.onClick.AddListener(ToggleChoicePanel);
        }
        else
        {
            Debug.LogError("InputMessageButton is not assigned in ChoiceHandler!");
        }

        ChatAreaEvents.OnChatAreaChanged += OnChatAreaChanged;

        choicePanel.SetActive(false);

        isInitialized = true;
        Debug.Log("ChoiceHandler Initialized successfully");
    }

    public void ShowChoices(List<Choice> choices, string currentChatArea)
    {
        inputMessageText = inputMessageButton.GetComponentInChildren<TextMeshProUGUI>();
        inputMessageText.color = Color.red;
        currentChatAreaForChoices = currentChatArea;
        currentChoices = choices;
    }

    private void ToggleChoicePanel()
    {
        Debug.Log($"ToggleChoicePanel called. IsOpen: {IsChoicePanelOpen}, InCorrectArea: {IsInCorrectChatArea()}");

        if (IsChoicePanelOpen && !isClosing)
        {
            CloseChoicePanel();
        }
        else if (IsInCorrectChatArea())
        {
            OpenChoicePanel();
        }
        else
        {
            Debug.Log($"Cannot open choice panel. Current area: {currentActiveChatArea}, Required area: {currentChatAreaForChoices}, Has choices: {currentChoices != null}");
        }
    }

    private void OpenChoicePanel()
    {
        if (!IsInCorrectChatArea())
        {
            Debug.LogWarning("Tried to open choice panel in wrong chat area");
            return;
        }

        isClosing = false;
        choicePanel.SetActive(true);
        DisplayChoices();
        TriggerShowAnimation();
        ChatAreaEvents.TriggerChoicePanelStateChanged(true);
    }

    private void CloseChoicePanel()
    {
        if (isClosing) return;

        isClosing = true;
        inputMessageText.color = new Color32(0xFF, 0xE1, 0xBA, 0xFF);
        TriggerCloseAnimation();
        ChatAreaEvents.TriggerChoicePanelStateChanged(false);

        ChatDialogueManager.GetInstance().StartCoroutine(WaitForCloseAnimationThenStayClosed());
    }

    private System.Collections.IEnumerator WaitForCloseAnimationThenStayClosed()
    {
        yield return new WaitForSeconds(0.5f);

        if (choicePanelAnimator != null)
        {
            choicePanelAnimator.SetTrigger(STAY_CLOSED_TRIGGER);
        }

        isClosing = false;
    }

    private void TriggerShowAnimation()
    {
        Debug.Log("TriggerShowAnimation called");
        if (choicePanelAnimator == null) return;

        int choiceCount = currentChoices?.Count ?? 0;

        switch (choiceCount)
        {
            case 1:
                choicePanelAnimator.SetTrigger(SHOW_CHOICE1_TRIGGER);
                break;
            case 2:
                choicePanelAnimator.SetTrigger(SHOW_CHOICE2_TRIGGER);
                break;
            case 3:
                choicePanelAnimator.SetTrigger(SHOW_CHOICE3_TRIGGER);
                break;
            default:
                Debug.LogWarning($"Unsupported number of choices: {choiceCount}");
                choicePanelAnimator.SetBool(IS_OPEN_BOOL, true);
                break;
        }

        choicePanelAnimator.SetBool(IS_OPEN_BOOL, true);
    }

    private void TriggerCloseAnimation()
    {
        Debug.Log("TriggerCloseAnimation called");
        if (choicePanelAnimator == null) return;

        int choiceCount = currentChoices?.Count ?? 0;

        switch (choiceCount)
        {
            case 1:
                Debug.Log("Closing choice panel with 1 choice");
                choicePanelAnimator.SetTrigger(CLOSE_CHOICE1_TRIGGER);
                break;
            case 2:
                choicePanelAnimator.SetTrigger(CLOSE_CHOICE2_TRIGGER);
                break;
            case 3:
                choicePanelAnimator.SetTrigger(CLOSE_CHOICE3_TRIGGER);
                break;
            default:
                break;
        }

        choicePanelAnimator.SetBool(IS_OPEN_BOOL, false);
    }

    private void DisplayChoices()
    {
        if (!IsInCorrectChatArea()) return;

        foreach (var button in choiceButtons)
        {
            if (button != null)
                button.SetActive(false);
        }

        int choiceCount = currentChoices?.Count ?? 0;
        for (int i = 0; i < Mathf.Min(choiceCount, choiceButtons.Length); i++)
        {
            if (choiceButtons[i] != null)
            {
                choiceButtons[i].SetActive(true);
                if (choicesText[i] != null)
                {
                    choicesText[i].text = currentChoices[i].text;
                }

                Button button = choiceButtons[i].GetComponent<Button>();
                if (button != null)
                {
                    int index = i;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => OnChoiceButtonClicked(index));
                }
            }
        }
        SelectFirstChoice();
    }

    private void OnChoiceButtonClicked(int choiceIndex)
    {
        List<Choice> choicesCopy = currentChoices != null ? new List<Choice>(currentChoices) : null;
        int selectedIndex = choiceIndex;

        if (IsInCorrectChatArea() && choiceIndex < currentChoices.Count)
        {
            CloseChoicePanel();

            onChoiceSelected?.Invoke(selectedIndex);
            Debug.Log($"Choice selected: {selectedIndex} - {choicesCopy[selectedIndex].text}");
        }
        else
        {
            Debug.LogError($"Invalid choice selection: index {choiceIndex}, choices count: {currentChoices?.Count}");
        }
    }

    private void SelectFirstChoice()
    {
        if (EventSystem.current == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < currentChoices.Count && choiceButtons[i] != null && choiceButtons[i].activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(choiceButtons[i].gameObject);
                break;
            }
        }
    }

    public void Hide()
    {
        TriggerCloseAnimation();
        ChatAreaEvents.TriggerChoicePanelStateChanged(false);

        currentChoices = null;
        currentChatAreaForChoices = null;

        ChatDialogueManager.GetInstance().StartCoroutine(DeactivatePanelAfterDelay());
    }

    private System.Collections.IEnumerator DeactivatePanelAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
    }

    private void OnChatAreaChanged(string newAreaName)
    {
        currentActiveChatArea = newAreaName;

        if (IsChoicePanelOpen && !IsInCorrectChatArea())
        {
            CloseChoicePanel();
        }
    }

    private bool IsInCorrectChatArea()
    {
        bool isCorrect = currentActiveChatArea == currentChatAreaForChoices && currentChoices != null && currentChoices.Count > 0;
        return isCorrect;
    }
}