using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PrivacyPolicyPanel : MonoBehaviour
{

    public MainPrivacy mainPrivacy;

    public TextMeshProUGUI privacyText;
    private void OnEnable()
    {
        StartCoroutine(GetDetails());
    }
    public IEnumerator GetDetails()
    {
        print("Barcode");
        WWWForm www = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(StaticData.baseURL + StaticData.getPrivacyPolicy, www);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            print("Error: " + request.error);
        }
        else
        {
            mainPrivacy = JsonUtility.FromJson<MainPrivacy>(request.downloadHandler.text);
            privacyText.text = mainPrivacy.data.privacyAndPolicy;
        }
    }
}

[Serializable]
public class PrivacyData
{
    public string _id;
    public string createdAt;
    public string privacyAndPolicy;
    public string updatedAt;
    public string id;
}

[Serializable]
public class MainPrivacy
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public PrivacyData data;
}
