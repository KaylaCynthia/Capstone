using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserMessage : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI messageText;

    public void SetMessage(string message, string userName)
    {
        messageText.text = message;

        if (nameText != null && !string.IsNullOrEmpty(userName))
        {
            nameText.text = userName;
        }
    }
}