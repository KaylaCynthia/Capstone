using UnityEngine;

public class ServerManager : MonoBehaviour
{
    [Header("Server References")]
    [SerializeField] private GameObject dmsServer;
    [SerializeField] private GameObject channelsServer;

    [Header("Default Server")]
    [SerializeField] private bool startWithDMs = true;

    private string currentServerType = "DMs";

    private static ServerManager instance;
    public static ServerManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        InitializeServers();
    }

    private void InitializeServers()
    {
        if (startWithDMs)
        {
            ShowDMsServer();
        }
        else
        {
            ShowChannelsServer();
        }
    }

    public void ShowDMsServer()
    {
        dmsServer.SetActive(true);
        channelsServer.SetActive(false);
        currentServerType = "DMs";

        ServerEvents.TriggerServerChanged(currentServerType);
    }

    public void ShowChannelsServer()
    {
        dmsServer.SetActive(false);
        channelsServer.SetActive(true);
        currentServerType = "Channels";

        ServerEvents.TriggerServerChanged(currentServerType);
    }

    public void ToggleServers()
    {
        if (currentServerType == "DMs")
        {
            ShowChannelsServer();
        }
        else
        {
            ShowDMsServer();
        }
    }

    public string GetCurrentServerType() => currentServerType;
    public bool IsDMsServerActive() => currentServerType == "DMs";
    public bool IsChannelsServerActive() => currentServerType == "Channels";
}