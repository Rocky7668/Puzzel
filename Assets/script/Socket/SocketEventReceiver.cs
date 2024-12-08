using System;
using UnityEngine;
using static StaticData;

public class SocketEventReceiver : MonoBehaviour
{
    [Header("===== Scripts =====")]
    [SerializeField] private TimerHandler timerHandler;
    [SerializeField] private DashboardHandler dashboardHandler;
    [SerializeField] private WinHandler winHandler;
    [SerializeField] private JoinGamePopup joinGamePopup;

    [Header("===== Model Class =====")]
    [SerializeField] private TimerResponse timerResponse;
    [SerializeField] private JoinTableRes joinTableRes;
    [SerializeField] private StartTimerRes startTimerRes;
    [SerializeField] private WinRes winRes;
    [SerializeField] private EntryFeeResponse entryFeeResponse;
    [SerializeField] private ResTimer resTimer;


    public void HandleEventResponse(string en, string res)
    {
        string enumString = en;
        PuzzleEvent enumValue = (PuzzleEvent)Enum.Parse(typeof(PuzzleEvent), enumString);

        Debug.Log("Event : " + en + "\nResponse : " + res);

        switch (enumValue)
        {
            case PuzzleEvent.START_TIMER:
                timerResponse = JsonUtility.FromJson<TimerResponse>(res);
                timerHandler.SecondSet(timerResponse.timer);
                GameManager.instance.ButtonsOnOff(false);
                uimanager.instance.win.SetActive(false);
                uimanager.instance.home.SetActive(true);
                uimanager.instance.play.SetActive(false);
                break;

            case PuzzleEvent.JOIN_TABLE:

                joinTableRes = JsonUtility.FromJson<JoinTableRes>(res);
                if (joinTableRes.success)
                {
                    GameManager.instance.ButtonsOnOff(false);
                    uimanager.instance.HomeButtonClick();
                    uimanager.instance.amountTxt.text = joinTableRes.amount.ToString("F2");
                }
                else
                    uimanager.instance.NotChips();
                break;

            case PuzzleEvent.START_GAME:
                startTimerRes = JsonUtility.FromJson<StartTimerRes>(res);
                dashboardHandler.DashboardSet(startTimerRes.timer);
                GameManager.instance.ButtonsOnOff(true);
                break;

            case PuzzleEvent.END_GAME:
                GameManager.instance.ButtonsOnOff(false);
                break;

            case PuzzleEvent.SUBMIT_TIMER:
                Debug.Log("Timer is submitted");
                break;

            case PuzzleEvent.ENTRYFEE:
                entryFeeResponse = JsonUtility.FromJson<EntryFeeResponse>(res);
                Debug.Log("Entry Fee deducted and game start");
                uimanager.instance.time.SetActive(true);
                NewUIManager.instance.OpenPanel(Panel.JoinGamePopUp);
                entryFeeResponse.totalEntryFee -= entryFeeResponse.gstAmount;
                joinGamePopup.TextSet(entryFeeResponse.entryFee, entryFeeResponse.gstAmount, entryFeeResponse.totalEntryFee);
                break;

            case PuzzleEvent.WINNER:
                winRes = JsonUtility.FromJson<WinRes>(res);
                if (uimanager.instance.play.activeInHierarchy && !GameManager.instance.isPraticeMode)
                {
                    NewUIManager.instance.OpenPanel(Panel.Win);
                    //uimanager.instance.WinButtonClick();
                    winHandler.WinningSet(winRes);
                    puzzleManager.instance.isEnterGame = false;
                }
                break;

            case PuzzleEvent.RES_TIMER:
                resTimer = JsonUtility.FromJson<ResTimer>(res);
                timerHandler.SecondSet(resTimer);
                GameManager.instance.isStarGame = resTimer.iStartGame;
                GameManager.instance.isWingame = resTimer.isWinning;
                GameManager.instance.periodnumber = resTimer.roundId;
                GameManager.instance.time = resTimer.timer;
                break;
        }
    }
}
