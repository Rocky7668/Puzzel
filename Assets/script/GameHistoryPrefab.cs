using NUnit.Framework.Internal;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameHistoryPrefab : MonoBehaviour
{
    public Text PeriodNumberTxt;
    public Text WinTxt;
    public Text WinAmountTxt;
    public Text GameJoinTimeTxt;
    public Text GameEndTimeTxt;

    internal void SetData(string periodNumber, string Win, double winAmount, string gameJoin, string gameCompleteTime)
    {
        PeriodNumberTxt.text = periodNumber;
        WinTxt.text = Win;
        WinAmountTxt.text = "₹ "+winAmount.ToString();
        GameJoinTimeTxt.text = "Game Join Time  " + gameJoin;
        GameEndTimeTxt.text = "Game Complete Time  " + gameCompleteTime;
    }
}
