using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DepositeHandler : MonoBehaviour
{
    public InputField amountInputfield, transctionIdinputfield;

    public MainDeposite mainDeposite;

    public void DepositeAmount()
    {
        StartCoroutine(Deposite());
    }

    public IEnumerator Deposite()
    {
        WWWForm form = new WWWForm();
        SendDeposite sendDeposite = new();
        sendDeposite.amount = int.Parse(amountInputfield.text);
        sendDeposite.transactionId = Double.Parse(transctionIdinputfield.text);

        string jsonData = JsonUtility.ToJson(sendDeposite);

        // Create a UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(StaticData.baseURL + StaticData.depositAmount, "POST");

        // Convert JSON string to byte array
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Attach raw data to the request
        request.uploadHandler = new UploadHandlerRaw(rawData);

        // Set response handler
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", GameManager.instance.token);

        //UploadHandlerRaw    

        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            mainDeposite = JsonUtility.FromJson<MainDeposite>(request.downloadHandler.text);

        }
    }
}

[System.Serializable]
public class DepositeData
{
    public string userId;
    public int amount;
    public string paymentType;
    public string status;
    public string transactionId;
    public string _id;
    public DateTime createdAt;
    public DateTime updatedAt;
    public string id;
}

public class MainDeposite
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public DepositeData data;
}


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [System.Serializable]
    public class SendDeposite
    {
        public int amount;
        public double transactionId;
    }



