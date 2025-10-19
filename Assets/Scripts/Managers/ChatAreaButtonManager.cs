using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatAreaButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class ChatAreaButton
    {
        public string chatAreaName;
        public Button button;
        public TextMeshProUGUI buttonText;
        public Transform buttonTransform;
        public bool hasNotification;
        public bool isDMArea;
    }

    [Header("Button Containers")]
    [SerializeField] private Transform dmsButtonsParent;
    [SerializeField] private Transform channelsButtonsParent;

    [Header("Button Lists")]
    [SerializeField] private List<ChatAreaButton> dmButtons = new List<ChatAreaButton>();
    [SerializeField] private List<ChatAreaButton> channelButtons = new List<ChatAreaButton>();

    private Dictionary<string, ChatAreaButton> buttonMap = new Dictionary<string, ChatAreaButton>();
    private Color normalColor = new Color(1f, 0.882353f, 0.7294118f, 1f);
    private Color notificationColor = Color.red;
    private Color lockedColor = Color.gray;

    private ChatAreaManager chatAreaManager;
    private ChatAreaUnlockManager unlockManager;
    private ServerManager serverManager;

    private static ChatAreaButtonManager instance;
    public static ChatAreaButtonManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Initialize(ChatAreaManager areaManager, ChatAreaUnlockManager unlockMgr, ServerManager serverMgr)
    {
        this.chatAreaManager = areaManager;
        this.unlockManager = unlockMgr;
        this.serverManager = serverMgr;

        InitializeButtonMap();
        SubscribeToEvents();
        UpdateAllButtonStates();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void InitializeButtonMap()
    {
        buttonMap.Clear();

        foreach (var buttonInfo in dmButtons)
        {
            buttonInfo.isDMArea = true;
            buttonMap[buttonInfo.chatAreaName] = buttonInfo;

            if (buttonInfo.button != null)
            {
                buttonInfo.button.onClick.AddListener(() =>
                    chatAreaManager.SwitchToChatAreaViaButton(buttonInfo.chatAreaName));
            }
        }

        foreach (var buttonInfo in channelButtons)
        {
            buttonInfo.isDMArea = false;
            buttonMap[buttonInfo.chatAreaName] = buttonInfo;

            if (buttonInfo.button != null)
            {
                buttonInfo.button.onClick.AddListener(() =>
                    chatAreaManager.SwitchToChatAreaViaButton(buttonInfo.chatAreaName));
            }
        }
    }

    private void SubscribeToEvents()
    {
        ChatNotificationEvents.OnNewMessageInInactiveArea += OnNewMessage;
        ChatNotificationEvents.OnChatAreaViewed += OnChatAreaViewed;
        ChatAreaEvents.OnChatAreaUnlocked += OnChatAreaUnlocked;
        ServerEvents.OnServerChanged += OnServerChanged;
    }

    private void UnsubscribeFromEvents()
    {
        ChatNotificationEvents.OnNewMessageInInactiveArea -= OnNewMessage;
        ChatNotificationEvents.OnChatAreaViewed -= OnChatAreaViewed;
        ChatAreaEvents.OnChatAreaUnlocked -= OnChatAreaUnlocked;
        ServerEvents.OnServerChanged -= OnServerChanged;
    }

    private void OnNewMessage(string chatAreaName)
    {
        if (buttonMap.ContainsKey(chatAreaName))
        {
            buttonMap[chatAreaName].hasNotification = true;
            UpdateButtonState(chatAreaName);
            ReorderButtons();
        }
    }

    private void OnChatAreaViewed(string chatAreaName)
    {
        if (buttonMap.ContainsKey(chatAreaName))
        {
            buttonMap[chatAreaName].hasNotification = false;
            UpdateButtonState(chatAreaName);
            ReorderButtons();
        }
    }

    private void OnChatAreaUnlocked(string chatAreaName)
    {
        if (buttonMap.ContainsKey(chatAreaName))
        {
            UpdateButtonState(chatAreaName);
            ReorderButtons();
        }
    }

    private void OnServerChanged(string serverType)
    {
        UpdateButtonsVisibility();
    }

    public void UpdateButtonState(string chatAreaName)
    {
        if (!buttonMap.ContainsKey(chatAreaName)) return;

        var buttonInfo = buttonMap[chatAreaName];
        bool isAccessible = chatAreaManager?.IsAreaAccessible(chatAreaName) ?? true;

        if (buttonInfo.button != null)
        {
            buttonInfo.button.interactable = isAccessible;
        }

        if (buttonInfo.buttonText != null)
        {
            if (!isAccessible)
            {
                buttonInfo.buttonText.color = lockedColor;
                buttonInfo.buttonText.text = $"{GetDisplayName(chatAreaName)} (Locked)";
            }
            else if (buttonInfo.hasNotification)
            {
                buttonInfo.buttonText.color = notificationColor;
                buttonInfo.buttonText.text = $"{GetDisplayName(chatAreaName)} ●";
            }
            else
            {
                buttonInfo.buttonText.color = normalColor;
                buttonInfo.buttonText.text = GetDisplayName(chatAreaName);
            }
        }
    }

    private string GetDisplayName(string chatAreaName)
    {
        if (chatAreaName.StartsWith("ChatArea"))
        {
            return chatAreaName.Substring("ChatArea".Length);
        }
        return chatAreaName;
    }

    private void UpdateButtonsVisibility()
    {
        bool isDMsActive = serverManager?.IsDMsServerActive() ?? true;

        if (dmsButtonsParent != null)
        {
            dmsButtonsParent.gameObject.SetActive(isDMsActive);
        }

        if (channelsButtonsParent != null)
        {
            channelsButtonsParent.gameObject.SetActive(!isDMsActive);
        }

        ReorderButtons();
    }

    private void ReorderButtons()
    {
        bool isDMsActive = serverManager?.IsDMsServerActive() ?? true;
        Transform activeParent = isDMsActive ? dmsButtonsParent : channelsButtonsParent;
        List<ChatAreaButton> activeButtons = isDMsActive ? dmButtons : channelButtons;

        if (activeParent == null) return;

        List<ChatAreaButton> sortedButtons = new List<ChatAreaButton>();

        foreach (var buttonInfo in activeButtons)
        {
            if (buttonInfo.hasNotification && IsButtonAccessible(buttonInfo.chatAreaName))
            {
                sortedButtons.Add(buttonInfo);
            }
        }

        foreach (var buttonInfo in activeButtons)
        {
            if (!buttonInfo.hasNotification && IsButtonAccessible(buttonInfo.chatAreaName))
            {
                sortedButtons.Add(buttonInfo);
            }
        }

        foreach (var buttonInfo in activeButtons)
        {
            if (!IsButtonAccessible(buttonInfo.chatAreaName))
            {
                sortedButtons.Add(buttonInfo);
            }
        }

        for (int i = 0; i < sortedButtons.Count; i++)
        {
            if (sortedButtons[i].buttonTransform != null)
            {
                sortedButtons[i].buttonTransform.SetParent(activeParent, false);
                sortedButtons[i].buttonTransform.SetSiblingIndex(i);
            }
        }
    }

    private bool IsButtonAccessible(string chatAreaName)
    {
        return chatAreaManager?.IsAreaAccessible(chatAreaName) ?? true;
    }

    public void UpdateAllButtonStates()
    {
        foreach (var buttonInfo in buttonMap.Values)
        {
            UpdateButtonState(buttonInfo.chatAreaName);
        }
        UpdateButtonsVisibility();
    }

    public void RegisterChatAreaButton(string chatAreaName, Button button, TextMeshProUGUI buttonText, Transform buttonTransform, bool isDMArea)
    {
        var newButton = new ChatAreaButton
        {
            chatAreaName = chatAreaName,
            button = button,
            buttonText = buttonText,
            buttonTransform = buttonTransform,
            hasNotification = false,
            isDMArea = isDMArea
        };

        if (isDMArea)
        {
            dmButtons.Add(newButton);
        }
        else
        {
            channelButtons.Add(newButton);
        }

        buttonMap[chatAreaName] = newButton;
        UpdateButtonState(chatAreaName);
        ReorderButtons();
    }

    public bool HasNotification(string chatAreaName)
    {
        return buttonMap.ContainsKey(chatAreaName) && buttonMap[chatAreaName].hasNotification;
    }

    public List<string> GetButtonsWithNotifications()
    {
        List<string> buttonsWithNotifications = new List<string>();
        foreach (var kvp in buttonMap)
        {
            if (kvp.Value.hasNotification)
            {
                buttonsWithNotifications.Add(kvp.Key);
            }
        }
        return buttonsWithNotifications;
    }
}