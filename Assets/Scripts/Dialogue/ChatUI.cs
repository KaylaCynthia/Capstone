using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class ChatUI
{
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private ScrollRect chatScrollRect;
    [SerializeField] private Transform chatContent;
    [SerializeField] private GameObject typingIndicator;
    [SerializeField] private RectTransform chatAreaRect;

    private float originalChatAreaHeight;
    private const float CHOICE_PANEL_HEIGHT = 150f;

    public void Initialize()
    {
        chatPanel.SetActive(false);
        typingIndicator.SetActive(false);
        if (chatAreaRect != null)
            originalChatAreaHeight = chatAreaRect.rect.height;
    }

    public void Show() => chatPanel.SetActive(true);
    public void Hide() => chatPanel.SetActive(false);

    public IEnumerator ShowTypingForDuration(float duration)
    {
        typingIndicator.SetActive(true);
        yield return new WaitForSeconds(duration);
        typingIndicator.SetActive(false);
    }

    public IEnumerator ShowTypingIndicator()
    {
        typingIndicator.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        typingIndicator.SetActive(false);
    }

    public GameObject AddMessageToChat(ChatMessage message)
    {
        typingIndicator.SetActive(false);
        GameObject messageUI = message.CreateUIElement(chatContent);

        ChatMessageUI chatMessageUI = messageUI.GetComponentInChildren<ChatMessageUI>();
        if (chatMessageUI != null)
        {
            chatMessageUI.Initialize(message);
        }

        ScrollToBottomDelayed();

        return messageUI;
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
        if (chatScrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatScrollRect.content);
            chatScrollRect.verticalNormalizedPosition = 0f;
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
        if (chatAreaRect != null)
        {
            float targetHeight = showChoices ? originalChatAreaHeight - CHOICE_PANEL_HEIGHT : originalChatAreaHeight;
            chatAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
        }
    }
}