using UnityEngine;

public abstract class ChatMessage
{
    public abstract string Speaker { get; }
    public abstract string Message { get; }
    public abstract string AnimationStateName { get; }
    public abstract bool IsPlayer { get; }

    private static GameObject messagePrefab;

    public static void SetMessagePrefab(GameObject prefab)
    {
        messagePrefab = prefab;
    }

    public virtual GameObject CreateUIElement(Transform parent)
    {
        if (messagePrefab == null)
        {
            Debug.LogError("Message prefab not set! Call ChatMessage.SetMessagePrefab() first.");
            return null;
        }

        GameObject messageObj = Object.Instantiate(messagePrefab, parent);
        return messageObj;
    }
}

public class PlayerMessage : ChatMessage
{
    public PlayerMessage(string message, string animationStateName)
    {
        this.message = message;
        this.animationStateName = animationStateName;
    }

    private string message;
    private string animationStateName;

    public override string Speaker => "You";
    public override string Message => message;
    public override string AnimationStateName => animationStateName;
    public override bool IsPlayer => true;
}

public class NPCMessage : ChatMessage
{
    public NPCMessage(string speaker, string message, string animationStateName)
    {
        this.speaker = speaker;
        this.message = message;
        this.animationStateName = animationStateName;
    }

    private string speaker;
    private string message;
    private string animationStateName;

    public override string Speaker => speaker;
    public override string Message => message;
    public override string AnimationStateName => animationStateName;
    public override bool IsPlayer => false;
}