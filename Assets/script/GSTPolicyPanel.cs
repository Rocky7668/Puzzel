using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GSTPolicyPanel : MonoBehaviour
{

    public MainGst MainGst;

    public TextMeshProUGUI privacyText;
    private void OnEnable()
    {
        StartCoroutine(GetDetails());
    }
    public IEnumerator GetDetails()
    {
        print("Barcode");
        WWWForm www = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(StaticData.baseURL + StaticData.getGstPolicy, www);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            print("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            MainGst = JsonUtility.FromJson<MainGst>(request.downloadHandler.text);
            privacyText.text = MainGst.data.GSTPolicy;
        }
    }
}

[Serializable]
public class GSTDATA
{
    public string _id;
    public string GSTPolicy;
    public DateTime createdAt;
    public DateTime updatedAt;
    public string id;
}

[Serializable]
public class MainGst
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public GSTDATA data;
}
