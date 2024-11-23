using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Profiling;

public class TransactionHistory : MonoBehaviour
{
    public TransactionHistoryRes transactionHistoryRes;

    public TransactionHistoryHandler transactionHistoryHandler;
    public List<TransactionHistoryHandler> transactionHistoryHandlers;
    public Transform historyGenerator;

    IEnumerator GetTransactionHistory()
    {
        WWWForm form = new();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.transactionHistory, form))
        {
            Debug.Log("NEtwork : " + api.result);
            api.SetRequestHeader("Authorization", GameManager.instance.token);
            yield return api.SendWebRequest();
            Debug.Log("Http : " + api.error);
            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found : " + api.error);
            }
            else
            {
                Debug.Log("Transaction History : " + api.downloadHandler.text);
                transactionHistoryRes = JsonUtility.FromJson<TransactionHistoryRes>(api.downloadHandler.text);
                SetTransactionHistory();

            }
        }
    }

    public void TransactionHistoryMain()
    {
        StartCoroutine(GetTransactionHistory());
    }

    void SetTransactionHistory()
    {
        uimanager.instance.transactionPanel.SetActive(true);

        if (transactionHistoryRes.data.docs.Count < 1)
            return;

        DestroyPrefab();
        int cnt = transactionHistoryRes.data.docs.Count > 20 ? 20 : transactionHistoryRes.data.docs.Count;
        for (int i = 0; i < cnt; i++)
        {
            TransactionHistoryHandler transactionHistoryHandlerClone = Instantiate(transactionHistoryHandler, historyGenerator);
            transactionHistoryHandlerClone.typeTxt.text = transactionHistoryRes.data.docs[i].amount.ToString();
            if (transactionHistoryRes.data.docs[i].transactionType == "credited")
                transactionHistoryHandlerClone.typeTxt.color = Color.green;
            else
                transactionHistoryHandlerClone.typeTxt.color = Color.red;
            transactionHistoryHandlers.Add(transactionHistoryHandlerClone);
        }
    }

    void DestroyPrefab()
    {
        foreach (var item in transactionHistoryHandlers)
        {
            Destroy(item.gameObject);
        }
        transactionHistoryHandlers.Clear();
    }
}

[System.Serializable]
public class TransactionHistoryResData
{
    public List<Doc> docs;
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
public class Doc
{
    public string _id;
    public string userId;
    public string title;
    public double amount;
    public string transactionType;
    public string createdAt;
    public string updatedAt;
    public string id;
}
[System.Serializable]
public class TransactionHistoryRes
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public TransactionHistoryResData data;
}


