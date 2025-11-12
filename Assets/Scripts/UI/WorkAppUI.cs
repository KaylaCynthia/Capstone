using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkAppUI : BaseAppUI
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI effectsText;
    [SerializeField] private Button performWorkButton;

    [Header("Work Effects")]
    [SerializeField]
    private ActionEffect workEffect = new ActionEffect
    {
        actionName = "Work",
        healthChange = -10f,
        stressChange = 10f,
        cashChange = 100
    };

    protected override void OnEnable()
    {
        base.OnEnable();

        if (performWorkButton != null)
            performWorkButton.onClick.AddListener(PerformWork);

        UpdateUI();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (performWorkButton != null)
            performWorkButton.onClick.RemoveListener(PerformWork);
    }

    private void UpdateUI()
    {
        if (effectsText != null)
        {
            effectsText.text = $"Work Effects:\n" +
                              $"{workEffect.healthChange}% Health\n" +
                              $"+{workEffect.stressChange}% Stress\n" +
                              $"+${workEffect.cashChange} Cash";
        }

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (performWorkButton != null)
        {
            PlayerStats stats = StatsManager.GetInstance().GetCurrentStats();
            bool isNight = TimeManager.GetInstance().CurrentTime == TimeManager.TimeOfDay.Night;
            bool hasActionsLeft = stats.actionsPerformedToday < stats.maxActionsPerDay;

            performWorkButton.interactable = hasActionsLeft && !isNight;
        }
    }

    private void PerformWork()
    {
        bool success = StatsManager.GetInstance().PerformAction(workEffect);
        if (success)
        {
            Debug.Log("Work completed!");
            AppSystemManager.GetInstance().ReturnToHomeScreen();
        }
    }

    protected override void OnStatsChanged(PlayerStats stats)
    {
        UpdateButtonState();
    }

    protected override void OnTimeChanged(TimeManager.TimeOfDay time)
    {
        UpdateButtonState();
    }
}