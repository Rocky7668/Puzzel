using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

public class WallerhistoryPrefabHandler : MonoBehaviour
{
    public TextMeshProUGUI PaymenttypeTxt;
    public TextMeshProUGUI AmountTxt;
    public TextMeshProUGUI UsernameTxt;
    public TextMeshProUGUI TimmingTxt;
    public TextMeshProUGUI StatusTxt;

    public void SetDataHistory(string Paymenttype, int Amount, string username, string timming, string statusdata)
    {
        char ch = '-';
        PaymenttypeTxt.text = Paymenttype;
        if (Paymenttype == "Deposit")
            ch = '+';
        AmountTxt.text = ch + " ₹ " + Amount.ToString();
        UsernameTxt.text = username;
        TimmingTxt.text = timming;
        StatusTxt.text = statusdata;
    }

}