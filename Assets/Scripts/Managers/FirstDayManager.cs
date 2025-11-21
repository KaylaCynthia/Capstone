using System.Collections;
using UnityEngine;

public class FirstDayManager : MonoBehaviour
{
    [Header("First Day Settings")]
    [SerializeField] private float autoReturnToHomeDelay = 5f;

    private bool isFirstDay = true;
    private bool isCompletedFirstDialogue = false;
    private bool hasReturnedToHome = false;

    private static FirstDayManager instance;
    public static FirstDayManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        ServerLockManager serverLockManager = ServerLockManager.GetInstance();
        if (serverLockManager != null && isFirstDay)
        {
            serverLockManager.LockServerSwitching();
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void HandleFirstDayConversationEnd()
    {
        if (isFirstDay && !hasReturnedToHome)
        {
            Debug.Log("First day first conversation ended.");
            StartCoroutine(AutoReturnToHome());
        }
    }

    private IEnumerator AutoReturnToHome()
    {
        Debug.Log("First conversation ended, returning to home in " + autoReturnToHomeDelay + " seconds");
        yield return new WaitForSeconds(autoReturnToHomeDelay);

        AppSystemManager appManager = AppSystemManager.GetInstance();
        TutorialManager tutorialManager = TutorialManager.GetInstance();
        if (appManager != null)
        {
            appManager.ReturnToHomeScreen();
            tutorialManager.StartTutorial("first_tutorial");
            hasReturnedToHome = true;
        }
    }

    public void CompleteFirstDay()
    {
        isFirstDay = false;
    }

    public void CompleteFirstDialogue()
    {
        isCompletedFirstDialogue = true;
    }

    public bool IsFirstDay() => isFirstDay;
    public bool IsCompletedFirstDialogue() => isCompletedFirstDialogue;
    public bool HasReturnedToHome() => hasReturnedToHome;
}