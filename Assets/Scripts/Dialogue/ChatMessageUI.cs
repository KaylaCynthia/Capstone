using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatMessageUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private RectTransform messageContainer;
    [SerializeField] private RectTransform chatMessageRect;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;

    [Header("Sizing")]
    [SerializeField] private float minHeight = 60f;
    [SerializeField] private float maxWidth = 600f;
    [SerializeField] private float spacingBetweenMessages = 10f;

    [Header("Animation")]
    [SerializeField] private string defaultAnimation = "portrait_default";

    private Animator portraitAnimator;
    private float originalMessageTextY;
    private float originalSpeakerTextY;

    private void Awake()
    {
        if (portraitImage != null)
        {
            portraitAnimator = portraitImage.GetComponent<Animator>();
            if (portraitAnimator == null)
            {
                Debug.LogWarning("No Animator component found on portraitImage GameObject");
            }
        }

        if (messageText != null)
        {
            originalMessageTextY = messageText.rectTransform.anchoredPosition.y;
        }
        if (speakerText != null)
        {
            originalSpeakerTextY = speakerText.rectTransform.anchoredPosition.y;
        }

        SetupAnchorsForDownwardExpansion();
    }

    private void SetupAnchorsForDownwardExpansion()
    {
        if (chatMessageRect != null)
        {
            chatMessageRect.pivot = new Vector2(0.5f, 1f);
            chatMessageRect.anchorMin = new Vector2(0.5f, 1f);
            chatMessageRect.anchorMax = new Vector2(0.5f, 1f);
        }

        if (messageText != null)
        {
            RectTransform textRect = messageText.rectTransform;
            textRect.pivot = new Vector2(0f, 1f);
            textRect.anchorMin = new Vector2(0f, 1f);
            textRect.anchorMax = new Vector2(0f, 1f);
        }
    }

    public void Initialize(ChatMessage message)
    {
        PlayPortraitAnimation(message.AnimationStateName);

        speakerText.text = message.Speaker;
        messageText.text = message.Message;

        if (message.IsPlayer)
        {
            SetPlayerMessageStyle();
        }
        else
        {
            SetNPCMessageStyle();
        }

        StartCoroutine(ResizeAfterLayout());
    }

    private void PlayPortraitAnimation(string animationStateName)
    {
        if (portraitAnimator == null) return;

        string animationToPlay = string.IsNullOrEmpty(animationStateName) ? defaultAnimation : animationStateName;

        if (HasAnimationState(animationToPlay))
        {
            portraitAnimator.Play(animationToPlay);
        }
        else
        {
            portraitAnimator.Play(defaultAnimation);
        }
    }

    private bool HasAnimationState(string stateName)
    {
        if (portraitAnimator == null || portraitAnimator.runtimeAnimatorController == null)
            return false;

        foreach (var clip in portraitAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == stateName)
                return true;
        }
        return false;
    }

    public void AppendMessage(string additionalText)
    {
        messageText.text += additionalText;
        StartCoroutine(ResizeAfterLayout());
    }

    public void SetMessageText(string text)
    {
        messageText.text = text;
        StartCoroutine(ResizeAfterLayout());
    }

    private System.Collections.IEnumerator ResizeAfterLayout()
    {
        messageText.ForceMeshUpdate();
        yield return new WaitForEndOfFrame();
        messageText.ForceMeshUpdate(true);
        ResizeMessageBubble();
    }

    private void ResizeMessageBubble()
    {
        if (messageText == null || chatMessageRect == null || speakerText == null) return;

        float messageHeight = messageText.textBounds.size.y;

        if (messageHeight <= 0.1f)
        {
            Vector2 messagePreferredSize = messageText.GetPreferredValues();
            messageHeight = messagePreferredSize.y;
        }

        Vector2 speakerPreferredSize = speakerText.GetPreferredValues(speakerText.text);
        float speakerHeight = speakerPreferredSize.y;
        float messageTextTopMargin = Mathf.Abs(originalMessageTextY);
        float totalHeight = speakerHeight + messageHeight + spacingBetweenMessages;

        messageText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, messageHeight);
        chatMessageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);

        if (messageContainer != null)
        {
            messageContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
        }

        LayoutElement layoutElement = chatMessageRect.GetComponent<LayoutElement>();
        if (layoutElement == null)
        {
            layoutElement = chatMessageRect.gameObject.AddComponent<LayoutElement>();
        }
        layoutElement.preferredHeight = totalHeight;
        layoutElement.minHeight = totalHeight;

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessageRect);

        if (chatMessageRect.parent != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessageRect.parent as RectTransform);
        }
    }

    private void SetPlayerMessageStyle()
    {
        if (layoutGroup != null)
        {
            layoutGroup.childAlignment = TextAnchor.UpperLeft;
        }

        if (chatMessageRect != null)
        {
            chatMessageRect.pivot = new Vector2(0f, 1f);
            chatMessageRect.anchorMin = new Vector2(0f, 1f);
            chatMessageRect.anchorMax = new Vector2(0f, 1f);
        }
    }

    private void SetNPCMessageStyle()
    {
        if (layoutGroup != null)
        {
            layoutGroup.childAlignment = TextAnchor.UpperLeft;
        }

        if (chatMessageRect != null)
        {
            chatMessageRect.pivot = new Vector2(0f, 1f);
            chatMessageRect.anchorMin = new Vector2(0f, 1f);
            chatMessageRect.anchorMax = new Vector2(0f, 1f);
        }
    }
}