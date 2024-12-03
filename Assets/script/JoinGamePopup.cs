using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JoinGamePopup : MonoBehaviour
{
    public TimerHandler timerHandler;

    public Image gameImg;
    public Text joinGameTxt, periodTxt, joinAmountTxt, gstTxt, amountTxt, payTxt, ReturngstAmount;

    public void MeCose()
    {
        gameObject.SetActive(false);
    }

    public void TextSet(int fee, double gst, double total)
    {
        // joinGameTxt.text = "Join Time Left : " + timerHandler.secondTxt.text;
        joinAmountTxt.text = fee.ToString();
        gstTxt.text = gst.ToString();
        ReturngstAmount.text = gst.ToString();
        amountTxt.text = total.ToString();
        payTxt.text = "Pay Now : " + total.ToString();
        periodTxt.text = GameManager.instance.periodnumber.ToString();
    }

    public void SecondManage(int second)
    {
        joinGameTxt.text = "Join Time = " + (second - 209) + " Seconds";
        if (second >= 0)
        {
            MeCose();
        }
    }
}