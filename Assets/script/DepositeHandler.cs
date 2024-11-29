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
    public GenerateQr GenerateQr;

    public List<Button> DepositeButtons;
    [SerializeField] private List<int> depositeBtnAmount;

    public RawImage QrCodeImage;

    public GameObject QrCodePanel;

    private void OnEnable()
    {
        for (int i = 0; i < DepositeButtons.Count; i++)
        {
            int index = i;
            DepositeButtons[i].onClick.AddListener(delegate
            {
                SetTextAmountInputField(index);
            });
        }
        amountInputfield.text = "";
        transctionIdinputfield.text = "";
        StartCoroutine(GetBarcodeImg());
    }
    private void OnDisable()
    {
        QrCodePanel.SetActive(false);
        amountInputfield.text = "";
        transctionIdinputfield.text = "";
    }
    public void OnClickPayNow()
    {
        if (amountInputfield.text.Length >= 3)
            QrCodePanel.SetActive(true);
    }

    public void DepositeAmount()
    {
        if (transctionIdinputfield.text.Length >= 2)
        {
            StartCoroutine(Deposite());
            QrCodePanel.SetActive(false);
        }
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
            amountInputfield.text = "";
            transctionIdinputfield.text = "";

        }
    }



    public IEnumerator GetBarcodeImg()
    {
        print("Barcode");
        WWWForm www = new WWWForm();
        UnityWebRequest request = UnityWebRequest.Post(StaticData.baseURL + StaticData.GetQrCode, www);
        request.SetRequestHeader("Authorization", GameManager.instance.token);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            print("Error: " + request.error);
        }
        else
        {
            GenerateQr = JsonUtility.FromJson<GenerateQr>(request.downloadHandler.text);
            StartCoroutine(DownloadQr(GenerateQr.data[0]));

        }
    }

    public IEnumerator DownloadQr(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading texture: " + request.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                QrCodeImage.texture = texture;
                QrCodeImage.color = Color.white;
            }
        }
    }


    public void SetTextAmountInputField(int Index)
    {
        amountInputfield.text = depositeBtnAmount[Index].ToString();
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

[System.Serializable]
public class MainDeposite
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public DepositeData data;
}



#region Generate Qr
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[System.Serializable]
public class GenerateQr
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public List<string> data;
}

#endregion



// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[System.Serializable]
public class SendDeposite
{
    public int amount;
    public double transactionId;
}