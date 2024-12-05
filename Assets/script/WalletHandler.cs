using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WalletHandler : MonoBehaviour
{
    [SerializeField] private int start = 0;
    [SerializeField] private int limit = 5;

    public MainWithdrawHistory mainWithdrawhistory;
    public MainDepositeHistory maindepositehistory;

    [SerializeField] private WallerhistoryPrefabHandler WallerhistoryPrefabHandlerPrefab;

    [SerializeField] private Transform Parent;

    [SerializeField] private TextMeshProUGUI totalCashTxt;




    private void OnEnable()
    {
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            Destroy(Parent.transform.GetChild(i));
        }

        StartCoroutine(WithdrawHistory());
        StartCoroutine(DepositeHistory());

        totalCashTxt.text = "₹ " + StaticData.TotalBalance;
    }

    public IEnumerator WithdrawHistory()
    {
        WWWForm form = new WWWForm();
        form.AddField("start", start);
        form.AddField("limit", limit);


        using (var profileApi = UnityWebRequest.Post(StaticData.baseURL + StaticData.userWithdrawHistory, form))
        {
            profileApi.SetRequestHeader("Authorization", GameManager.instance.token);

            yield return profileApi.SendWebRequest();

            if (profileApi.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found : " + profileApi.error);
            }
            else
            {
                Debug.Log(profileApi.downloadHandler.text);
                Debug.Log("Withdra History fetch.....");

                mainWithdrawhistory = JsonUtility.FromJson<MainWithdrawHistory>(profileApi.downloadHandler.text);
                int count = mainWithdrawhistory.data.docs.Count;
                GenerateWithdrawHistory(count);
            }
        }
    }

    public IEnumerator DepositeHistory()
    {
        WWWForm form = new WWWForm();
        form.AddField("start", start);
        form.AddField("limit", limit);


        using (var profileApi = UnityWebRequest.Post(StaticData.baseURL + StaticData.userDepositeHistory, form))
        {
            profileApi.SetRequestHeader("Authorization", GameManager.instance.token);

            yield return profileApi.SendWebRequest();

            if (profileApi.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found : " + profileApi.error);
            }
            else
            {
                Debug.Log(profileApi.downloadHandler.text);
                Debug.Log("Deposite History fetch......");

                maindepositehistory = JsonUtility.FromJson<MainDepositeHistory>(profileApi.downloadHandler.text);
                int count = maindepositehistory.data.docs.Count;
                GenerateDepositeHistory(count);
            }
        }
    }
    List<GameObject> deposite = new();
    List<GameObject> withdraw = new();
    public void GenerateDepositeHistory(int count)
    {
        ClearHistory();
        for (int i = 0; i < count; i++)
        {
            WallerhistoryPrefabHandler wallerhistoryPrefabHandlerClone = Instantiate(WallerhistoryPrefabHandlerPrefab, Parent);
            wallerhistoryPrefabHandlerClone.SetDataHistory(maindepositehistory.data.docs[i].paymentType, maindepositehistory.data.docs[i].amount, maindepositehistory.data.docs[i].userId, maindepositehistory.data.docs[i].createdAt.ToString(), maindepositehistory.data.docs[i].status);
            deposite.Add(wallerhistoryPrefabHandlerClone.gameObject);
        }
    }

    public void GenerateWithdrawHistory(int count)
    {
        ClearHistory();
        for (int i = 0; i < count; i++)
        {
            WallerhistoryPrefabHandler wallerhistoryPrefabHandlerClone = Instantiate(WallerhistoryPrefabHandlerPrefab, Parent);
            wallerhistoryPrefabHandlerClone.SetDataHistory(mainWithdrawhistory.data.docs[i].paymentType, mainWithdrawhistory.data.docs[i].amount, mainWithdrawhistory.data.docs[i].userId, mainWithdrawhistory.data.docs[i].createdAt.ToString(), mainWithdrawhistory.data.docs[i].status);
            withdraw.Add(wallerhistoryPrefabHandlerClone.gameObject);
        }
    }

    public void ClearHistory()
    {
        for (int i = 0; i <deposite.Count; i++)
        {
            Destroy(deposite[i].gameObject);
        }
        deposite.Clear();

        for (int i = 0; i < withdraw.Count; i++)
        {
            Destroy(withdraw[i].gameObject);
        }
        withdraw.Clear();
    }
}

#region Withdraw History
[System.Serializable]
public class WithdrawHistoryData
{
    public List<WithdrawHistoryList> docs;
    public int totalDocs;
    public int offset;
    public int limit;
    public int totalPages;
    public int page;
    public int pagingCounter;
    public bool hasPrevPage;
    public bool hasNextPage;
    public object prevPage;
    public object nextPage;
}
[System.Serializable]
public class WithdrawHistoryList
{
    public string _id;
    public string userId;
    public int amount;
    public string paymentType;
    public string bankName;
    public string IFSC;
    public string accountHolderName;
    public string accountNumber;
    public string status;
    public double tdsAmount;
    public int tdsPercentage;
    public string tdsId;
    public string createdAt;
    public string updatedAt;
    public string id;
}

[System.Serializable]
public class MainWithdrawHistory
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public WithdrawHistoryData data;
}

#endregion


#region Deposite History

[System.Serializable]
public class Depositehistory
{
    public List<DepositehistoryList> docs;
    public int totalDocs;
    public int offset;
    public int limit;
    public int totalPages;
    public int page;
    public int pagingCounter;
    public bool hasPrevPage;
    public bool hasNextPage;
    public object prevPage;
    public object nextPage;
}

[System.Serializable]
public class DepositehistoryList
{
    public string _id;
    public string userId;
    public int amount;
    public string paymentType;
    public string status;
    public string transactionId;
    public string createdAt;
    public string updatedAt;
    public string id;
}

[System.Serializable]
public class MainDepositeHistory
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public Depositehistory data;
}

#endregion

