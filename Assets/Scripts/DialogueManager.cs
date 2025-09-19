using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject content;
    [SerializeField] private Button inputMessageBox;
    [SerializeField] private GameObject typingIndicator;
    [SerializeField] private GameObject userSetPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float typingDelay = 0.05f;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button[] choiceButtons;

    [Header("Ink Tags")]
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";

    private const float FONT_SIZE = 36f;
    private const float TEXT_BOX_WIDTH = 775f;
    private const float MIN_HEIGHT = 50f;
    private const float VERTICAL_PADDING = 0f;

    private Story currentStory;
    private string currentSpeaker;
    private string currentPortrait;
    private GameObject currentMessageBubble;
    [SerializeField] private TextMeshProUGUI currentMessageText;
    private bool isCurrentlyTyping = false;
    private bool isPlayerTurn = false;

    private Queue<GameObject> messageQueue = new Queue<GameObject>();
    [SerializeField] private int maxMessages = 50;

    private void Start()
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int choiceIndex = i;
            choiceButtons[i].onClick.AddListener(() => MakeChoice(choiceIndex));
        }

        choicePanel.SetActive(false);
        typingIndicator.SetActive(false);

        //inputMessageBox.onClick.AddListener(OnInputMessageButtonClicked);
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        if (inkJSON == null)
        {
            Debug.LogError("No Ink JSON provided!");
            return;
        }

        currentStory = new Story(inkJSON.text);

        ResetMessageBubbleReference();
        currentSpeaker = null;
        currentPortrait = null;

        StartCoroutine(ProcessStoryCoroutine());
    }

    private IEnumerator ProcessStoryCoroutine()
    {
        while (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();

            string previousSpeaker = currentSpeaker;

            HandleTags(currentStory.currentTags);

            string messageText = nextLine.Trim();

            bool isNewSpeaker = previousSpeaker != currentSpeaker;

            if (isNewSpeaker || currentMessageBubble == null)
            {
                yield return StartCoroutine(CreateNewMessageBubble(messageText));
            }
            else
            {
                yield return StartCoroutine(AppendToCurrentMessage("\n" + messageText));
            }

            if (currentStory.currentChoices.Count > 0)
            {
                yield return StartCoroutine(ShowChoices());
                break;
            }
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
                continue;
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    currentSpeaker = tagValue;
                    break;
                case PORTRAIT_TAG:
                    currentPortrait = tagValue;
                    PlayPortraitAnimation(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private void PlayPortraitAnimation(string animationName)
    {
        if (currentMessageBubble != null)
        {
            Animator portraitAnimator = currentMessageBubble.GetComponentInChildren<Animator>();
            if (portraitAnimator != null)
            {
                portraitAnimator.Play(animationName);
            }
        }
    }

    private IEnumerator ShowChoices()
    {
        inputMessageBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        isPlayerTurn = true;

        while (isPlayerTurn && !choicePanel.activeSelf)
        {
            yield return null;
        }
    }

    public void OnInputMessageButtonClicked()
    {
        if (isPlayerTurn)
        {
            ShowChoicePanel();
        }
    }

    private void ShowChoicePanel()
    {
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < currentStory.currentChoices.Count)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentStory.currentChoices[i].text;
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (isPlayerTurn && choiceIndex < currentStory.currentChoices.Count)
        {
            string choiceText = currentStory.currentChoices[choiceIndex].text;
            CreatePlayerMessage(choiceText);

            currentStory.ChooseChoiceIndex(choiceIndex);

            choicePanel.SetActive(false);
            inputMessageBox.gameObject.SetActive(false);
            isPlayerTurn = false;

            ResetMessageBubbleReference();

            StartCoroutine(ProcessStoryCoroutine());
        }
    }

    private void CreatePlayerMessage(string message)
    {
        GameObject playerMessage = Instantiate(userSetPrefab, content.transform);

        UserMessage userMessageComponent = playerMessage.GetComponent<UserMessage>();
        if (userMessageComponent != null)
        {
            userMessageComponent.SetMessage(message, "Player");

            Transform usersName = playerMessage.transform.Find("UsersName");
            if (usersName != null)
            {
                Transform textMessageBox = usersName.Find("TextMessageBox");
                if (textMessageBox != null)
                {
                    TextMeshProUGUI messageText = textMessageBox.GetComponent<TextMeshProUGUI>();
                    if (messageText != null)
                    {
                        ResizeTextBox(textMessageBox.gameObject, message);
                    }
                }
            }

            Animator portraitAnimator = playerMessage.GetComponentInChildren<Animator>();
            if (portraitAnimator != null)
            {
                portraitAnimator.Play("player_default");
            }
        }
        else
        {
            Transform usersName = playerMessage.transform.Find("UsersName");
            if (usersName != null)
            {
                Transform textMessageBox = usersName.Find("TextMessageBox");
                if (textMessageBox != null)
                {
                    TextMeshProUGUI messageText = textMessageBox.GetComponent<TextMeshProUGUI>();
                    if (messageText != null)
                    {
                        messageText.text = message;
                        messageText.fontSize = FONT_SIZE;
                        ResizeTextBox(textMessageBox.gameObject, message);
                    }
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(playerMessage.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();

        messageQueue.Enqueue(playerMessage);
        if (messageQueue.Count > maxMessages)
        {
            GameObject oldMessage = messageQueue.Dequeue();
            Destroy(oldMessage);
        }

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private IEnumerator CreateNewMessageBubble(string message)
    {
        scrollRect.enabled = false;
        ShowTypingIndicator(currentSpeaker);
        yield return new WaitForSeconds(0.5f);
        HideTypingIndicator();

        currentMessageBubble = Instantiate(userSetPrefab, content.transform);

        Transform usersName = currentMessageBubble.transform.Find("UsersName");
        if (usersName != null)
        {
            Transform textMessageBox = usersName.Find("TextMessageBox");
            if (textMessageBox != null)
            {
                currentMessageText = textMessageBox.GetComponent<TextMeshProUGUI>();
                if (currentMessageText != null)
                {
                    currentMessageText.fontSize = FONT_SIZE;
                    currentMessageText.enableAutoSizing = false;
                }
            }
        }

        UserMessage userMessageComponent = currentMessageBubble.GetComponent<UserMessage>();
        if (userMessageComponent != null)
        {
            userMessageComponent.SetMessage(message, currentSpeaker);
            if (!string.IsNullOrEmpty(currentPortrait))
            {
                Animator portraitAnimator = currentMessageBubble.GetComponentInChildren<Animator>();
                if (portraitAnimator != null)
                {
                    portraitAnimator.Play(currentPortrait);
                }
            }
        }
        else
        {
            if (currentMessageText != null)
            {
                currentMessageText.text = $"{currentSpeaker}: {message}";
                ResizeTextBox(currentMessageText.gameObject, $"{currentSpeaker}: {message}");
            }
        }

        if (usersName != null)
        {
            Transform textMessageBox = usersName.Find("TextMessageBox");
            if (textMessageBox != null)
            {
                ResizeTextBox(textMessageBox.gameObject, message);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(currentMessageBubble.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();

        messageQueue.Enqueue(currentMessageBubble);
        if (messageQueue.Count > maxMessages)
        {
            GameObject oldMessage = messageQueue.Dequeue();
            Destroy(oldMessage);
        }

        scrollRect.enabled = true;
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private IEnumerator AppendToCurrentMessage(string text)
    {
        scrollRect.enabled = false;
        ShowTypingIndicator(currentSpeaker);
        yield return new WaitForSeconds(0.3f);
        HideTypingIndicator();

        string currentText = currentMessageText.text;
        currentMessageText.text += text;

        ResizeTextBox(currentMessageText.gameObject, currentMessageText.text);

        LayoutRebuilder.ForceRebuildLayoutImmediate(currentMessageBubble.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();

        scrollRect.enabled = true;
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        yield return null;
    }

    private void ResizeTextBox(GameObject textBoxObject, string text)
    {
        TextMeshProUGUI textComponent = textBoxObject.GetComponent<TextMeshProUGUI>();
        RectTransform textRect = textBoxObject.GetComponent<RectTransform>();

        if (textComponent != null && textRect != null)
        {
            textComponent.text = text;
            textComponent.ForceMeshUpdate();

            float preferredHeight = textComponent.preferredHeight;

            float calculatedHeight = Mathf.Max(MIN_HEIGHT, preferredHeight + VERTICAL_PADDING);

            float maxHeight = 600f;
            calculatedHeight = Mathf.Min(calculatedHeight, maxHeight);

            textRect.sizeDelta = new Vector2(TEXT_BOX_WIDTH, calculatedHeight);

            Transform usersName = textBoxObject.transform.parent;
            if (usersName != null)
            {
                RectTransform usersNameRect = usersName.GetComponent<RectTransform>();
                if (usersNameRect != null)
                {
                    usersNameRect.sizeDelta = new Vector2(usersNameRect.sizeDelta.x, calculatedHeight + 50);
                }
            }

            Transform usersSet = usersName?.parent;
            if (usersSet != null)
            {
                RectTransform usersSetRect = usersSet.GetComponent<RectTransform>();
                if (usersSetRect != null)
                {
                    float maxChildHeight = calculatedHeight;

                    Transform usersPortrait = usersSet.Find("UsersPortrait");
                    if (usersPortrait != null)
                    {
                        RectTransform portraitRect = usersPortrait.GetComponent<RectTransform>();
                        if (portraitRect != null)
                        {
                            maxChildHeight = Mathf.Max(maxChildHeight, portraitRect.sizeDelta.y);
                        }
                    }

                    float totalHeight = maxChildHeight + 20f;
                    usersSetRect.sizeDelta = new Vector2(900f, totalHeight);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);
            Canvas.ForceUpdateCanvases();

            if (content != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
            }
        }
    }

    private void ShowTypingIndicator(string speaker)
    {
        isCurrentlyTyping = true;
        typingIndicator.SetActive(true);

        TextMeshProUGUI typingText = typingIndicator.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void HideTypingIndicator()
    {
        isCurrentlyTyping = false;
        typingIndicator.SetActive(false);
    }

    private void ResetMessageBubbleReference()
    {
        currentMessageBubble = null;
        currentMessageText = null;
        currentSpeaker = null;
    }
}