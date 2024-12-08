using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerHandler : MonoBehaviour
{
    public int second, gamePlaySecond;

    public Text secondTxt, JoinGamePopUpTimeTxt;
    public TextMeshProUGUI gamePlaySecTxt;

    public DashboardHandler dashboardHandler;
    public JoinGamePopup joinGamePopup;

    public Text GameplayText;

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

        if (resTimer.timer >= 210)
        {
            secondTxt.text = "Join Time Left\n" + formattedTime;
            if(puzzleManager.instance.isEnterGame)
            {

            GameplayText.gameObject.SetActive(true);
            GameplayText.text = "Game Start in \n " + formattedTime + " sec";
            }else GameplayText.gameObject.SetActive(false);
        }
        else
        {
            secondTxt.text = "Wair For Next Round\n" + formattedTime;
            GameplayText.gameObject.SetActive(false);

        }


        if (!resTimer.iStartGame)
        {
            //Debug.Log("First");
            GameManager.instance.RunningGameTxt.text = "Previous Game";
            GameManager.instance.RunningGamePeriodNumber.text = (resTimer.roundId - 1).ToString();
            dashboardHandler.firstBtn.interactable = true;
            return;
        }

        if (resTimer.iStartGame || resTimer.isWinning)
        {
            int newInt = resTimer.timer - 30;
            if (newInt < 0) return;
            GameManager.instance.ButtonsOnOff(true);
            string formatteTime = FormatTime(newInt);
            int min = newInt / 60;
            int sec = newInt % 60;
            if (sec <= 30 && min == 0)
            {
                gamePlaySecTxt.fontSize = 100;
                gamePlaySecTxt.color = Color.white;
            }
            else
            {

                gamePlaySecTxt.fontSize = 52;
                gamePlaySecTxt.color = Color.red;
            }
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
        if (totalSeconds >= 210)
        {
            int n = totalSeconds - 210;
            Debug.Log("<color=red><b> n.ToString() " + n.ToString() + "</b></color>");
            return n.ToString();
        }
        return string.Format("{0}:{1:D2}", minutes, seconds);

    }

    private void Update()
    {
        JoinGamePopUpTimeTxt.text = "Join Time Left : " + secondTxt.text;
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
