using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatDialogueManager : MonoBehaviour
{
    [Header("Prefab Reference")]
    [SerializeField] private GameObject chatMessagePrefab;

    [Header("UI References")]
    [SerializeField] private ChatUI chatUI;
    [SerializeField] private ChoiceHandler choiceHandler;

    [Header("Dialogue Parameters")]
    [SerializeField] private float lettersPerSecond;
    [SerializeField] private InkFileManager inkFileManager;

    private Story currentStory;
    public bool DialogueIsPlaying { get; private set; }

    private DialogueState currentState;
    private DialogueParser dialogueParser;
    private Dictionary<string, GameObject> lastMessageFromUser;
    private string currentConversationChatArea;

    private static ChatDialogueManager instance;
    public static ChatDialogueManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one ChatDialogueManager");
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (chatMessagePrefab != null)
        {
            ChatMessage.SetMessagePrefab(chatMessagePrefab);
        }
        else
        {
            Debug.LogError("ChatMessagePrefab is not assigned in inspector!");
        }

        dialogueParser = new DialogueParser();
        currentState = new DialogueState();
        lastMessageFromUser = new Dictionary<string, GameObject>();
        Initialize();
        chatUI.SwitchToChatArea("ChatAreaDiluc");
    }

    private void Initialize()
    {
        DialogueIsPlaying = false;
        chatUI.Initialize();
        choiceHandler.Initialize(MakeChoice, chatUI);
        lastMessageFromUser.Clear();
    }

    public void StartConversation(string conversationKey, string chatAreaName)
    {
        //Debug.Log($"Starting conversation: {conversationKey} in chat area: {chatAreaName}");
        if (inkFileManager == null)
        {
            Debug.LogError("InkFileManager is not assigned!");
            return;
        }

        currentConversationChatArea = chatAreaName;

        inkFileManager.StartConversation(conversationKey);
        TextAsset inkFile = inkFileManager.GetCurrentInkFile();

        if (inkFile != null)
        {
            //Debug.Log($"Starting conversation with Ink file: {inkFile.name}. Messages will go to: {currentConversationChatArea}");
            EnterDialogueMode(inkFile);
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        chatUI.Show();
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (!currentStory.canContinue)
        {
            Debug.Log("End of Conversation reached.");
            StartCoroutine(HandleEndOfConversation());
            return;
        }

        string nextLine = currentStory.Continue();

        dialogueParser.HandleTags(currentStory.currentTags, currentState);

        bool isPlayerMessage = currentState.IsPlayerSpeaking;
        StartCoroutine(DisplayMessage(nextLine, isPlayerMessage));
    }

    private IEnumerator HandleEndOfConversation()
    {
        string nextChatArea = DetermineNextChatArea();
        string nextBranch = DetermineNextBranch();

        //Debug.Log($"Conversation ended. Next area: {nextChatArea}, Next branch: {nextBranch}");

        if (inkFileManager.IsDayTransitionBranch(nextBranch))
        {
            yield return StartCoroutine(HandleDayTransition());

            if (DialogueIsPlaying)
            {
                //Debug.Log("New conversation started after day transition, exiting HandleEndOfConversation");
                yield break;
            }
        }
        else if (!string.IsNullOrEmpty(nextBranch))
        {
            inkFileManager.MoveToNextBranch(nextBranch);
            TextAsset nextInkFile = inkFileManager.GetCurrentInkFile();

            if (nextInkFile != null)
            {
                currentConversationChatArea = nextChatArea ?? currentConversationChatArea;
                EnterDialogueMode(nextInkFile);
                yield break;
            }
        }

        //Debug.Log("No next branch found, exiting dialogue mode");
        StartCoroutine(ExitDialogueMode());
    }

    private IEnumerator HandleDayTransition()
    {
        lastMessageFromUser.Clear();

        DayManager dayManager = DayManager.GetInstance();
        if (dayManager != null)
        {
            dayManager.StartNextDay();

            yield return new WaitForSeconds(dayManager.TransitionDuration + 0.5f);

            string nextChatArea = DetermineNextChatArea();
            string nextBranch = DetermineNextBranchAfterDayTransition();

            //Debug.Log(nextChatArea + ", " + nextBranch);

            if (!string.IsNullOrEmpty(nextBranch))
            {
                //Debug.Log("Moving to next branch after day transition: " + nextBranch);
                inkFileManager.MoveToNextBranch(nextBranch);
                TextAsset nextInkFile = inkFileManager.GetCurrentInkFile();

                if (nextInkFile != null)
                {
                    currentConversationChatArea = nextChatArea ?? currentConversationChatArea;
                    EnterDialogueMode(nextInkFile);
                    yield break;
                }
            }
        }

        StartCoroutine(ExitDialogueMode());
    }

    private string DetermineNextBranchAfterDayTransition()
    {
        int currentDay = DayManager.GetInstance().GetCurrentDay();

        switch (currentDay)
        {
            case 2: return "diluc_morning1";
            case 3: return "day3_morning";
            default: return "default_morning";
        }
    }

    private string DetermineNextChatArea()
    {
        if (currentStory.variablesState.GlobalVariableExistsWithName("nextChatArea"))
        {
            object nextChatAreaObj = currentStory.variablesState["nextChatArea"];
            return nextChatAreaObj?.ToString();
        }
        return null;
    }

    public void SwitchToChatArea(string areaName)
    {
        chatUI.SwitchToChatArea(areaName);
    }

    private string DetermineNextBranch()
    {
        if (currentStory.variablesState.GlobalVariableExistsWithName("nextBranch"))
        {
            object nextBranchObj = currentStory.variablesState["nextBranch"];
            return nextBranchObj?.ToString();
        }

        List<string> possibleBranches = inkFileManager.GetNextPossibleBranches();

        if (possibleBranches.Count == 1)
        {
            return possibleBranches[0];
        }
        else if (possibleBranches.Count > 1)
        {
            return possibleBranches[0];
        }

        return null;
    }

    private IEnumerator DisplayMessage(string message, bool isPlayerMessage)
    {
        currentState.SetWaitingForInput(false);

        if (!isPlayerMessage)
        {
            if (chatUI.GetCurrentChatAreaName() == currentConversationChatArea)
            {
                yield return StartCoroutine(chatUI.ShowTypingForDuration(message.Length / lettersPerSecond, currentConversationChatArea));
            }
            else
            {
                yield return new WaitForSeconds(message.Length / lettersPerSecond);
            }
        }

        string currentSpeaker = currentState.CurrentSpeaker;
        GameObject messageUI;

        bool shouldAppend = lastMessageFromUser.ContainsKey(currentSpeaker) && !isPlayerMessage;

        if (shouldAppend)
        {
            messageUI = lastMessageFromUser[currentSpeaker];
            chatUI.AppendToMessage(messageUI, message);
        }
        else
        {
            ChatMessage chatMessage = ChatMessageFactory.Create(message, currentState);

            messageUI = chatUI.AddMessageToChat(chatMessage, currentConversationChatArea);

            if (!isPlayerMessage)
            {
                lastMessageFromUser[currentSpeaker] = messageUI;
            }
            else
            {
                lastMessageFromUser.Remove("You");
                lastMessageFromUser.Remove(currentSpeaker);
            }
        }

        if (chatUI.GetCurrentChatAreaName() == currentConversationChatArea || isPlayerMessage)
        {
            chatUI.ScrollToBottom();
        }

        currentState.SetWaitingForInput(true);

        if (currentStory.currentChoices.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);

            if (chatUI.GetCurrentChatAreaName() == currentConversationChatArea)
            {
                choiceHandler.ShowChoices(currentStory.currentChoices, currentConversationChatArea);
            }
            else
            {
                //Debug.Log($"Choices available in {currentConversationChatArea}, but player is in {chatUI.GetCurrentChatAreaName()}. Waiting for player to switch...");
                yield return WaitForPlayerToSwitchToCorrectArea();
            }
        }
        else
        {
            choiceHandler.Hide();
            yield return new WaitForSeconds(1f);
            ContinueStory();
        }
    }

    private IEnumerator WaitForPlayerToSwitchToCorrectArea()
    {
        while (chatUI.GetCurrentChatAreaName() != currentConversationChatArea)
        {
            yield return null;

            if (!DialogueIsPlaying)
            {
                yield break;
            }
        }

        choiceHandler.ShowChoices(currentStory.currentChoices, currentConversationChatArea);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (currentStory.currentChoices.Count > choiceIndex)
        {
            string choiceText = currentStory.currentChoices[choiceIndex].text;
            DisplayPlayerChoice(choiceText);
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    private void DisplayPlayerChoice(string choiceText)
    {
        ChatMessage playerMessage = new PlayerMessage(choiceText, null);
        GameObject messageUI = chatUI.AddMessageToChat(playerMessage);

        if (messageUI != null)
        {
            ChatMessageUI chatMessageUI = messageUI.GetComponentInChildren<ChatMessageUI>();
            if (chatMessageUI != null)
            {
                chatMessageUI.SetMessageText(choiceText);
            }
        }

        chatUI.ScrollToBottom();
        lastMessageFromUser.Clear();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        DialogueIsPlaying = false;
        chatUI.Hide();
        choiceHandler.Hide();
        lastMessageFromUser.Clear();
    }
}