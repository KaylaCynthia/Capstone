using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InkFileManager", menuName = "Dialogue/Ink File Manager")]
public class InkFileManager : ScriptableObject
{
    [System.Serializable]
    public class ConversationBranch
    {
        public string conversationKey;
        public TextAsset inkFile;
        public List<string> nextPossibleBranches;
    }

    [SerializeField] private List<ConversationBranch> conversationBranches;
    private string currentConversationKey;

    public void StartConversation(string conversationKey)
    {
        currentConversationKey = conversationKey;
    }

    public TextAsset GetCurrentInkFile()
    {
        return GetInkFileByKey(currentConversationKey);
    }

    public TextAsset GetInkFileByKey(string conversationKey)
    {
        ConversationBranch branch = conversationBranches.Find(b => b.conversationKey == conversationKey);
        return branch?.inkFile;
    }

    public List<string> GetNextPossibleBranches()
    {
        ConversationBranch currentBranch = conversationBranches.Find(b => b.conversationKey == currentConversationKey);
        return currentBranch?.nextPossibleBranches ?? new List<string>();
    }

    public void MoveToNextBranch(string nextBranchKey)
    {
        if (conversationBranches.Exists(b => b.conversationKey == nextBranchKey))
        {
            currentConversationKey = nextBranchKey;
        }
        else
        {
            Debug.LogError($"Conversation branch not found: {nextBranchKey}");
        }
    }

    public bool HasNextBranches()
    {
        return GetNextPossibleBranches().Count > 0;
    }
}