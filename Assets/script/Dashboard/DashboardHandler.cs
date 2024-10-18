using UnityEngine;
using UnityEngine.UI;

public class DashboardHandler : MonoBehaviour
{
    [Header("===== Scripts =====")]
    [SerializeField] private TimerHandler timerHandler;

    public Button firstBtn;

    public void DashboardSet(int timer)
    {
        timerHandler.GamePlaySecondSet(timer);
        firstBtn.interactable = false;
        if (timer < 210)
            firstBtn.interactable = true;
    }
}

#region ModelClass
[System.Serializable]
public class StartTimerRes
{
    public int timer;
}
#endregion
