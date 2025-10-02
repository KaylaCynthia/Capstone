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
    private System.Action<int> onChoiceSelected;
    private List<Choice> currentChoices;
    private string currentChatAreaForChoices;
    private string currentActiveChatArea;
    private bool choicesAvailable = false;

    public bool IsChoicePanelOpen => choicePanel.activeInHierarchy;

    public void Initialize(System.Action<int> choiceCallback, ChatUI chatUIReference)
    {
        onChoiceSelected = choiceCallback;
        choicePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choiceButtons.Length];
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choicesText[i] = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceButtons[i].SetActive(false);
        }

        inputMessageButton.onClick.AddListener(ToggleChoicePanel);

        ChatAreaEvents.OnChatAreaChanged += OnChatAreaChanged;
    }

    public void Cleanup()
    {
        ChatAreaEvents.OnChatAreaChanged -= OnChatAreaChanged;
    }

    public void ShowChoices(List<Choice> choices, string currentChatArea)
    {
        currentChatAreaForChoices = currentChatArea;
        currentActiveChatArea = currentChatArea;
        choicesAvailable = true;
        inputMessageButton.interactable = true;
        currentChoices = choices;

        Debug.Log($"Choices shown for chat area: {currentChatAreaForChoices}");
    }

    private void ToggleChoicePanel()
    {
        if (choicePanel.activeInHierarchy)
        {
            CloseChoicePanel();
        }
        else
        {
            OpenChoicePanel();
        }
    }

    private void OpenChoicePanel()
    {
        if (choicePanel.activeInHierarchy) return;

        if (!IsInCorrectChatArea() || !choicesAvailable)
        {
            Debug.LogWarning($"Cannot show choices - wrong chat area. Current: {currentActiveChatArea}, Required: {currentChatAreaForChoices}, Choices Available: {choicesAvailable}");
            return;
        }

        choicePanel.SetActive(true);
        DisplayChoices(currentChoices);
        ChatAreaEvents.TriggerChoicePanelStateChanged(true);
    }

    private void CloseChoicePanel()
    {
        choicePanel.SetActive(false);
        foreach (var button in choiceButtons) button.SetActive(false);
        ChatAreaEvents.TriggerChoicePanelStateChanged(false);
    }

    private void DisplayChoices(List<Choice> choices)
    {
        if (choices == null) return;

        if (!IsInCorrectChatArea())
        {
            //Debug.LogWarning("Cannot display choices - chat area mismatch detected");
            CloseChoicePanel();
            return;
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            bool shouldShow = i < choices.Count;
            choiceButtons[i].SetActive(shouldShow);

            if (shouldShow)
            {
                choicesText[i].text = choices[i].text;
                SetupButtonListener(choiceButtons[i].GetComponent<Button>(), i);
            }
        }
        SelectFirstChoice();
    }

    private void SetupButtonListener(Button button, int choiceIndex)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if (IsInCorrectChatArea())
            {
                onChoiceSelected?.Invoke(choiceIndex);
                choicesAvailable = false;
                CloseChoicePanel();
            }
            else
            {
                //Debug.LogWarning($"Choice blocked - current chat area ({currentActiveChatArea}) doesn't match choice area ({currentChatAreaForChoices})");
            }
        });
    }

    private void SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (choiceButtons[i].activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(choiceButtons[i].gameObject);
                break;
            }
        }
    }

    public void Hide()
    {
        CloseChoicePanel();
        inputMessageButton.interactable = false;
        choicesAvailable = false;
        currentChoices = null;
        currentChatAreaForChoices = null;
    }

    private void OnChatAreaChanged(string newAreaName)
    {
        currentActiveChatArea = newAreaName;

        if (choicePanel.activeInHierarchy && !IsInCorrectChatArea())
        {
            //Debug.Log($"Auto-closing choice panel due to chat area switch from {currentChatAreaForChoices} to {newAreaName}");
            CloseChoicePanel();
        }

        if (choicesAvailable)
        {
            inputMessageButton.interactable = IsInCorrectChatArea();
        }
    }

    private bool IsInCorrectChatArea()
    {
        bool isCorrectArea = currentActiveChatArea == currentChatAreaForChoices;
        bool hasChoices = choicesAvailable && currentChoices != null;

        return isCorrectArea && hasChoices;
    }
}