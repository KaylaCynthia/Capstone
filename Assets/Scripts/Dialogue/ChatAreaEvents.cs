public static class ChatAreaEvents
{
    public static event System.Action<string> OnChatAreaChanged;
    public static event System.Action<bool> OnChoicePanelStateChanged;

    public static void TriggerChatAreaChanged(string newAreaName)
    {
        OnChatAreaChanged?.Invoke(newAreaName);
    }

    public static void TriggerChoicePanelStateChanged(bool isOpen)
    {
        OnChoicePanelStateChanged?.Invoke(isOpen);
    }
}