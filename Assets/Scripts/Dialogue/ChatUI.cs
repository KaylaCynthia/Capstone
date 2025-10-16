using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ChatUI
{
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private GameObject typingIndicator;
    [SerializeField] private ChatAreaManager chatAreaManager;

    private Coroutine currentTypingCoroutine;
    private string currentTypingChatArea;
    private bool isTypingInProgress = false;

    public void Initialize()
    {
        typingIndicator.SetActive(false);
        chatAreaManager.Initialize();

        ChatAreaEvents.OnChatAreaChanged += OnChatAreaChanged;
    }

    public void Cleanup()
    {
        ChatAreaEvents.OnChatAreaChanged -= OnChatAreaChanged;

        if (currentTypingCoroutine != null)
        {
            ChatDialogueManager.GetInstance().StopCoroutine(currentTypingCoroutine);
            currentTypingCoroutine = null;
        }
        isTypingInProgress = false;
    }

    private void OnChatAreaChanged(string newAreaName)
    {
        UpdateTypingIndicatorVisibility();
    }

    private void UpdateTypingIndicatorVisibility()
    {
        if (isTypingInProgress)
        {
            bool shouldShow = GetCurrentChatAreaName() == currentTypingChatArea;
            typingIndicator.SetActive(shouldShow);

            if (shouldShow)
            {
                ChatArea currentArea = chatAreaManager.CurrentChatArea;
                TextMeshProUGUI typingText = typingIndicator.GetComponentInChildren<TextMeshProUGUI>();

                if (typingText != null && currentArea != null)
                {
                    if (currentArea.AreaType == ChatAreaType.Server)
                    {
                        typingText.text = "Somebody is typing...";
                    }
                    else
                    {
                        string currentPerson = currentArea.AreaName.Replace("ChatArea", "");
                        typingText.text = $"{currentPerson} is typing...";
                    }
                }
            }
        }
        else
        {
            typingIndicator.SetActive(false);
        }
    }

    public void Show() => chatPanel.SetActive(true);
    public void Hide() => chatPanel.SetActive(false);

    public void SwitchToChatArea(string areaName)
    {
        chatAreaManager.SwitchToChatArea(areaName);
        ScrollToBottom();
    }

    public void SwitchToChatAreaViaButton(string areaName)
    {
        chatAreaManager.SwitchToChatAreaViaButton(areaName);
    }

    public IEnumerator ShowTypingForDuration(float duration, string targetChatArea)
    {
        currentTypingChatArea = targetChatArea;
        isTypingInProgress = true;

        UpdateTypingIndicatorVisibility();

        yield return new WaitForSeconds(duration);

        isTypingInProgress = false;
        typingIndicator.SetActive(false);
        currentTypingCoroutine = null;
    }

    public GameObject AddMessageToChat(ChatMessage message, string intendedChatArea = null)
    {
        string targetArea = message.IsPlayer ? chatAreaManager.CurrentChatAreaName : (intendedChatArea ?? GetSpeakerChatArea(message.Speaker));

        if (targetArea == currentTypingChatArea && isTypingInProgress)
        {
            isTypingInProgress = false;
            typingIndicator.SetActive(false);

            if (currentTypingCoroutine != null)
            {
                ChatDialogueManager.GetInstance().StopCoroutine(currentTypingCoroutine);
                currentTypingCoroutine = null;
            }
        }

        Transform targetContent = chatAreaManager.GetContentForArea(targetArea);

        if (targetContent == null)
        {
            Debug.LogWarning($"No content found for area: {targetArea}");
            return null;
        }

        if (message.IsPlayer && string.IsNullOrEmpty(message.Message))
        {
            Debug.LogWarning("Attempted to create empty player message, skipping.");
            return null;
        }

        GameObject messageUI = message.CreateUIElement(targetContent);

        ChatMessageUI chatMessageUI = messageUI.GetComponentInChildren<ChatMessageUI>();
        if (chatMessageUI != null)
        {
            chatMessageUI.Initialize(message);
        }

        if (chatAreaManager.IsAreaActive(targetArea))
        {
            ScrollToBottomDelayed();
        }
        else
        {
            if (!message.IsPlayer)
            {
                ChatNotificationEvents.TriggerNewMessage(targetArea);
            }
        }

        return messageUI;
    }

    private string GetSpeakerChatArea(string speakerName)
    {
        return chatAreaManager.CurrentChatAreaName;
    }

    public void AppendToMessage(GameObject messageUI, string additionalText)
    {
        ChatMessageUI chatMessageUI = messageUI.GetComponentInChildren<ChatMessageUI>();
        if (chatMessageUI != null)
        {
            chatMessageUI.AppendMessage(additionalText);
        }

        ScrollToBottomDelayed();
    }

    public void ScrollToBottom()
    {
        ScrollRect currentScrollRect = chatAreaManager.GetCurrentScrollRect();
        if (currentScrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(currentScrollRect.content);
            currentScrollRect.verticalNormalizedPosition = 0f;
            Canvas.ForceUpdateCanvases();
        }
    }

    private void ScrollToBottomDelayed()
    {
        ChatDialogueManager.GetInstance().StartCoroutine(ScrollToBottomCoroutine());
    }

    private IEnumerator ScrollToBottomCoroutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        ScrollToBottom();
    }

    public void AdjustForChoices(bool showChoices)
    {
        chatAreaManager.AdjustCurrentChatAreaForChoices(showChoices);
    }

    public string GetCurrentChatAreaName()
    {
        if (chatAreaManager != null)
        {
            return chatAreaManager.CurrentChatAreaName;
        }
        return "DefaultArea";
    }
}