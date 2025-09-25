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

    public void Initialize(System.Action<int> choiceCallback)
    {
        onChoiceSelected = choiceCallback;
        choicePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choiceButtons.Length];
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choicesText[i] = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceButtons[i].SetActive(false);
        }

        inputMessageButton.onClick.AddListener(OnInputMessageClicked);
    }

    public void ShowChoices(List<Choice> choices)
    {
        inputMessageButton.interactable = true;
        currentChoices = choices;
    }

    private void OnInputMessageClicked()
    {
        if (choicePanel.activeInHierarchy) return;

        choicePanel.SetActive(true);
        DisplayChoices(currentChoices);
    }

    private void DisplayChoices(List<Choice> choices)
    {
        if (choices == null) return;

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
        button.onClick.AddListener(() => onChoiceSelected?.Invoke(choiceIndex));
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
        choicePanel.SetActive(false);
        inputMessageButton.interactable = false;
        foreach (var button in choiceButtons) button.SetActive(false);
        currentChoices = null;
    }
}