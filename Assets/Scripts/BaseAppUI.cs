using UnityEngine;

public abstract class BaseAppUI : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        StatsEvents.OnStatsChanged += OnStatsChanged;
        TimeEvents.OnTimeChanged += OnTimeChanged;
    }

    protected virtual void OnDisable()
    {
        StatsEvents.OnStatsChanged -= OnStatsChanged;
        TimeEvents.OnTimeChanged -= OnTimeChanged;
    }

    protected virtual void OnStatsChanged(PlayerStats stats)
    {
        // Override in derived classes if needed
    }

    protected virtual void OnTimeChanged(TimeManager.TimeOfDay time)
    {
        // Override in derived classes if needed
    }
}