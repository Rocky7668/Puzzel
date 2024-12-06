using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProfileHandler : MonoBehaviour
{
    public static ProfileHandler instance;


    public Text userNameTxt, userIdTxt,userMobilenumber;
    public Slider pointSlider;

    public TextMeshProUGUI amtTxt;

    public ProfileRes profileRes;
    public NotificationRes notificationRes;

    [Header("===== Notification =====")]
    [SerializeField] private Transform notificationGenerator;
    [SerializeField] private NotificationPrefabHandler notificationPrefabHandler;
    [SerializeField] private List<NotificationPrefabHandler> notificationPrefabHandlers;

    public Text RefferalCodeText;


    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        ProfileDataSet();
    }

    public void ProfileDataSet()
    {
        StartCoroutine(PostProfile());
    }

    IEnumerator PostProfile()
    {
        WWWForm form = new();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.getProfile, form))
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
                Debug.Log(api.downloadHandler.text);
                profileRes = JsonUtility.FromJson<ProfileRes>(api.downloadHandler.text);
                userNameTxt.text = profileRes.data._id.Substring(0, 8); 
                userIdTxt.text = "#"+profileRes.data._id;
                pointSlider.maxValue = profileRes.data.totalGamePlayed;
                pointSlider.value = profileRes.data.totalWinGame;
                amtTxt.text = profileRes.data.amount.ToString("F2");
                userMobilenumber.text = profileRes.data.phoneNumber;

                RefferalCodeText.text = profileRes.data.referralCode;
                StaticData.TotalBalance = profileRes.data.amount;
            }
        }
    }

    public void NotifoicationSet()
    {
        StartCoroutine(PostNotification());
    }

    IEnumerator PostNotification()
    {
        WWWForm form = new();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.getNotification, form))
        {
            api.SetRequestHeader("Authorization", GameManager.instance.token);

            yield return api.SendWebRequest();

            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error : "+api.error);
            }
            else
            {
                Debug.Log(api.downloadHandler.text);
                notificationRes = JsonUtility.FromJson<NotificationRes>(api.downloadHandler.text);
                DestroyNotification();
                for (int i = 0; i < notificationRes.data.Count; i++)
                {
                    NotificationPrefabHandler notificationPrefabHandlerClone = Instantiate(notificationPrefabHandler, notificationGenerator);
                    notificationPrefabHandlers.Add(notificationPrefabHandlerClone);
                    notificationPrefabHandlerClone.mainTxt.text = notificationRes.data[i].notificationContent;
                    //notificationPrefabHandlerClone.btnTxt.text = "Add Coin";
                }
            }
        }
    }

    public void CopyReffralCode()
    {
        GUIUtility.systemCopyBuffer = RefferalCodeText.text;
    }

    void DestroyNotification()
    {
        foreach (var item in notificationPrefabHandlers)
        {
            Destroy(item.gameObject);
        }
        notificationPrefabHandlers.Clear();
    }
}

#region ModelClass
[System.Serializable]
public class ProfileResData
{
    public string _id;
    public string phoneNumber;
    public double amount;
    public string role;
    public string referralCode;
    public int totalGamePlayed;
    public int totalWinGame;
}

[System.Serializable]
public class ProfileRes
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public ProfileResData data;
}

[System.Serializable]
public class NotificationResData
{
    public string _id;
    public string notificationImage;
    public string notificationContent;
    public System.DateTime createdAt;
    public System.DateTime updatedAt;
    public string id;
}

[System.Serializable]
public class NotificationRes
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public List<NotificationResData> data;
}
#endregion
