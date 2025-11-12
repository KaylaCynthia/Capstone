using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SleepAppUI : BaseAppUI
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI sleepBenefitsText;
    [SerializeField] private Button sleepButton;
    [SerializeField] private TextMeshProUGUI currentDayText;

    [Header("Sleep Effects")]
    [SerializeField]
    private ActionEffect sleepEffect = new ActionEffect
    {
        actionName = "Sleep",
        healthChange = 20f,
        stressChange = -20f,
        cashChange = 0
    };

    protected override void OnEnable()
    {
        base.OnEnable();

        if (sleepButton != null)
            sleepButton.onClick.AddListener(PerformSleep);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (sleepButton != null)
            sleepButton.onClick.RemoveListener(PerformSleep);
    }

    protected override void RefreshUI()
    {
        UpdateSleepBenefitsText();
        UpdateCurrentDayText();
        UpdateButtonState();
    }

    private void UpdateSleepBenefitsText()
    {
        if (sleepBenefitsText != null)
        {
            sleepBenefitsText.text = $"Sleep Benefits:\n" +
                                    $"+{sleepEffect.healthChange}% Health\n" +
                                    $"{sleepEffect.stressChange}% Stress";
        }
    }

    private void UpdateCurrentDayText()
    {
        if (currentDayText != null)
        {
            int currentDay = DayManager.GetInstance().GetCurrentDay();
            currentDayText.text = $"Current Day: {currentDay}";
        }
    }

    private void UpdateButtonState()
    {
        if (sleepButton != null)
            sleepButton.interactable = true;
    }

    private void PerformSleep()
    {
        bool success = StatsManager.GetInstance().PerformAction(sleepEffect);
        if (success)
        {
            Debug.Log("Sleep completed! Stats updated.");
            AppSystemManager.GetInstance().ReturnToHomeScreen();
        }
    }

    protected override void OnStatsChanged(PlayerStats stats)
    {
        // Update UI if needed when stats change
    }
}