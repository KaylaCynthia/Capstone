using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private DayManager dayManager;
    [SerializeField] private InkFileManager inkFileManager;
    [SerializeField] private ChatDialogueManager chatDialogueManager;

    private static GameStateManager instance;
    public static GameStateManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public int GetCurrentDay() => dayManager.GetCurrentDay();

    public void StartConversationForCurrentDay(string chatArea = null)
    {
        if (GetCurrentDay() == 1)
        {
            ContinueFirstConversation();
        }
        else
        {
            string conversationKey = GetConversationKeyForDay(GetCurrentDay());
            string targetChatArea = chatArea ?? GetChatAreaForDay(GetCurrentDay());

            chatDialogueManager.StartConversation(conversationKey, targetChatArea);
        }
    }

    public void StartFirstConversation()
    {
        chatDialogueManager.StartConversation("sunny_intro", "ChatAreaSunny");
    }

    public void ContinueFirstConversation()
    {
        chatDialogueManager.StartConversation("sunny_intro2", "ChatAreaSunny");
    }

    private string GetConversationKeyForDay(int day)
    {
        switch (day)
        {
            case 1: return "sunny_intro";
            case 2: return "sunny_day2";
            case 3: return "sunny_day3";
            default: return "sunny_intro";
        }
    }

    private string GetChatAreaForDay(int day)
    {
        switch (day)
        {
            case 1: return "ChatAreaSunny";
            case 2: return "ChatAreaSunny";
            case 3: return "ChatAreaSunny";
            default: return "ChatAreaSunny";
        }
    }
}