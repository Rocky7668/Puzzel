using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class WithdrawHandler : MonoBehaviour
{
    public static WithdrawHandler instance;

    public InputField amountInputfield, bankNameInputfield, IFSCInputfield, accountHolderNameInputfield, accountNumberInputfield;

    public MainUserWithdraw mainUserWithdraw;

    public MainAddBank MainAddBank;
    public MainGetBank MainGetBank;

    public GameObject addBankPanel;
    public GameObject BankListPanel;


    [Header("AddBank Section")]
    public InputField AddbankNameInputfield;
    public InputField AddIFSCInputfield;
    public InputField AddaccountHolderNameInputfield;
    public InputField AddaccountNumberInputfield;


    [Header("BankHistory Section")]
    public BankHistoryPrefab bankHistoryPrefab;
    public Transform HistoryParent;

    public List<BankHistoryPrefab> bankHistoryPrefabList = new();



    [Header("UPI")]
    public InputField UPIInputField;




    public GameObject BankSectionBoard, UPISectionBoard;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        GetBankData();
        isBank = true;
    }
    private void OnDisable()
    {
        addBankPanel.SetActive(false);
        BankListPanel.SetActive(false);
        UPISectionBoard.SetActive(false);
        BankSectionBoard.SetActive(true);

        UPIInputField.text = "";
        amountInputfield.text = "";
        amountInputfield.text = "";
        bankNameInputfield.text = "";
        IFSCInputfield.text = "";
        accountHolderNameInputfield.text = "";
        accountNumberInputfield.text = "";
        AddbankNameInputfield.text = "";
        AddaccountNumberInputfield.text = "";
        AddIFSCInputfield.text = "";
        AddaccountHolderNameInputfield.text = "";
    }

    public void WithdrawAmount()
    {
        if (amountInputfield.text.Length >= 3 && bankNameInputfield.text.Length >= 2 && IFSCInputfield.text.Length >= 2 && accountHolderNameInputfield.text.Length >= 2 && accountNumberInputfield.text.Length >= 2 && isBank)
            StartCoroutine(WithDrawAmountApiBank());
        else if (amountInputfield.text.Length >= 3 && UPIInputField.text.Length >= 5)
            StartCoroutine(WithDrawAmountApiBank());  ////////////////////
    }
    public void AddBankData()
    {
        StartCoroutine(addBankAccount());
    }
    public void GetBankData()
    {
        StartCoroutine(GetBankHis());
    }


    public void ClearBankHistory()
    {
        for (int i = 0; i < HistoryParent.transform.childCount; i++)
        {
            Destroy(HistoryParent.transform.GetChild(i).gameObject);
        }
        bankHistoryPrefabList.Clear();
    }

    public void GenerateBankHistory(int count)
    {
        ClearBankHistory();

        for (int i = 0; i < count; i++)
        {
            int Index = i;

            BankHistoryPrefab bankHistoryPrefabClone = Instantiate(bankHistoryPrefab, HistoryParent);
            bankHistoryPrefabList.Add(bankHistoryPrefabClone);
            bankHistoryPrefabClone.GetComponent<Button>().onClick.AddListener(delegate
            {
                OnClickBanHistoryButton(Index);
            });
            string bankname = MainGetBank.data.docs[i].bankName;
            string Accountnumber = MainGetBank.data.docs[i].accountNumber;
            string AccountHolderName = MainGetBank.data.docs[i].accountHolderName;
            string AccountIFSCCode = MainGetBank.data.docs[i].IFSC;

            bankHistoryPrefabClone.SetBankPrefabData(bankname, Accountnumber, AccountHolderName, AccountIFSCCode);
        }
    }

    public void OnClickBanHistoryButton(int Index)
    {
        addBankPanel.SetActive(false);
        BankListPanel.SetActive(false);
        bankNameInputfield.text = bankHistoryPrefabList[Index].bankNameTxt.text;
        accountHolderNameInputfield.text = bankHistoryPrefabList[Index].AccountHolderNameTxt.text;
        accountNumberInputfield.text = bankHistoryPrefabList[Index].AccountNumberTxt.text;
        accountHolderNameInputfield.text = bankHistoryPrefabList[Index].AccountHolderNameTxt.text;
        IFSCInputfield.text = bankHistoryPrefabList[Index].AccountIFSCCode.text;
    }


    public IEnumerator WithDrawAmountApiBank()
    {
        WWWForm form = new WWWForm();
        form.AddField("amount", amountInputfield.text);
        form.AddField("bankName", bankNameInputfield.text);
        form.AddField("IFSC", IFSCInputfield.text);
        form.AddField("accountHolderName", accountHolderNameInputfield.text);
        form.AddField("accountNumber", accountNumberInputfield.text);

        using (var profileApi = UnityWebRequest.Post(StaticData.baseURL + StaticData.withdrawAmout, form))
        {
            profileApi.SetRequestHeader("Authorization", GameManager.instance.token);

            yield return profileApi.SendWebRequest();

            if (profileApi.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found : " + profileApi.error);
            }
            else
            {
                Debug.Log(profileApi.downloadHandler.text);
                Debug.Log("Bank Update Form Upload Complate");

                mainUserWithdraw = JsonUtility.FromJson<MainUserWithdraw>(profileApi.downloadHandler.text);
                amountInputfield.text = "";
                bankNameInputfield.text = "";
                IFSCInputfield.text = "";
                accountHolderNameInputfield.text = "";
                accountNumberInputfield.text = "";
            }
        }
    }



    public IEnumerator addBankAccount()
    {
        SendDataAddBank sendDataAddBank = new();
        sendDataAddBank.bankName = AddbankNameInputfield.text;
        sendDataAddBank.accountNumber = AddaccountNumberInputfield.text;
        sendDataAddBank.IFSC = AddIFSCInputfield.text;
        sendDataAddBank.accountHolderName = AddaccountHolderNameInputfield.text;

        string jsonData = JsonUtility.ToJson(sendDataAddBank);

        UnityWebRequest request = new UnityWebRequest(StaticData.baseURL + StaticData.AddBank, "POST");

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
            MainAddBank = JsonUtility.FromJson<MainAddBank>(request.downloadHandler.text);
            AddbankNameInputfield.text = "";
            AddaccountNumberInputfield.text = "";
            AddIFSCInputfield.text = "";
            AddaccountHolderNameInputfield.text = "";
            GetBankData();
        }
    }







    public IEnumerator updateBankAccount(string accountNumber, string BankId, string bankName, string IFSC, string accountHolderName)
    {
        WWWForm wwwform = new WWWForm();

        Debug.Log("BANK ID : " + BankId);

        wwwform.AddField("bankId", BankId);
        wwwform.AddField("bankName", bankName);
        wwwform.AddField("IFSC", IFSC);
        wwwform.AddField("accountNumber", accountNumber);
        wwwform.AddField("accountHolderName", accountHolderName);
        Debug.Log("break" + BankId);
        using (UnityWebRequest request = UnityWebRequest.Post(StaticData.baseURL + StaticData.UpdateBank, wwwform))
        {
            request.SetRequestHeader("Authorization", GameManager.instance.token);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                print("Error: " + request.error);
            }
            else
            {
                print("Response: " + request.downloadHandler.text);
                PlayerPrefs.SetString("accountnumber", accountNumber);
                PlayerPrefs.SetString("bankName", bankName);
                PlayerPrefs.SetString("IFSC", IFSC);
                PlayerPrefs.SetString("acountholdename", name);
            }
        }
    }

    public IEnumerator GetBankHis()
    {
        WWWForm wwwform = new WWWForm();

        Debug.Log("Bank History CAllles");

        using (UnityWebRequest request = UnityWebRequest.Post(StaticData.baseURL + StaticData.GetBankHistory, wwwform))
        {
            request.SetRequestHeader("Authorization", GameManager.instance.token);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                print("Error: " + request.error);
            }
            else
            {
                print("Response: " + request.downloadHandler.text);
                MainGetBank = JsonUtility.FromJson<MainGetBank>(request.downloadHandler.text);
                int GenerateCount = MainGetBank.data.docs.Count;
                GenerateBankHistory(GenerateCount);
            }
        }
    }
    internal bool isBank = true;
    internal bool isUpi = false;

    public void OnClickToChange(string name)
    {
        switch(name)
        {
            case "bank":
                UPIInputField.text = "";
                amountInputfield.text = "";
                BankSectionBoard.SetActive(true);
                UPISectionBoard.SetActive(false);
                break;

            case "upi":
                amountInputfield.text = "";
                bankNameInputfield.text = "";
                IFSCInputfield.text = "";
                accountHolderNameInputfield.text = "";
                accountNumberInputfield.text = "";
                BankSectionBoard.SetActive(false);
                UPISectionBoard.SetActive(true);
                break;
        }
    }
}

