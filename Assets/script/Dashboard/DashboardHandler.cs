using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class DashboardHandler : MonoBehaviour
{
    [Header("===== Scripts =====")]
    [SerializeField] private TimerHandler timerHandler;

    public Button firstBtn;

    public TextMeshProUGUI OnlineplayerTxt;
    public float OnlinePlayerCount;
    public TextMeshProUGUI totalCommission;



    private void Start()
    {
        OnlinePlayerCount = Random.Range(10000, 40000);
        OnlineplayerTxt.text = OnlinePlayerCount.ToString("F0");

        InvokeRepeating(nameof(SetOnlinePlayer), 2, Random.Range(3, 7));
    }

    private void FixedUpdate()
    {
        //totalCommission.text = StaticData.TotalCommission.ToString("F2");
    }

    public void DashboardSet(int timer)
    {
        timerHandler.GamePlaySecondSet(timer);
        firstBtn.interactable = false;
        if (timer < 210)
            firstBtn.interactable = true;
    }


    public void SetOnlinePlayer()
    {
        int random = Random.Range(0, 2);

        if (OnlinePlayerCount >= 5000 && OnlinePlayerCount <= 100000)
        {
            switch (random)
            {
                case 1:
                    OnlinePlayerCount += Random.Range(1000, 5000);
                    break;
                case 2:
                    OnlinePlayerCount -= Random.Range(10, 3000);
                    break;

            }
        }

        if (OnlinePlayerCount <= 5000)
        {
            OnlinePlayerCount += Random.Range(5000, 10000);
        }
        else if (OnlinePlayerCount <= 100000)
        {
            OnlinePlayerCount -= Random.Range(10000, 30000);
        }

        OnlinePlayerCount = Mathf.Abs(OnlinePlayerCount);
        OnlineplayerTxt.text = OnlinePlayerCount.ToString("F0");
    }


    //internal IEnumerator GetCommisionPoints()
    //{
    //    WWWForm form = new WWWForm();

    //    using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.commisionPoints, form))
    //    {
    //        api.SetRequestHeader("Authorization", PlayerPrefs.GetString("token"));
    //        yield return api.SendWebRequest();
    //        Debug.Log("Http : " + api.downloadHandler.text);

    //        if (api.result == UnityWebRequest.Result.ConnectionError)
    //        {
    //            Debug.Log("Data Not Found");
    //        }
    //        else
    //        {
    //            Debug.Log("Commission Points --------- " + api.downloadHandler.text);
    //            string s = JsonUtility.ToJson(api.downloadHandler.text);
    //            StaticData.TotalCommission = s[""];
    //        };
    //    }
    //}
}


#region ModelClass
[System.Serializable]
public class StartTimerRes
{
    public int timer;
}
#endregion
