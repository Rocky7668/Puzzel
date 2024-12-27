using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LegalityPanel : MonoBehaviour
{

    public MainLegality MainLegality;

    public TextMeshProUGUI LegalityText;

    private void OnEnable()
    {
        StartCoroutine(GetDetails());
    }
    public IEnumerator GetDetails()
    {
        print("Barcode");
        WWWForm www = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(StaticData.baseURL + StaticData.getLegality, www);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            print("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            MainLegality = JsonUtility.FromJson<MainLegality>(request.downloadHandler.text);
            LegalityText.text = MainLegality.data.legality;
        }
    }
}

[Serializable]
public class LegalityDATA
{
    public string _id ;
    public DateTime createdAt ;
    public string legality ;
    public DateTime updatedAt ;
    public string id ;
}

[Serializable]
public class MainLegality
{
    public string message ;
    public string status ;
    public int statusCode ;
    public bool success ;
    public LegalityDATA data ;
}
