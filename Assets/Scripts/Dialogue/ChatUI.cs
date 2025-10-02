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

    public void Initialize()
    {
        typingIndicator.SetActive(false);
        chatAreaManager.Initialize();
    }

    public void Show() => chatPanel.SetActive(true);
    public void Hide() => chatPanel.SetActive(false);

    public void SwitchToChatArea(string areaName)
    {
        chatAreaManager.SwitchToChatArea(areaName);
    }

    public void SwitchToChatAreaViaButton(string areaName)
    {
        chatAreaManager.SwitchToChatAreaViaButton(areaName);
    }

    public IEnumerator ShowTypingForDuration(float duration)
    {
        typingIndicator.SetActive(true);
        yield return new WaitForSeconds(duration);
        typingIndicator.SetActive(false);
    }

    public GameObject AddMessageToChat(ChatMessage message, string intendedChatArea = null)
    {
        typingIndicator.SetActive(false);

        string targetArea = message.IsPlayer ? chatAreaManager.CurrentChatAreaName : (intendedChatArea ?? GetSpeakerChatArea(message.Speaker));

        Transform targetContent = chatAreaManager.GetContentForArea(targetArea);

        if (targetContent == null)
        {
            Debug.LogWarning($"No content found for area: {targetArea}");
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
            ChatArea targetChatArea = chatAreaManager.GetChatArea(targetArea);
        }

        Debug.Log($"Message added to {targetArea} (viewing: {chatAreaManager.CurrentChatAreaName}, isPlayer: {message.IsPlayer})");
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