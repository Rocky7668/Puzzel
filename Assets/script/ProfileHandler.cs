using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProfileHandler : MonoBehaviour
{
    public static ProfileHandler instance;


    public Text userNameTxt, userIdTxt, userMobilenumber, WalletPopuUpAmount, emailTxt;
    public Slider pointSlider;

    public TextMeshProUGUI amtTxt;

    public ProfileRes profileRes;
    public NotificationRes notificationRes;
    public MainUpdateProfile MainUpdateProfile;
    public MainUserNameUpdateData MainUserNameUpdateData;
    public MainUpdateEmailData MainUpdateEmailData;

    [Header("===== Notification =====")]
    [SerializeField] private Transform notificationGenerator;
    [SerializeField] private NotificationPrefabHandler notificationPrefabHandler;
    [SerializeField] private List<NotificationPrefabHandler> notificationPrefabHandlers;

    public Text RefferalCodeText;

    public Text FullnameTxt;
    public Text DOBTxt;
    public Text PincodeTxt;
    public Text LocationTxt;

    public InputField FullnameTxtIF;
    public InputField DOBTxtIF;
    public InputField PincodeTxtIF;
    public InputField LocationTxtIF;

    public GameObject FirstUpdateButton;
    public GameObject SecondUpdateButton;

    public GameObject FirstUpdateAccountButton;
    public GameObject SecondUpdateAccountButton;


    public Image EmailImage;
    public Sprite emailPending;
    public Sprite emailVerified;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        ProfileDataSet();
    }

    public void ClickUpdateProfile(bool isTrue)
    {
        FullnameTxtIF.gameObject.SetActive(isTrue);
        DOBTxtIF.gameObject.SetActive(isTrue);
        PincodeTxtIF.gameObject.SetActive(isTrue);
        LocationTxtIF.gameObject.SetActive(isTrue);
        SecondUpdateButton.SetActive(isTrue);
        FirstUpdateButton.SetActive(!isTrue);
    }

    public void ClickUpdateAccountDetails(bool isTrue)
    {
        UpdateUsernameINF.gameObject.SetActive(isTrue);
        UpdateEmailINF.gameObject.SetActive(isTrue);
        SecondUpdateAccountButton.SetActive(isTrue);
        FirstUpdateAccountButton.SetActive(!isTrue);
    }

    IEnumerator DashBoradAmount()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            WalletPopuUpAmount.text = "₹ " + profileRes.data.amount.ToString("F2");
            string Amount = FormatMoney((float)profileRes.data.amount);
            amtTxt.text = Amount;
        }
    }
    public void ProfileDataSet()
    {
        StartCoroutine(PostProfile());
    }



    private static readonly string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return Regex.IsMatch(email, emailPattern);
    }

    public void OnClickUpdateAccountDetails()
    {
        UpdateUsername();
        UpdateEmail();
    }

    public void UpdateUsername()
    {
        if (UpdateUsernameINF.text.Length >= 4)
            StartCoroutine(UpdateUsernameData());
    }
    public void UpdateEmail()
    {
        if (IsValidEmail(UpdateEmailINF.text))
            StartCoroutine(UpdateEmailData());
    }

    public void OnclickUpdateProfile()
    {
        if (FullnameTxtIF.text.Length >= 5 && DOBTxtIF.text.Length == 10 && PincodeTxtIF.text.Length >= 4 && LocationTxtIF.text.Length >= 4)
            StartCoroutine(updateProfile());
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
                userNameTxt.text = profileRes.data.userName;
                userIdTxt.text = "#" + profileRes.data._id;
                pointSlider.maxValue = profileRes.data.totalGamePlayed;
                pointSlider.value = profileRes.data.totalWinGame;
                WalletPopuUpAmount.text = "₹ " + profileRes.data.amount.ToString("F2");
                string Amount = FormatMoney((float)profileRes.data.amount);
                amtTxt.text = Amount;
                userMobilenumber.text = profileRes.data.phoneNumber;
                emailTxt.text = profileRes.data.email;

                RefferalCodeText.text = profileRes.data.referralCode;
                StaticData.TotalBalance = profileRes.data.amount;
                SetPersonalData();
                StartCoroutine(DashBoradAmount());

                if (emailTxt.text.Length >= 5)
                    EmailImage.sprite = emailVerified;
                else
                    EmailImage.sprite = emailPending;
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
                Debug.Log("Error : " + api.error);
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

    IEnumerator updateProfile()
    {
        WWWForm form = new WWWForm();
        SendUpdateProfile SendUpdateProfile = new();
        SendUpdateProfile.name = FullnameTxtIF.text;
        SendUpdateProfile.dateOfBirth = DOBTxtIF.text;
        SendUpdateProfile.pincode = PincodeTxtIF.text;
        SendUpdateProfile.location = LocationTxtIF.text;


        string jsonData = JsonUtility.ToJson(SendUpdateProfile);
        Debug.Log("Update json ---------  " + jsonData);



        // Create a UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(StaticData.baseURL + StaticData.UpdateProfile, "POST");

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
            MainUpdateProfile = JsonUtility.FromJson<MainUpdateProfile>(request.downloadHandler.text);
            StartCoroutine(PostProfile());
            ClickUpdateProfile(false);
            //SetPersonalData();
        }
    }

    void SetPersonalData()
    {
        FullnameTxt.text = profileRes.data.fullName;
        DOBTxt.text = profileRes.data.dateOfBirth;

        if (profileRes.data.pincode != 0)
            PincodeTxt.text = profileRes.data.pincode.ToString();

        LocationTxt.text = profileRes.data.location;

        FullnameTxtIF.text = "";
        DOBTxtIF.text = "";
        PincodeTxtIF.text = "";
        LocationTxtIF.text = "";
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


    public static string FormatMoney(float number)
    {
        if (number >= 1_000_000_000_000)
        {
            return (number / 1_000_000_000_000f).ToString((number % 1_000_000_000_000 == 0) ? "0" : "0.0") + "t"; // Trillion
        }
        else if (number >= 1_000_000_000)
        {
            return (number / 1_000_000_000f).ToString((number % 1_000_000_000 == 0) ? "0" : "0.0") + "b"; // Billion
        }
        else if (number >= 1_000_000)
        {
            return (number / 1_000_000f).ToString((number % 1_000_000 == 0) ? "0" : "0.0") + "m"; // Million
        }
        else if (number >= 1_000)
        {
            return (number / 1_000f).ToString((number % 1_000 == 0) ? "0" : "0.0") + "k"; // Thousand
        }
        else
        {
            return number.ToString("0"); // No formatting needed
        }
    }

    public InputField UpdateUsernameINF;
    public InputField UpdateEmailINF;

    IEnumerator UpdateUsernameData()
    {
        WWWForm form = new WWWForm();
        UpdateUsernameSendData update = new();
        update.userName = UpdateUsernameINF.text;


        string jsonData = JsonUtility.ToJson(update);
        Debug.Log("Update json ---------  " + jsonData);



        // Create a UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(StaticData.baseURL + StaticData.UpdateUsername, "POST");

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
            MainUserNameUpdateData = JsonUtility.FromJson<MainUserNameUpdateData>(request.downloadHandler.text);
            userNameTxt.text = MainUserNameUpdateData.data.userName;
            StartCoroutine(PostProfile());
            ClickUpdateAccountDetails(false);
        }
    }

    IEnumerator UpdateEmailData()
    {
        WWWForm form = new WWWForm();
        UpdateEmailSendData update = new();
        update.email = UpdateEmailINF.text;


        string jsonData = JsonUtility.ToJson(update);
        Debug.Log("Update json ---------  " + jsonData);



        // Create a UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(StaticData.baseURL + StaticData.UpdateEmail, "POST");

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
            MainUpdateEmailData = JsonUtility.FromJson<MainUpdateEmailData>(request.downloadHandler.text);
            emailTxt.text = MainUpdateEmailData.data.email;
            StartCoroutine(PostProfile());
            ClickUpdateAccountDetails(false);
        }
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
    public bool isActive;
    public bool isAllowNotifications;
    public string notificationToken;
    public string dateOfBirth;
    public string fullName;
    public string location;
    public int pincode;
    public int totalGamePlayed;
    public int totalWinGame;
    public string userName;
    public string email;
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
    public string createdAt;
    public string updatedAt;
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

