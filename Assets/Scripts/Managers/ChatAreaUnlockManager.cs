using System.Collections.Generic;
using UnityEngine;

public class ChatAreaUnlockManager : MonoBehaviour
{
    [System.Serializable]
    public class ChatAreaUnlockState
    {
        public string chatAreaName;
        public bool isUnlocked;
        public bool isDMArea;
    }

    [Header("Initial Unlock States")]
    [SerializeField] private List<ChatAreaUnlockState> initialUnlockStates = new List<ChatAreaUnlockState>();

    private Dictionary<string, bool> unlockMap = new Dictionary<string, bool>();
    private Dictionary<string, bool> dmAreaMap = new Dictionary<string, bool>();

    private static ChatAreaUnlockManager instance;
    public static ChatAreaUnlockManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeUnlockMap();
    }

    private void InitializeUnlockMap()
    {
        unlockMap.Clear();
        dmAreaMap.Clear();

        foreach (var state in initialUnlockStates)
        {
            unlockMap[state.chatAreaName] = state.isUnlocked;
            dmAreaMap[state.chatAreaName] = state.isDMArea;
        }

        InitializeDefaultAreas();
    }

    private void InitializeDefaultAreas()
    {
        string[] dmAreas = { "ChatAreaSunny", "ChatAreaRael" };
        foreach (string dmArea in dmAreas)
        {
            if (!unlockMap.ContainsKey(dmArea))
            {
                unlockMap[dmArea] = false;
                dmAreaMap[dmArea] = true;
            }
        }

        string[] serverAreas = { "ChatAreaEntrance", "ChatAreaLounge", "ChatAreaDaily" };
        foreach (string serverArea in serverAreas)
        {
            if (!unlockMap.ContainsKey(serverArea))
            {
                unlockMap[serverArea] = true;
                dmAreaMap[serverArea] = false;
            }
        }
    }

    public void UnlockChatArea(string chatAreaName)
    {
        if (unlockMap.ContainsKey(chatAreaName))
        {
            unlockMap[chatAreaName] = true;
        }
        else
        {
            bool isDM = IsDMArea(chatAreaName);
            unlockMap[chatAreaName] = true;
            dmAreaMap[chatAreaName] = isDM;

            initialUnlockStates.Add(new ChatAreaUnlockState
            {
                chatAreaName = chatAreaName,
                isUnlocked = true,
                isDMArea = isDM
            });
        }

        ChatAreaEvents.TriggerChatAreaUnlocked(chatAreaName);
        Debug.Log($"Chat area unlocked: {chatAreaName}");
    }

    public bool IsChatAreaUnlocked(string chatAreaName)
    {
        if (!IsDMArea(chatAreaName))
            return true;

        return unlockMap.ContainsKey(chatAreaName) && unlockMap[chatAreaName];
    }

    public bool IsDMArea(string chatAreaName)
    {
        return dmAreaMap.ContainsKey(chatAreaName) && dmAreaMap[chatAreaName];
    }

    public List<string> GetUnlockedChatAreas()
    {
        List<string> unlocked = new List<string>();
        foreach (var kvp in unlockMap)
        {
            if (kvp.Value)
                unlocked.Add(kvp.Key);
        }
        return unlocked;
    }

    public List<string> GetAllDMAreas()
    {
        List<string> dmAreas = new List<string>();
        foreach (var kvp in dmAreaMap)
        {
            if (kvp.Value)
                dmAreas.Add(kvp.Key);
        }
        return dmAreas;
    }

    public List<string> GetAllChannelAreas()
    {
        List<string> channelAreas = new List<string>();
        foreach (var kvp in dmAreaMap)
        {
            if (!kvp.Value)
                channelAreas.Add(kvp.Key);
        }
        return channelAreas;
    }
}