using System;

public static class ChatNotificationEvents
{
    public static event Action<string> OnNewMessageInInactiveArea;
    public static event Action<string> OnChatAreaViewed;

    public static void TriggerNewMessage(string chatAreaName)
    {
        OnNewMessageInInactiveArea?.Invoke(chatAreaName);
    }

    public static void TriggerChatAreaViewed(string chatAreaName)
    {
        OnChatAreaViewed?.Invoke(chatAreaName);
    }
}