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
    private bool isMakingChoice = false;

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
    }

    private void Initialize()
    {
        DialogueIsPlaying = false;
        chatUI.Initialize();
        choiceHandler.Initialize(MakeChoice);
        lastMessageFromUser.Clear();
    }

    public void StartConversation(string conversationKey)
    {
        if (inkFileManager == null)
        {
            Debug.LogError("InkFileManager is not assigned!");
            return;
        }

        inkFileManager.StartConversation(conversationKey);
        TextAsset inkFile = inkFileManager.GetCurrentInkFile();

        if (inkFile != null)
        {
            EnterDialogueMode(inkFile);
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        chatUI.Show();
        currentState.Reset();
        lastMessageFromUser.Clear();
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (!currentStory.canContinue)
        {
            HandleEndOfConversation();
            return;
        }

        string nextLine = currentStory.Continue();

        dialogueParser.HandleTags(currentStory.currentTags, currentState);

        bool isPlayerMessage = currentState.IsPlayerSpeaking;
        StartCoroutine(DisplayMessage(nextLine, isPlayerMessage));
    }

    private void HandleEndOfConversation()
    {
        string nextBranch = DetermineNextBranch();

        if (!string.IsNullOrEmpty(nextBranch))
        {
            inkFileManager.MoveToNextBranch(nextBranch);
            TextAsset nextInkFile = inkFileManager.GetCurrentInkFile();

            if (nextInkFile != null)
            {
                EnterDialogueMode(nextInkFile);
                return;
            }
        }

        Debug.Log("End of conversation reached.");
        StartCoroutine(ExitDialogueMode());
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
            yield return StartCoroutine(chatUI.ShowTypingForDuration(message.Length / lettersPerSecond));
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
            messageUI = chatUI.AddMessageToChat(chatMessage);

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

        chatUI.ScrollToBottom();
        currentState.SetWaitingForInput(true);

        if (currentStory.currentChoices.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
            choiceHandler.ShowChoices(currentStory.currentChoices);
        }
        else if (currentStory.canContinue)
        {
            yield return new WaitForSeconds(1f);
            ContinueStory();
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (currentStory.currentChoices.Count > choiceIndex)
        {
            string choiceText = currentStory.currentChoices[choiceIndex].text;
            DisplayPlayerChoice(choiceText);
            currentStory.ChooseChoiceIndex(choiceIndex);
            choiceHandler.Hide();
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

        GameManager.GetInstance()?.OnConversationComplete();
    }
}