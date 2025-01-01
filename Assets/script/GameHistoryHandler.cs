using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameHistoryHandler : MonoBehaviour
{

    public MainGameHistory MainGameHistory;
    public GameHistoryPrefab gameHistoryPrefab;

    private void OnEnable()
    {
        StartCoroutine(GetGameHistory());
    }

    internal IEnumerator GetGameHistory()
    {
        WWWForm form = new WWWForm();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.GetGameHistory, form))
        {
            api.SetRequestHeader("Authorization", GameManager.instance.token);
            yield return api.SendWebRequest();
            Debug.Log("Http : " + api.downloadHandler.text);

            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log(" Main Game History --------- " + api.downloadHandler.text);
                MainGameHistory = JsonUtility.FromJson<MainGameHistory>(api.downloadHandler.text);

            }
        }
    }

    void GenerateGameHistory()
    {

    }

    void ClearHistory()
    {

    }
}




[Serializable]
public class MainGameHistoryData
{
    public string userId;
    public int gameCompleteTime;
    public string gameJoinTime;
    public bool isWinner;
    public int winAmount;
}

[Serializable]
public class MainGameHistory
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public List<MainGameHistoryData> data;
}
