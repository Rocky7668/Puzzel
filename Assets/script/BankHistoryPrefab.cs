using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BankHistoryPrefab : MonoBehaviour
{
    public TextMeshProUGUI bankNameTxt;
    public TextMeshProUGUI AccountNumberTxt;
    public TextMeshProUGUI AccountHolderNameTxt;
    public TextMeshProUGUI AccountIFSCCode;

    internal int Index;


    public void SetBankPrefabData(string BankName, string AccountNumber, string AccountName, string IFSCCode, int index)
    {
        bankNameTxt.text = BankName;
        AccountNumberTxt.text = AccountNumber;
        AccountHolderNameTxt.text = AccountName;
        AccountIFSCCode.text = IFSCCode;
        Index = index;
    }
}
