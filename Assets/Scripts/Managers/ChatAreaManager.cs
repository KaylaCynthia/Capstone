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

    public string CurrentChatAreaName => currentChatAreaName;
    public ChatArea CurrentChatArea => chatAreas.ContainsKey(currentChatAreaName) ? chatAreas[currentChatAreaName] : null;

    public void Initialize()
    {
        foreach (Transform child in chatAreasParent)
        {
            ChatArea area = child.GetComponent<ChatArea>();
            if (area != null)
            {
                chatAreas[area.AreaName] = area;
                area.Initialize();
            }
        }

        ChatAreaEvents.OnChoicePanelStateChanged += OnChoicePanelStateChanged;
    }

    public void Cleanup()
    {
        ChatAreaEvents.OnChoicePanelStateChanged -= OnChoicePanelStateChanged;
    }

    public void SwitchToChatArea(string areaName)
    {
        if (choicePanelIsOpen)
        {
            //Debug.LogWarning("Cannot switch chat areas while choice panel is open");
            return;
        }

        if (!string.IsNullOrEmpty(currentChatAreaName) && chatAreas.ContainsKey(currentChatAreaName))
        {
            chatAreas[currentChatAreaName].SetAsInactive();
        }

        if (chatAreas.ContainsKey(areaName))
        {
            currentChatAreaName = areaName;
            chatAreas[areaName].SetAsActive();
        }

        ChatAreaEvents.TriggerChatAreaChanged(areaName);
    }

    public void SwitchToChatAreaViaButton(string areaName)
    {
        SwitchToChatArea(areaName);
    }

    private void OnChoicePanelStateChanged(bool isOpen)
    {
        choicePanelIsOpen = isOpen;
        //Debug.Log($"Choice panel is now {(isOpen ? "open" : "closed")}. Chat area switching: {(isOpen ? "disabled" : "enabled")}");
    }

    public ChatArea GetChatArea(string areaName)
    {
        return chatAreas.ContainsKey(areaName) ? chatAreas[areaName] : null;
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

    public bool IsAreaActive(string areaName)
    {
        return currentChatAreaName == areaName;
    }
}