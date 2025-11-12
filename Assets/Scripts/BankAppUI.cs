using TMPro;
using UnityEngine;

public class BankAppUI : BaseAppUI
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI cashBalanceText;
    [SerializeField] private TextMeshProUGUI lastUpdatedText;
    [SerializeField] private TextMeshProUGUI transactionHistoryText;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void RefreshUI()
    {
        UpdateBankDisplay();
    }

    private void UpdateBankDisplay()
    {
        if (cashBalanceText != null)
        {
            PlayerStats currentStats = StatsManager.GetInstance().GetCurrentStats();
            cashBalanceText.text = $"${currentStats.cash}";
        }

        if (lastUpdatedText != null)
        {
            lastUpdatedText.text = $"Last updated: {System.DateTime.Now.ToString("hh:mm tt")}";
        }

        if (transactionHistoryText != null)
        {
            transactionHistoryText.text = "Recent Transactions:\nWork: +$100\nWork: +$100";
        }
    }

    protected override void OnStatsChanged(PlayerStats stats)
    {
        UpdateBankDisplay();
    }
}