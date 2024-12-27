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

    public float OnlinePlayerCount;
    public TextMeshProUGUI totalCommission;



    private void FixedUpdate()
    {
        totalCommission.text = "₹ " + StaticData.TotalCommission.ToString("F2");
    }

    public void DashboardSet(int timer)
    {
        timerHandler.GamePlaySecondSet(timer);
        firstBtn.interactable = false;
        if (timer < 210)
            firstBtn.interactable = true;
    }

    MainCommissionList MainCommissionList;
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
                MainCommissionList.data.totalCommission = StaticData.TotalCommission;
            }
        }
    }

}


#region ModelClass
[System.Serializable]
public class StartTimerRes
{
    public int timer;
}
#endregion
