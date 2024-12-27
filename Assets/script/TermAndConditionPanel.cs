using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TermAndConditionPanel : MonoBehaviour
{
    public MainTAC mainTac;
    public TextMeshProUGUI policyText;


    private void OnEnable()
    {
        StartCoroutine(GetDetails());
    }
    public IEnumerator GetDetails()
    {
        print("Barcode");
        WWWForm www = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(StaticData.baseURL + StaticData.getTermsAndCondition, www);

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            print("Error: " + request.error);
        }
        else
        {
            mainTac = JsonUtility.FromJson<MainTAC>(request.downloadHandler.text);
            policyText.text = mainTac.data.termsAndConditions;
        }
    }
}
[Serializable]
public class TACData
{
    public string _id;
    public string createdAt;
    public string termsAndConditions;
    public string updatedAt;
    public string id;
}

[Serializable]
public class MainTAC
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public TACData data;
}
