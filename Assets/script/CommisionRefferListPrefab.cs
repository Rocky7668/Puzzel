using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CommisionRefferListPrefab : MonoBehaviour
{
    public TextMeshProUGUI UserNameTxt;
    public TextMeshProUGUI AmountTxt;

    public void SetData(string userName, string Amount)
    {
        UserNameTxt.text = userName;
        AmountTxt.text = Amount;
    }
}