[System.Serializable]
public class SendUpdateProfile
{
    public string name;
    public string dateOfBirth;
    public string pincode;
    public string location;
}


[System.Serializable]
public class MainUpdateProfileData
{
    public string _id;
    public string phoneNumber;
    public double amount;
    public string role;
    public bool isBlock;
    public string referralCode;
    public bool isActive;
    public bool isAllowNotifications;
    public string notificationToken;
    public string lastActivateAt;
    public string createdAt;
    public string updatedAt;
    public string token;
    public string socketId;
    public string dateOfBirth;
    public string fullName;
    public string location;
    public int pincode;
    public string id;
}

[System.Serializable]
public class MainUpdateProfile
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public MainUpdateProfileData data;
}





[System.Serializable]
public class UserNameUpdateData
{
    public int pincode;
    public string location;
    public string dateOfBirth;
    public string _id;
    public string phoneNumber;
    public double amount;
    public string role;
    public bool isBlock;
    public string referralCode;
    public bool isActive;
    public bool isAllowNotifications;
    public string notificationToken;
    public string lastActivateAt;
    public string createdAt;
    public string updatedAt;
    public string token;
    public string socketId;
    public string userName;
    public string id;
}


[System.Serializable]
public class MainUserNameUpdateData
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public UserNameUpdateData data;
}


[System.Serializable]
public class UpdateEmailData
{
    public int pincode;
    public string location;
    public string dateOfBirth;
    public string _id;
    public string phoneNumber;
    public double amount;
    public string role;
    public bool isBlock;
    public string referralCode;
    public bool isActive;
    public bool isAllowNotifications;
    public string notificationToken;
    public string lastActivateAt;
    public string createdAt;
    public string updatedAt;
    public string token;
    public string socketId;
    public string userName;
    public string email;
    public string id;
}

[System.Serializable]
public class MainUpdateEmailData
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public UpdateEmailData data;
}



[System.Serializable]
public class UpdateUsernameSendData
{
    public string userName;
}

[System.Serializable]
public class UpdateEmailSendData
{
    public string email;
}