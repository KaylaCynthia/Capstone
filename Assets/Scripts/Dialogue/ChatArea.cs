using UnityEngine;
using UnityEngine.UI;

public enum ChatAreaType { DM, Server }

public class ChatArea : MonoBehaviour
{
    [SerializeField] private string areaName;
    [SerializeField] private ChatAreaType areaType;
    [SerializeField] private Transform content;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform chatAreaRect;

    private float originalChatAreaHeight;
    private const float CHOICE_PANEL_HEIGHT = 150f;

    public string AreaName => areaName;
    public ChatAreaType AreaType => areaType;
    public Transform Content => content;
    public ScrollRect ScrollRect => scrollRect;

    private void Awake()
    {
        if (chatAreaRect != null)
            originalChatAreaHeight = chatAreaRect.rect.height;
    }

    public void Initialize()
    {
        SetAsInactive();
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void Clear()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);
    }

    public void SetAsActive()
    {
        Show();

        if (scrollRect != null)
        {
            scrollRect.enabled = true;
        }

        transform.SetAsLastSibling();
    }

    public void SetAsInactive()
    {
        if (scrollRect != null)
        {
            scrollRect.enabled = false;
        }
    }

    public void AdjustForChoices(bool showChoices)
    {
        if (chatAreaRect != null)
        {
            float targetHeight = showChoices ? originalChatAreaHeight - CHOICE_PANEL_HEIGHT : originalChatAreaHeight;
            chatAreaRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
        }
    }
}