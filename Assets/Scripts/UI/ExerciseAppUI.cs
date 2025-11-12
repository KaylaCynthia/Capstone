using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseAppUI : BaseAppUI
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI effectsText;
    [SerializeField] private Button performExerciseButton;

    [Header("Exercise Effects")]
    [SerializeField]
    private ActionEffect exerciseEffect = new ActionEffect
    {
        actionName = "Exercise",
        healthChange = 10f,
        stressChange = -10f,
        cashChange = 0
    };

    protected override void OnEnable()
    {
        base.OnEnable();

        if (performExerciseButton != null)
            performExerciseButton.onClick.AddListener(PerformExercise);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (performExerciseButton != null)
            performExerciseButton.onClick.RemoveListener(PerformExercise);
    }

    protected override void RefreshUI()
    {
        UpdateEffectsText();
        UpdateButtonState();
    }

    private void UpdateEffectsText()
    {
        if (effectsText != null)
        {
            effectsText.text = $"Exercise Effects:\n" +
                              $"+{exerciseEffect.healthChange}% Health\n" +
                              $"{exerciseEffect.stressChange}% Stress";
        }
    }

    private void UpdateButtonState()
    {
        if (performExerciseButton != null)
        {
            PlayerStats stats = StatsManager.GetInstance().GetCurrentStats();
            bool isNight = TimeManager.GetInstance().CurrentTime == TimeManager.TimeOfDay.Night;
            bool hasActionsLeft = stats.actionsPerformedToday < stats.maxActionsPerDay;

            performExerciseButton.interactable = hasActionsLeft && !isNight;
        }
    }

    private void PerformExercise()
    {
        bool success = StatsManager.GetInstance().PerformAction(exerciseEffect);
        if (success)
        {
            Debug.Log("Exercise completed!");
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