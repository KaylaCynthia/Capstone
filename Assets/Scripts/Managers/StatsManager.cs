using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private PlayerStats currentStats = new PlayerStats();
    [SerializeField] private List<string> transactionHistory = new List<string>();

    private static StatsManager instance;
    public static StatsManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDefaultStats();
    }

    private void InitializeDefaultStats()
    {
        currentStats.health = 100f;
        currentStats.stress = 0f;
        currentStats.cash = 0;
        currentStats.actionsPerformedToday = 0;
        currentStats.maxActionsPerDay = 3;

        StatsEvents.TriggerStatsChanged(currentStats);
    }

    public PlayerStats GetCurrentStats() => currentStats;
    public List<string> GetTransactionHistory() => new List<string>(transactionHistory);

    public bool PerformAction(ActionEffect actionEffect)
    {
        if (!currentStats.CanPerformAction() && actionEffect.actionName != "Sleep")
        {
            Debug.LogWarning("Cannot perform action - daily limit reached!");
            return false;
        }

        currentStats.ModifyHealth(actionEffect.healthChange);
        currentStats.ModifyStress(actionEffect.stressChange);
        currentStats.ModifyCash(actionEffect.cashChange);

        if (actionEffect.actionName != "Sleep")
        {
            currentStats.actionsPerformedToday++;
        }

        if (actionEffect.cashChange != 0)
        {
            string transaction = $"{actionEffect.actionName}: ${actionEffect.cashChange:+0;-#}";
            transactionHistory.Add(transaction);

            if (transactionHistory.Count > 5)
                transactionHistory.RemoveAt(0);
        }

        if (actionEffect.actionName != "Sleep")
        {
            TimeManager.GetInstance().AdvanceTime();
        }

        StatsEvents.TriggerStatsChanged(currentStats);
        ActionEvents.TriggerActionPerformed(actionEffect);

        Debug.Log($"Action performed: {actionEffect.actionName}. Health: {currentStats.health}%, Stress: {currentStats.stress}%, Cash: ${currentStats.cash}");

        return true;
    }

    public void ResetDailyStats()
    {
        currentStats.ResetDailyStats();
        StatsEvents.TriggerStatsChanged(currentStats);
    }

    public void SetStats(PlayerStats newStats)
    {
        currentStats = newStats;
        StatsEvents.TriggerStatsChanged(currentStats);
    }
}