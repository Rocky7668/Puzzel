using UnityEngine;
using UnityEngine.UI;

public class JoinGamePopup : MonoBehaviour
{
    public TimerHandler timerHandler;

    public Image gameImg;
    public Text joinGameTxt, periodTxt, joinAmountTxt, gstTxt, amountTxt, payTxt;



    private void OnEnable()
    {
        
    }



    public void MeCose()
    {
        gameObject.SetActive(false);
    }

    public void TextSet(int fee, double gst, double total)
    {
        joinGameTxt.text = "Join Time = " + timerHandler.gamePlaySecond;
        joinAmountTxt.text = "Join Amount - " + fee.ToString();
        gstTxt.text = "28% GST - " + gst.ToString();
        amountTxt.text = "Total Amount - " + total.ToString();
        payTxt.text = "Pay Now : " + total.ToString();
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
