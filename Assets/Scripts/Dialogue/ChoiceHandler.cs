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
    private System.Action<int> onChoiceSelected;
    private List<Choice> currentChoices;
    private string currentChatAreaForChoices;
    private string currentActiveChatArea;

    public bool IsChoicePanelOpen => choicePanelAnimator.GetBool("IsOpen");

    public void Initialize(System.Action<int> choiceCallback, ChatUI chatUIReference)
    {
        onChoiceSelected = choiceCallback;
        choicePanelAnimator = choicePanel.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choiceButtons.Length];
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choicesText[i] = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        inputMessageButton.onClick.AddListener(ToggleChoicePanel);
        ChatAreaEvents.OnChatAreaChanged += OnChatAreaChanged;

        choicePanelAnimator.SetBool("IsOpen", false);
    }

    public void Cleanup()
    {
        ChatAreaEvents.OnChatAreaChanged -= OnChatAreaChanged;
    }

    public void ShowChoices(List<Choice> choices, string currentChatArea)
    {
        currentChatAreaForChoices = currentChatArea;
        currentChoices = choices;
    }

    private void ToggleChoicePanel()
    {
        if (IsChoicePanelOpen)
        {
            CloseChoicePanel();
        }
        else if (IsInCorrectChatArea())
        {
            OpenChoicePanel();
        }
    }

    private void OpenChoicePanel()
    {
        if (!IsInCorrectChatArea()) return;

        choicePanelAnimator.SetBool("IsOpen", true);
        DisplayChoices();
        ChatAreaEvents.TriggerChoicePanelStateChanged(true);
    }

    private void CloseChoicePanel()
    {
        choicePanelAnimator.SetBool("IsOpen", false);
        ChatAreaEvents.TriggerChoicePanelStateChanged(false);
    }

    private void DisplayChoices()
    {
        if (!IsInCorrectChatArea()) return;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            bool shouldShow = i < currentChoices.Count;

            if (shouldShow)
            {
                choicesText[i].text = currentChoices[i].text;
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
                CloseChoicePanel();
            }
        });
    }

    private void SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < currentChoices.Count)
            {
                EventSystem.current.SetSelectedGameObject(choiceButtons[i].gameObject);
                break;
            }
        }
    }

    public void Hide()
    {
        CloseChoicePanel();
        currentChoices = null;
        currentChatAreaForChoices = null;
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
        return currentActiveChatArea == currentChatAreaForChoices && currentChoices != null;
    }
}