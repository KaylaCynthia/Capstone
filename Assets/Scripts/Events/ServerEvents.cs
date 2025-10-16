using System;

public static class ServerEvents
{
    public static event Action<string> OnServerChanged;

    public static void TriggerServerChanged(string serverType)
    {
        OnServerChanged?.Invoke(serverType);
    }
}