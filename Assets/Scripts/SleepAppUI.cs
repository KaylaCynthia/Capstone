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
            sleepButton.onClick.AddListener(StartSleep);

        UpdateUI();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (sleepButton != null)
            sleepButton.onClick.RemoveListener(StartSleep);
    }

    private void UpdateUI()
    {
        if (sleepBenefitsText != null)
        {
            sleepBenefitsText.text = $"Sleep Benefits:\n" +
                                    $"+{sleepEffect.healthChange}% Health\n" +
                                    $"{sleepEffect.stressChange}% Stress\n" +
                                    "Progress to Next Day";
        }

        if (currentDayText != null)
        {
            int currentDay = DayManager.GetInstance().GetCurrentDay();
            currentDayText.text = $"Current Day: {currentDay}";
        }

        if (sleepButton != null)
            sleepButton.interactable = true;
    }

    private void StartSleep()
    {
        StatsManager.GetInstance().PerformAction(sleepEffect);

        DayManager dayManager = DayManager.GetInstance();
        if (dayManager != null)
        {
            dayManager.StartNextDay();

            AppSystemManager.GetInstance().ReturnToHomeScreen();
        }

        Debug.Log("Going to sleep... Starting next day!");
    }
}