using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatNotificationManager : MonoBehaviour
{
    [System.Serializable]
    public class ChatAreaNotification
    {
        public string chatAreaName;
        public TextMeshProUGUI buttonText;
        public bool hasNewMessages;
    }

    [SerializeField] private List<ChatAreaNotification> chatAreaNotifications = new List<ChatAreaNotification>();

    private Dictionary<string, ChatAreaNotification> notificationMap = new Dictionary<string, ChatAreaNotification>();
    private Color normalColor = new Color(1f, 0.882353f, 0.7294118f, 1);
    private Color notificationColor = Color.red;

    private static ChatNotificationManager instance;
    public static ChatNotificationManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        InitializeNotificationMap();

        ChatNotificationEvents.OnNewMessageInInactiveArea += OnNewMessage;
        ChatNotificationEvents.OnChatAreaViewed += OnChatAreaSwitched;
        ServerEvents.OnServerChanged += OnServerChanged;
    }

    private void OnDestroy()
    {
        ChatNotificationEvents.OnNewMessageInInactiveArea -= OnNewMessage;
        ChatNotificationEvents.OnChatAreaViewed -= OnChatAreaSwitched;
        ServerEvents.OnServerChanged -= OnServerChanged;
    }

    private void InitializeNotificationMap()
    {
        notificationMap.Clear();
        foreach (var notification in chatAreaNotifications)
        {
            notificationMap[notification.chatAreaName] = notification;
            SetTextColor(notification.chatAreaName, normalColor);
        }
    }

    private void OnNewMessage(string chatAreaName)
    {
        if (notificationMap.ContainsKey(chatAreaName))
        {
            notificationMap[chatAreaName].hasNewMessages = true;

            if (IsChatAreaInActiveServer(chatAreaName))
            {
                SetTextColor(chatAreaName, notificationColor);
            }
        }
    }

    private void OnChatAreaSwitched(string chatAreaName)
    {
        if (notificationMap.ContainsKey(chatAreaName))
        {
            notificationMap[chatAreaName].hasNewMessages = false;
            SetTextColor(chatAreaName, normalColor);
        }
    }

    private void OnServerChanged(string serverType)
    {
        UpdateAllTextColors();
    }

    private void UpdateAllTextColors()
    {
        foreach (var notification in chatAreaNotifications)
        {
            Color targetColor = notification.hasNewMessages && IsChatAreaInActiveServer(notification.chatAreaName)
                ? notificationColor
                : normalColor;

            SetTextColor(notification.chatAreaName, targetColor);
        }
    }

    private void SetTextColor(string chatAreaName, Color color)
    {
        if (notificationMap.ContainsKey(chatAreaName))
        {
            TextMeshProUGUI text = notificationMap[chatAreaName].buttonText;
            if (text != null)
            {
                text.color = color;
            }
        }
    }

    private bool IsChatAreaInActiveServer(string chatAreaName)
    {
        string[] dmAreas = { "ChatAreaSunny", "ChatAreaZhongli", "ChatAreaNeuvilette" };

        ServerManager serverManager = ServerManager.GetInstance();
        if (serverManager == null) return true;

        if (serverManager.IsDMsServerActive())
        {
            foreach (string dmArea in dmAreas)
            {
                if (chatAreaName == dmArea) return true;
            }
            return false;
        }
        else
        {
            foreach (string dmArea in dmAreas)
            {
                if (chatAreaName == dmArea) return false;
            }
            return true;
        }
    }

    public bool HasNewMessages(string chatAreaName)
    {
        return notificationMap.ContainsKey(chatAreaName) && notificationMap[chatAreaName].hasNewMessages;
    }
}