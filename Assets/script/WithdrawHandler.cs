using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class WithdrawHandler : MonoBehaviour
{
    public static WithdrawHandler instance;

    public InputField amountInputfield, bankNameInputfield, IFSCInputfield, accountHolderNameInputfield, accountNumberInputfield;

    public MainUserWithdraw mainUserWithdraw;

    public MainAddBank MainAddBank;
    public MainGetBank MainGetBank;
    public MainAddUPI MainAddUPI;
    public MainUPiHIstory MainUPiHIstory;

    public GameObject addBankPanel;
    public GameObject BankListPanel;
    public GameObject AddUPiPanel;
    public GameObject UPIListPanel;
    public GameObject AddBankButton;
    public GameObject AddUPIButton;
    public GameObject updateBankButton;
    public GameObject updateupiButton;
    public GameObject UpdateBankTextObject;
    public GameObject UpdateUPiTextObject;


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

    [Header("Add Upi Section")]
    public InputField AddUPIInputField;

    [Header("UPI HIstory Section")]
    public UPIHistoryPrefab UPIHistoryPrefab;
    public Transform UpiContent;
    public List<UPIHistoryPrefab> UPIHistoryPrefabList = new();

    public GameObject BankSectionBoard, UPISectionBoard;


    public GameObject AddBankUpiPopUp;


    public bool isUpdate;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        GetBankData();
        GetUPIData();

        isBank = true;
    }

    private void Start()
    {

        DOVirtual.DelayedCall(.5f, delegate { checkForBankAndUPi(); });
    }

    private void OnDisable()
    {
        //isUpdate = false;

        addBankPanel.SetActive(false);
        BankListPanel.SetActive(false);
        UPISectionBoard.SetActive(false);
        BankSectionBoard.SetActive(true);
        AddUPiPanel.SetActive(false);
        UPIListPanel.SetActive(false);
        updateBankButton.SetActive(false);
        updateupiButton.SetActive(false);

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
        AddUPIInputField.text = "";
    }

    private void Update()
    {
        if (isUpdate)
        {
            updateBankButton.SetActive(true);
            updateupiButton.SetActive(true);
        }
    }

    public void checkForBankAndUPi()
    {
        int count = MainGetBank.data.docs.Count;
        int count1 = MainUPiHIstory.data.docs.Count;


        Debug.Log(count + "  ---------- " + count1);

        if (count >= 2)
        {
            bankNameInputfield.text = MainGetBank.data.docs[0].bankName;
            accountHolderNameInputfield.text = MainGetBank.data.docs[0].accountHolderName;
            accountNumberInputfield.text = MainGetBank.data.docs[0].accountNumber;
            IFSCInputfield.text = MainGetBank.data.docs[0].IFSC;
            paymentID = MainGetBank.data.docs[0].id;
        }
        else if (count1 >= 1)
        {
            OnClickToChange("upi");
            UPIInputField.text = MainUPiHIstory.data.docs[0].upiId;
            paymentID = MainUPiHIstory.data.docs[0].id;
        }
        else
        {
           
            AddBankUpiPopUp.SetActive(true);
            DOVirtual.DelayedCall(3f, delegate { AddBankUpiPopUp.SetActive(false); });
        }
    }

    public void WithdrawAmount()
    {
        if (amountInputfield.text.Length >= 3 && bankNameInputfield.text.Length >= 2 && IFSCInputfield.text.Length >= 2 && accountHolderNameInputfield.text.Length >= 2 && accountNumberInputfield.text.Length >= 2 && isBank && !string.IsNullOrEmpty(paymentID))
        {
            StartCoroutine(WithDrawAmountApi());

        }
        else if (amountInputfield.text.Length >= 3 && UPIInputField.text.Length >= 5 && isUpi && !string.IsNullOrEmpty(paymentID))
        {
            StartCoroutine(WithDrawAmountApi());
        }
        else
        {
            NewUIManager.instance.InformationPopUp.NoticeText.text = "Enter Valid Details";
            NewUIManager.instance.InformationPopUp.gameObject.SetActive(true);
        }
    }
    public void AddBankData()
    {
        if (AddbankNameInputfield.text.Length >= 2 && AddIFSCInputfield.text.Length >= 2 && AddaccountHolderNameInputfield.text.Length >= 2 && AddaccountNumberInputfield.text.Length >= 2)
            StartCoroutine(addBankAccount());
    }
    public void GetBankData()
    {
        StartCoroutine(GetBankHis());
    }


    public void AddUPIData()
    {
        if (AddUPIInputField.text.Length >= 5)
            StartCoroutine(addUPI());
    }
    public void GetUPIData()
    {
        StartCoroutine(GetUPIHis());
    }

    public void UpdateBankAccountDetails()
    {
        if (AddbankNameInputfield.text.Length >= 2 && AddIFSCInputfield.text.Length >= 2 && AddaccountHolderNameInputfield.text.Length >= 2 && AddaccountNumberInputfield.text.Length >= 2 && !string.IsNullOrEmpty(updateBankid))
            StartCoroutine(updateBankAccount());
    }

    public void UpdateUpiAccountDetails()
    {
        if (AddUPIInputField.text.Length >= 5 && !string.IsNullOrEmpty(UpdateUPIId))
            StartCoroutine(updateUPiAccount());
    }


    public void ClearBankHistory()
    {
        for (int i = 0; i < HistoryParent.transform.childCount; i++)
        {
            Destroy(HistoryParent.transform.GetChild(i).gameObject);
        }
        bankHistoryPrefabList.Clear();
    }

    public void ClearUPIHistory()
    {
        for (int i = 0; i < UpiContent.transform.childCount; i++)
        {
            Destroy(UpiContent.transform.GetChild(i).gameObject);
        }
        UPIHistoryPrefabList.Clear();
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

            bankHistoryPrefabClone.SetBankPrefabData(bankname, Accountnumber, AccountHolderName, AccountIFSCCode, Index);
        }
    }

    public void GenerateUPIHistory(int count)
    {
        ClearUPIHistory();
        for (int i = 0; i < count; i++)
        {
            int Index = i;
            UPIHistoryPrefab UPIHistoryPrefabClone = Instantiate(UPIHistoryPrefab, UpiContent);
            UPIHistoryPrefabClone.GetComponent<Button>().onClick.AddListener(delegate
            {
                OnClickUpiHisotyButton(Index);
            });
            UPIHistoryPrefabList.Add(UPIHistoryPrefabClone);
            string upiid = MainUPiHIstory.data.docs[i].upiId;
            UPIHistoryPrefabClone.SetData(upiid, Index);
        }
    }

    public void OnClickBanHistoryButton(int Index)
    {
        if (!isUpdate)
        {
            NewUIManager.instance.OpenPanel(Panel.Withdrwal);
            OnClickToChange("bank");
            bankNameInputfield.text = bankHistoryPrefabList[Index].bankNameTxt.text;
            accountHolderNameInputfield.text = bankHistoryPrefabList[Index].AccountHolderNameTxt.text;
            accountNumberInputfield.text = bankHistoryPrefabList[Index].AccountNumberTxt.text;
            IFSCInputfield.text = bankHistoryPrefabList[Index].AccountIFSCCode.text;
            paymentID = MainGetBank.data.docs[Index].id;

        }
        else
        {
            //isUpdate = false;
            NewUIManager.instance.OpenPanel(Panel.AddBank);
            AddbankNameInputfield.text = bankHistoryPrefabList[Index].bankNameTxt.text;
            AddaccountHolderNameInputfield.text = bankHistoryPrefabList[Index].AccountHolderNameTxt.text;
            AddaccountNumberInputfield.text = bankHistoryPrefabList[Index].AccountNumberTxt.text;
            AddaccountHolderNameInputfield.text = bankHistoryPrefabList[Index].AccountHolderNameTxt.text;
            AddIFSCInputfield.text = bankHistoryPrefabList[Index].AccountIFSCCode.text;
            updateBankid = MainGetBank.data.docs[Index].id;
        }
    }

    public void OnClickUpiHisotyButton(int Index)
    {
        if (!isUpdate)
        {
            NewUIManager.instance.OpenPanel(Panel.Withdrwal);
            OnClickToChange("upi");
            UPIInputField.text = UPIHistoryPrefabList[Index].UPiID.text;
            paymentID = MainUPiHIstory.data.docs[Index].id;
        }
        else
        {
            //isUpdate = false;
            NewUIManager.instance.OpenPanel(Panel.AddUPI);
            AddUPIInputField.text = MainUPiHIstory.data.docs[Index].upiId;
            updateupiButton.SetActive(true);
            UpdateUPIId = MainUPiHIstory.data.docs[Index].id;
        }
    }

    public IEnumerator OffUodateText(GameObject Obj)
    {
        yield return new WaitForSeconds(2f);
        Obj.SetActive(false);
    }

    public void OnclickUpdate(string name)
    {
        isUpdate = true;

        switch (name)
        {
            case "bank":
                UpdateBankTextObject.SetActive(true);
                StartCoroutine(OffUodateText(UpdateBankTextObject));
                updateBankButton.SetActive(true);
                Debug.Log("------------ From Here ------------------");
                break;

            case "upi":
                UpdateUPiTextObject.SetActive(true);
                StartCoroutine(OffUodateText(UpdateUPiTextObject));
                updateupiButton.SetActive(true);
                Debug.Log("------------ From Here ------------------");
                break;
        }
    }


    internal string paymentID;

    public IEnumerator WithDrawAmountApi()
    {
        WWWForm form = new WWWForm();
        form.AddField("amount", amountInputfield.text);
        form.AddField("userPaymentId", paymentID);

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
                paymentID = "";
                NewUIManager.instance.OpenPanel(Panel.Withdrwal);
                NewUIManager.instance.InformationPopUp.NoticeText.text = "Withdrwal Request Sent.";
                NewUIManager.instance.InformationPopUp.gameObject.SetActive(true);
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
            NewUIManager.instance.InformationPopUp.NoticeText.text = "Bank Add";
            NewUIManager.instance.InformationPopUp.gameObject.SetActive(true);
        }
    }




    public string updateBankid;


    public IEnumerator updateBankAccount()
    {
        WWWForm wwwform = new WWWForm();

        Debug.Log("BANK ID : " + updateBankid);

        wwwform.AddField("bankId", updateBankid);
        wwwform.AddField("bankName", AddbankNameInputfield.text);
        wwwform.AddField("IFSC", AddIFSCInputfield.text);
        wwwform.AddField("accountNumber", AddaccountNumberInputfield.text);
        wwwform.AddField("accountHolderName", AddaccountHolderNameInputfield.text);
        //Debug.Log("break" + BankId);
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
                isUpdate = false;
                NewUIManager.instance.InformationPopUp.NoticeText.text = "Bank Update";
                NewUIManager.instance.InformationPopUp.gameObject.SetActive(true);
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
        switch (name)
        {
            case "bank":
                UPIInputField.text = "";
                amountInputfield.text = "";
                BankSectionBoard.SetActive(true);
                UPISectionBoard.SetActive(false);
                AddBankButton.SetActive(true);
                AddUPIButton.SetActive(false);
                isBank = true;
                isUpi = false;
                break;

            case "upi":
                amountInputfield.text = "";
                bankNameInputfield.text = "";
                IFSCInputfield.text = "";
                accountHolderNameInputfield.text = "";
                accountNumberInputfield.text = "";
                BankSectionBoard.SetActive(false);
                UPISectionBoard.SetActive(true);
                AddBankButton.SetActive(false);
                AddUPIButton.SetActive(true);
                isBank = false;
                isUpi = true;
                break;
        }
    }





    public IEnumerator addUPI()
    {
        SendUPIData sendUPIData = new();
        sendUPIData.upiId = AddUPIInputField.text;

        string jsonData = JsonUtility.ToJson(sendUPIData);

        UnityWebRequest request = new UnityWebRequest(StaticData.baseURL + StaticData.AddUPI, "POST");

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
            MainAddUPI = JsonUtility.FromJson<MainAddUPI>(request.downloadHandler.text);
            GetUPIData();
            AddUPIInputField.text = "";
            NewUIManager.instance.InformationPopUp.NoticeText.text = "UPI Add";
            NewUIManager.instance.InformationPopUp.gameObject.SetActive(true);
        }
    }

    public string UpdateUPIId;

    public IEnumerator updateUPiAccount()
    {
        WWWForm wwwform = new WWWForm();

        wwwform.AddField("upiId", AddUPIInputField.text);
        wwwform.AddField("updateId", UpdateUPIId);

        using (UnityWebRequest request = UnityWebRequest.Post(StaticData.baseURL + StaticData.UpdateUPI, wwwform))
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
                isUpdate = false;
                AddUPIInputField.text = "";


                NewUIManager.instance.InformationPopUp.NoticeText.text = "UPI Update";
                NewUIManager.instance.InformationPopUp.gameObject.SetActive(true);
            }
        }
    }

    public IEnumerator GetUPIHis()
    {
        WWWForm wwwform = new WWWForm();

        Debug.Log("UPi History CAllles");

        using (UnityWebRequest request = UnityWebRequest.Post(StaticData.baseURL + StaticData.GetUPIHistory, wwwform))
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
                MainUPiHIstory = JsonUtility.FromJson<MainUPiHIstory>(request.downloadHandler.text);
                int GenerateCount = MainUPiHIstory.data.docs.Count;
                GenerateUPIHistory(GenerateCount);
            }
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

public class SendUPIData
{
    public string upiId;
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



#region Add UPi
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
[System.Serializable]
public class MainAddUPI
{
    public string userId;
    public string type;
    public string _id;
    public DateTime createdAt;
    public DateTime updatedAt;
    public string id;
}
[System.Serializable]
public class AddUPIData
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public MainAddUPI data;
}
#endregion


#region Get UPI History
[System.Serializable]
public class UPIHistory
{
    public List<UPIIDList> docs;
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
public class UPIIDList
{
    public string _id;
    public string userId;
    public string type;
    public string upiId;
    public DateTime createdAt;
    public DateTime updatedAt;
    public string id;
}
[System.Serializable]
public class MainUPiHIstory
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public UPIHistory data;
}

#endregion