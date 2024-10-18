using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerHandler : MonoBehaviour
{
    public int second, gamePlaySecond;

    public Text secondTxt;
    public TextMeshProUGUI gamePlaySecTxt;

    public DashboardHandler dashboardHandler;
    public JoinGamePopup joinGamePopup;

    public void SecondSet(int sec)
    {
        second = sec;
        if (second >= 210)
        {
            dashboardHandler.firstBtn.interactable = true;
        }
        CancelInvoke(nameof(SecondDecrease));
        InvokeRepeating(nameof(SecondDecrease), 0f, 1f);
    }

    public void SecondSet(ResTimer resTimer)
    {
        uimanager.instance.periodTxt.text = resTimer.roundId.ToString();
        string formattedTime = FormatTime(resTimer.timer);
        secondTxt.text = formattedTime;
        if (!resTimer.iStartGame)
        {
            //Debug.Log("First");
            dashboardHandler.firstBtn.interactable = true;
            return;
        }

        if (resTimer.iStartGame || resTimer.isWinning)
        {
            int newInt = resTimer.timer - 30;
            if (newInt < 0) return;
            GameManager.instance.ButtonsOnOff(true);
            string formatteTime = FormatTime(newInt);
            gamePlaySecTxt.text = formatteTime;
            dashboardHandler.firstBtn.interactable = false;
        }
    }

    public void GamePlaySecondSet(int sec)
    {
        gamePlaySecond = sec;
        CancelInvoke(nameof(GameplaySecondDecrease));
        InvokeRepeating(nameof(GameplaySecondDecrease), 0f, 1f);
    }

    void SecondDecrease()
    {
        string formattedTime = FormatTime(second);
        secondTxt.text = formattedTime;
        //joinGamePopup.SecondManage(second);
        second--;
    }

    void GameplaySecondDecrease()
    {
        string formattedTime = FormatTime(gamePlaySecond);
        gamePlaySecTxt.text = formattedTime;
        gamePlaySecond--;
        if (gamePlaySecond < 0)
            CancelInvoke(nameof(GameplaySecondDecrease));
    }

    string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return string.Format("{0}:{1:D2}", minutes, seconds);
    }
}

#region MODEL_CLASS
[System.Serializable]
public class TimerResponse
{
    public int timer;
}

[System.Serializable]
public class ResTimer
{
    public int timer;
    public bool isWinning;
    public bool iStartGame;
    public int roundId;
}


#endregion
