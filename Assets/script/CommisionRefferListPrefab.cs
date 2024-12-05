using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CommisionRefferListPrefab : MonoBehaviour
{
    public TextMeshProUGUI UserNameTxt;
    public TextMeshProUGUI AmountTxt;
    public TextMeshProUGUI TileTxt;

    public void SetData(string userName, string Amount,string Title)
    {
        UserNameTxt.text = userName;
        AmountTxt.text = "₹"+Amount;
        TileTxt.text = Title;
    }
}
