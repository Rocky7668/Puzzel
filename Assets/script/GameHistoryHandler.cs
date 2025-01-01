using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameHistoryHandler : MonoBehaviour
{
    public MainGameHistory MainGameHistory;
    public GameHistoryPrefab gameHistoryPrefab;

    List<GameHistoryPrefab> historyList = new List<GameHistoryPrefab>();
    [SerializeField] Transform Parent;

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
                GenerateGameHistory(MainGameHistory.data.Count);
            }
        }
    }

    void GenerateGameHistory(int count)
    {
        ClearHistory();
        for (int i = 0; i < count; i++)
        {
            GameHistoryPrefab gameHistoryPrefabClone = Instantiate(gameHistoryPrefab, Parent);
            string win = MainGameHistory.data[i].isWinner ? "Win" : "Lose";
            int GameEnd = MainGameHistory.data[i].gameCompleteTime - 30 - 90;
            string GameCompleteTime = FormatSecondsToTime(GameEnd);
            gameHistoryPrefabClone.SetData(MainGameHistory.data[i].roundId, win, MainGameHistory.data[i].winAmount, MainGameHistory.data[i].gameJoinTime,GameCompleteTime);
            historyList.Add(gameHistoryPrefab);
        }
    }

    void ClearHistory()
    {
        foreach (var item in historyList)
        {
            Destroy(item);
        }
    }

    string FormatSecondsToTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60; // Get the number of minutes
        int seconds = totalSeconds % 60; // Get the remaining seconds
        return $"{minutes:D2}:{seconds:D2}"; // Format as MM:SS
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
    public string roundId;
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