[System.Serializable]
public class WithdrawData
{
    public string _id;
    public string phoneNumber;
    public int amount;
    public string referralCode;
    public bool isActive;
    public bool isAllowNotifications;
    public string socketId;
}

[System.Serializable]
public class MainUserWithdraw
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public WithdrawData data;
}


#region Add bank
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class AddBankData
{
    public string userId;
    public string bankName;
    public string IFSC;
    public string accountHolderName;
    public string accountNumber;
    public string _id;
    public string createdAt;
    public string updatedAt;
    public string id;
}

public class MainAddBank
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public AddBankData data;
}

[System.Serializable]
public class SendDataAddBank
{
    public string bankName;
    public string accountNumber;
    public string IFSC;
    public string accountHolderName;
}

#endregion



#region Get Bank data

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[System.Serializable]
public class MainGetbankData
{
    public List<BakDataList> docs;
    public int totalDocs;
    public int offset;
    public int limit;
    public int totalPages;
    public int page;
    public int pagingCounter;
    public bool hasPrevPage;
    public bool hasNextPage;
    public object prevPage;
    public object nextPage;
}

[System.Serializable]
public class BakDataList
{
    public string _id;
    public string userId;
    public string bankName;
    public string IFSC;
    public string accountHolderName;
    public string accountNumber;
    public string createdAt;
    public string updatedAt;
    public string id;
}

[System.Serializable]
public class MainGetBank
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public MainGetbankData data;
}


#endregion

