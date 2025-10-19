using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ChatAreaManager
{
    [SerializeField] private Transform chatAreasParent;

    private Dictionary<string, ChatArea> chatAreas = new Dictionary<string, ChatArea>();
    private string currentChatAreaName;
    private bool choicePanelIsOpen = false;

    private ChatAreaUnlockManager unlockManager;
    private ChatAreaButtonManager buttonManager;
    private ChatUI chatUI;

    public string CurrentChatAreaName => currentChatAreaName;
    public ChatArea CurrentChatArea => chatAreas.ContainsKey(currentChatAreaName) ? chatAreas[currentChatAreaName] : null;
    public IReadOnlyDictionary<string, ChatArea> ChatAreas => chatAreas;

    public System.Action<string> OnChatAreaChanged { get; set; }
    public System.Action<string> OnChatAreaSwitchBlocked { get; set; }

    public void Initialize(ChatUI parentChatUI = null, ChatAreaUnlockManager unlockManager = null, ChatAreaButtonManager buttonManager = null)
    {
        this.chatUI = parentChatUI;
        this.unlockManager = unlockManager;
        this.buttonManager = buttonManager;

        FindAndInitializeChatAreas();
        SubscribeToEvents();
        ActivateFirstAvailableChatArea();
    }

    public void Cleanup()
    {
        UnsubscribeFromEvents();
        chatAreas.Clear();
    }

    private void FindAndInitializeChatAreas()
    {
        chatAreas.Clear();

        if (chatAreasParent == null)
        {
            Debug.LogError("ChatAreasParent is not assigned!");
            return;
        }

        foreach (Transform child in chatAreasParent)
        {
            if (child.name == "background") continue;

            ChatArea area = child.GetComponent<ChatArea>();
            if (area != null)
            {
                chatAreas[area.AreaName] = area;
                area.Initialize();

                buttonManager?.UpdateButtonState(area.AreaName);
            }
        }

        if (chatAreas.Count == 0)
        {
            Debug.LogError("No chat areas found during initialization!");
        }
    }

    private void SubscribeToEvents()
    {
        ChatAreaEvents.OnChoicePanelStateChanged += OnChoicePanelStateChanged;
        ChatAreaEvents.OnChatAreaUnlocked += OnChatAreaUnlocked;
    }

    private void UnsubscribeFromEvents()
    {
        ChatAreaEvents.OnChoicePanelStateChanged -= OnChoicePanelStateChanged;
        ChatAreaEvents.OnChatAreaUnlocked -= OnChatAreaUnlocked;
    }

    private void ActivateFirstAvailableChatArea()
    {
        foreach (var area in chatAreas)
        {
            if (IsChatAreaAccessible(area.Key))
            {
                SwitchToChatAreaInternal(area.Key);
                break;
            }
        }
    }

    public void SwitchToChatArea(string areaName)
    {
        if (!CanSwitchChatArea(areaName))
        {
            OnChatAreaSwitchBlocked?.Invoke(areaName);
            return;
        }

        SwitchToChatAreaInternal(areaName);
    }

    public void SwitchToChatAreaViaButton(string areaName)
    {
        SwitchToChatArea(areaName);
    }

    private bool CanSwitchChatArea(string areaName)
    {
        if (choicePanelIsOpen)
        {
            Debug.LogWarning("Cannot switch chat areas while choice panel is open");
            return false;
        }

        if (!chatAreas.ContainsKey(areaName))
        {
            Debug.LogWarning($"Chat area not found: {areaName}");
            return false;
        }

        if (!IsChatAreaAccessible(areaName))
        {
            Debug.LogWarning($"Chat area is not accessible: {areaName}");
            return false;
        }

        return true;
    }

    private bool IsChatAreaAccessible(string areaName)
    {
        if (unlockManager == null) return true;

        return unlockManager.IsChatAreaUnlocked(areaName);
    }

    private void SwitchToChatAreaInternal(string areaName)
    {
        if (!string.IsNullOrEmpty(currentChatAreaName) && chatAreas.ContainsKey(currentChatAreaName))
        {
            chatAreas[currentChatAreaName].SetAsInactive();
        }

        currentChatAreaName = areaName;
        chatAreas[areaName].SetAsActive();

        ChatNotificationEvents.TriggerChatAreaViewed(areaName);
        OnChatAreaChanged?.Invoke(areaName);
        ChatAreaEvents.TriggerChatAreaChanged(areaName);

        chatUI?.ScrollToBottom();
    }

    private void OnChoicePanelStateChanged(bool isOpen)
    {
        choicePanelIsOpen = isOpen;
    }

    private void OnChatAreaUnlocked(string areaName)
    {
        if (chatAreas.ContainsKey(areaName))
        {
            if (string.IsNullOrEmpty(currentChatAreaName))
            {
                SwitchToChatArea(areaName);
            }

            buttonManager?.UpdateButtonState(areaName);
        }
    }

    public bool IsAreaActive(string areaName)
    {
        return currentChatAreaName == areaName;
    }

    public bool IsAreaAccessible(string areaName)
    {
        return IsChatAreaAccessible(areaName);
    }

    public bool HasAccessibleAreas()
    {
        foreach (var areaName in chatAreas.Keys)
        {
            if (IsChatAreaAccessible(areaName))
                return true;
        }
        return false;
    }

    public Transform GetCurrentContent() => CurrentChatArea?.Content;
    public ScrollRect GetCurrentScrollRect() => CurrentChatArea?.ScrollRect;

    public void AdjustCurrentChatAreaForChoices(bool showChoices)
    {
        CurrentChatArea?.AdjustForChoices(showChoices);
    }

    public Transform GetContentForArea(string areaName)
    {
        return chatAreas.ContainsKey(areaName) ? chatAreas[areaName].Content : null;
    }

    public ChatArea GetChatArea(string areaName)
    {
        return chatAreas.ContainsKey(areaName) ? chatAreas[areaName] : null;
    }

    public void UpdateDependencies(ChatUI newChatUI = null, ChatAreaUnlockManager newUnlockManager = null, ChatAreaButtonManager newButtonManager = null)
    {
        if (newChatUI != null) chatUI = newChatUI;
        if (newUnlockManager != null) unlockManager = newUnlockManager;
        if (newButtonManager != null) buttonManager = newButtonManager;

        if (buttonManager != null)
        {
            foreach (var areaName in chatAreas.Keys)
            {
                buttonManager.UpdateButtonState(areaName);
            }
        }
    }
}