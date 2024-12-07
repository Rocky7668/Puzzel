using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class CommisionHandler : MonoBehaviour
{
    public MainData mainData;
    public MainCommissionList MainCommissionList;

    public TextMeshProUGUI currentBalanceTxt;
    public TextMeshProUGUI bonusPointTxt;
    public TextMeshProUGUI depositePointTxt;
    public TextMeshProUGUI winningPointTxt;

    public CommisionRefferListPrefab commisionRefferListPrefab;

    internal List<CommisionRefferListPrefab> CommisionRefferListPrefabList = new();

    public GameObject ListPanel;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetCommisionPoints());
        StartCoroutine(GetCommisionList());
    }

    private void OnDisable() => ListPanel.SetActive(false);

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
        currentBalanceTxt.text = "₹" + mainData.data.currentBalancePoints.ToString("F2");
        bonusPointTxt.text = "₹" + mainData.data.bonusPoints.ToString("F2");
        depositePointTxt.text = "₹" + mainData.data.depositPoints.ToString("F2");
        winningPointTxt.text = "₹" + mainData.data.winningPoints.ToString("F2");

    }


    internal IEnumerator GetCommisionList()
    {
        WWWForm form = new WWWForm();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.GetCommissionHistory, form))
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
                MainCommissionList = JsonUtility.FromJson<MainCommissionList>(api.downloadHandler.text);
                StaticData.TotalCommission = MainCommissionList.data.totalCommission;
                int count = MainCommissionList.data.commisionHistory.Count;
                GenerateCommissionList(count);
            }
        }
    }


    public Transform CommisionRefferPrefabContent;
    void GenerateCommissionList(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CommisionRefferListPrefab CommisionRefferListPrefabClone = Instantiate(commisionRefferListPrefab, CommisionRefferPrefabContent);
            CommisionRefferListPrefabList.Add(CommisionRefferListPrefabClone);
            string userPhone = MainCommissionList.data.commisionHistory[i].fromUserId.phoneNumber;
            string amount = MainCommissionList.data.commisionHistory[i].amount.ToString();
            string title = MainCommissionList.data.commisionHistory[i].title;
            CommisionRefferListPrefabClone.SetData(userPhone, amount, title);
        }
    }

    void clearList()
    {
        for (int i = 0; i < CommisionRefferPrefabContent.transform.childCount; i++)
        {
            Destroy(CommisionRefferPrefabContent.transform.GetChild(i).gameObject);
        }
        CommisionRefferListPrefabList.Clear();
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
    public float bonusPoints;
    public float depositPoints;
    public float winningPoints;
    public float currentBalancePoints;
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


#region Commision List

[System.Serializable]
public class MainCommissionListData
{
    public string _id;
    public string userId;
    public string title;
    public double amount;
    public string transactionType;
    public string createdAt;
    public string updatedAt;
    public FromUserId fromUserId;
    public string id;
}


[System.Serializable]
public class FromUserId
{
    public string _id;
    public string phoneNumber;
    public string id;
}


[System.Serializable]
public class MainCommissionList
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public MainCommissionData data;

}

[System.Serializable]
public class MainCommissionData
{
    public List<MainCommissionListData> commisionHistory;
    public double totalCommission;
}


#endregion
