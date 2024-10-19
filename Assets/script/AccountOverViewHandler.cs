using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class AccountOverViewHandler : MonoBehaviour
{
    public OverView overView;

    public TextMeshProUGUI depositeTxt;
    public TextMeshProUGUI withdralTxt;
    public TextMeshProUGUI totalCashTxt;
    public TextMeshProUGUI taxDeducatedTxt;
    public TextMeshProUGUI pendingBonusTxt;
    public TextMeshProUGUI releasedBonusTxt;
    public TextMeshProUGUI practiceChipsTxt;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetCommisionPoints());
    }

    internal IEnumerator GetCommisionPoints()
    {
        WWWForm form = new WWWForm();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.userAccountView, form))
        {
            api.SetRequestHeader("Authorization", PlayerPrefs.GetString("token"));
            yield return api.SendWebRequest();

            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log("OverView --------- " + api.downloadHandler.text);

                overView = JsonUtility.FromJson<OverView>(api.downloadHandler.text);
                SetData();
            }
        }
    }

    internal void SetData()
    {
        depositeTxt.text = overView.data.depositBalance.ToString();
        withdralTxt.text = overView.data.withdrawalBalance.ToString();
        totalCashTxt.text = overView.data.cashBalance.ToString();
        taxDeducatedTxt.text = overView.data.depositBalance.ToString();

    }
}

#region Extra Classes

[System.Serializable]
public class OverViewData
{
    public int withdrawalBalance;
    public int depositBalance;
    public int cashBalance;
    public int taxDeductBalance;
    public int totalBonus;
}

[System.Serializable]
public class OverView
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public OverViewData data;
}
#endregion
