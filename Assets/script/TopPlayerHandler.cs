using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TopPlayerHandler : MonoBehaviour
{
    public TopPlayerResponse topPlayerResponse;

    public TopPlayerPrefabHandler topPlayerPrefabHandler;
    public Transform topPlayerGenerator;
    public List<TopPlayerPrefabHandler> topPlayerPrefabHandlers;

    IEnumerator GetTransactionHistory()
    {
        WWWForm form = new();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.getTopusers, form))
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
                topPlayerResponse = JsonUtility.FromJson<TopPlayerResponse>(api.downloadHandler.text);
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
        uimanager.instance.topPanel.SetActive(true);

        if (topPlayerResponse.data.Count < 1)
            return;

        DestroyPrefab();
        int cnt = topPlayerResponse.data.Count > 20 ? 20 : topPlayerResponse.data.Count;

        for (int i = 0; i < cnt; i++)
        {
            TopPlayerPrefabHandler transactionHistoryHandlerClone = Instantiate(topPlayerPrefabHandler, topPlayerGenerator);
            transactionHistoryHandlerClone.rankTxt.text = topPlayerResponse.data[i].rank.ToString();
            //if (topPlayerResponse.data[i].email != "")
                //transactionHistoryHandlerClone.nameTxt.text = topPlayerResponse.data[i].email.Split('@')[0].ToString();
            transactionHistoryHandlerClone.amountTxt.text = topPlayerResponse.data[i].amountOfWinnLoss.ToString();
            topPlayerPrefabHandlers.Add(transactionHistoryHandlerClone);
        }        
    }

    void DestroyPrefab()
    {
        foreach (var item in topPlayerPrefabHandlers)
        {
            Destroy(item.gameObject);
        }
        topPlayerPrefabHandlers.Clear();
    }
}

[System.Serializable]
public class TopPlayerResponseData
{
    public string userId;
    public double totalAmountOfLoss;
    public int totalAmountOfWinn;
    public double amountOfWinnLoss;
    public int rank;
    public string email;
}

[System.Serializable]
public class TopPlayerResponse
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public List<TopPlayerResponseData> data;
}


