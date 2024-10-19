using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class CommisionHandler : MonoBehaviour
{
    public MainData mainData;

    public TextMeshProUGUI currentBalanceTxt;
    public TextMeshProUGUI bonusPointTxt;
    public TextMeshProUGUI depositePointTxt;
    public TextMeshProUGUI winningPointTxt;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetCommisionPoints());
    }


    internal IEnumerator GetCommisionPoints()
    {
        WWWForm form = new WWWForm();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.commisionPoints, form))
        {
            api.SetRequestHeader("Authorization", PlayerPrefs.GetString("token"));
            yield return api.SendWebRequest();
            Debug.Log("Http : " + api.downloadHandler.text);

            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log("Commission Points --------- " + api.downloadHandler.text);

                mainData = JsonUtility.FromJson<MainData>(api.downloadHandler.text);
                SetData();
            }
        }
    }

    internal void SetData()
    {
        currentBalanceTxt.text = "₹"+mainData.data.currentBalancePoints.ToString();
        bonusPointTxt.text = "₹" + mainData.data.bonusPoints.ToString();
        depositePointTxt.text = "₹" + mainData.data.depositPoints.ToString();
        winningPointTxt.text = "₹" + mainData.data.winningPoints.ToString();

    }

}



#region Extra Classes
[System.Serializable]
public class MainData
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public CommissionData data;
}

[System.Serializable]
public class CommissionData
{
    public int bonusPoints;
    public int depositPoints;
    public int winningPoints;
    public int currentBalancePoints;
    public ReferralList referralList;
}

[System.Serializable]
public class ReferralList
{
    public string phoneNumber;
    public double bonusPoints;
    public List<object> refreaaluserList;
}
#endregion
