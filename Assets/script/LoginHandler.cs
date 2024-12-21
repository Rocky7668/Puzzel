using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginHandler : MonoBehaviour
{
    public static LoginHandler instance;

    public InputField registerMailInputfield, registerPasswordInputfield, otpInputfield;
    public InputField loginMailInputfield, loginPasswordInputfield;

    public Text otpTxt;
    public TextMeshProUGUI OtpPageMobileNumeber;
    public TextMeshProUGUI termsTxt;

    public LoginResponse loginData;
    public SendOTPRes sendOTPRes;
    public VerifyOtpRes verifyOtpRes;

    public GameObject registerPanel, loginPanel, otpPanel;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
    }

    public void OnEnable()
    {
        Application.runInBackground = true;
    }

    public void LoginBtnClick()
    {
        //string email = loginMailInputfield.text;
        string email = "gaurav@gmail.com";
        //string pass = loginPasswordInputfield.text;
        string pass = "123456";

        bool isValid = IsValidEmail(loginMailInputfield.text);
        //if (isValid)
        {
            LoginMain(email, pass);
        }
        //else
        {
            Debug.Log("Please Enter Valid Email");
        }
    }

    public void LoginMain(string phone, string pass, bool isFirst = true)
    {
        StartCoroutine(RequestWithPost(phone, pass, isFirst));
    }

    public IEnumerator RequestWithPost(string phone, string password, bool isFirst = true)
    {
        WWWForm form = new WWWForm();
        form.AddField("phoneNumber", phone);
        form.AddField("password", password);

        using (UnityWebRequest api = UnityWebRequest.Post(StaticData.baseURL + StaticData.userLogin, form))
        {
            yield return api.SendWebRequest();
            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log(api.downloadHandler.text);
                loginData = JsonUtility.FromJson<LoginResponse>(api.downloadHandler.text);
                Debug.Log("Login Successfull");
                if (isFirst) SceneManager.LoadScene(1);
                //SocketConnection.instance.StartSocketConnection();
                PlayerPrefs.SetString("login", "true");
                PlayerPrefs.SetString("email", phone);
                PlayerPrefs.SetString("password", password);

            }
        }
    }

    IEnumerator PostRegister(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        Debug.Log("Email : " + email);
        Debug.Log("Pass : " + password);

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.userRegister, form))
        {
            Debug.Log("NEtwork : " + api.result);
            yield return api.SendWebRequest();
            Debug.Log("Http : " + api.error);
            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log(api.downloadHandler.text);
                //SliderScript.Instance.otpPanelNumTxt.text = 
                Debug.Log("User Email : " + email);
                Debug.Log("User Password : " + password);

                registerPanel.SetActive(false);
                loginPanel.SetActive(true);
                //JSONNode data = JSONNode.Parse (api.downloadHandler.text);


            }
        }
    }

    public void VerifyOTPMain()
    {
        StartCoroutine(VerifyOTP(registerMailInputfield.text, otpInputfield.text));
    }

    IEnumerator VerifyOTP(string phone, string otp)
    {
        WWWForm form = new WWWForm();
        form.AddField("phoneNumber", phone);
        form.AddField("otp", otp);

        Debug.Log("phone : " + phone);

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.verifyOtp, form))
        {
            Debug.Log("NEtwork : " + api.result);
            yield return api.SendWebRequest();
            Debug.Log("Http : " + api.downloadHandler.text);
            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log(api.downloadHandler.text);
                verifyOtpRes = JsonUtility.FromJson<VerifyOtpRes>(api.downloadHandler.text);
                otpTxt.text = sendOTPRes.data.otp;
                PlayerPrefs.SetString("phone", phone);  
                PlayerPrefs.SetString("token", verifyOtpRes.data.tokenData.token);
                Debug.Log("Token ------ " + verifyOtpRes.data.tokenData.token);
                Debug.Log("User Phone : " + phone);
                loginData.data.tokenData.token = verifyOtpRes.data.tokenData.token;
                if (verifyOtpRes.success)
                    SceneManager.LoadScene(1);

            }
        }
    }

    public void SendOTPMain()
    {
        OtpPageMobileNumeber.text = registerMailInputfield.text;
        StartCoroutine(SendOTP(registerMailInputfield.text));
    }

    [System.Serializable]
    public class SendOptData
    {
        public string phoneNumber;
    }

    IEnumerator SendOTP(string phone)
    {
        /*WWWForm form = new WWWForm();
        form.AddField("phoneNumber", phone);

        Debug.Log("phone : " + phone);

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.sendOTP, form))
        {
            Debug.Log("NEtwork : " + api.result);
            yield return api.SendWebRequest();
            Debug.Log("Http : " + api.downloadHandler.text);
            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log(api.downloadHandler.text);
                sendOTPRes = JsonUtility.FromJson<SendOTPRes>(api.downloadHandler.text);
                otpTxt.text = sendOTPRes.data.otp;
                PlayerPrefs.SetString("phone", phone);
                Debug.Log("User Phone : " + phone);
                registerPanel.SetActive(false);
                otpPanel.SetActive(true);

            }
        }*/
        WWWForm form = new WWWForm();
        SendOptData update = new();

        update.phoneNumber = registerMailInputfield.text;

        string jsonData = JsonUtility.ToJson(update);
        Debug.Log("Update json ---------  " + jsonData);



        // Create a UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(StaticData.baseURL + StaticData.sendOTP, "POST");

        // Convert JSON string to byte array
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Attach raw data to the request
        request.uploadHandler = new UploadHandlerRaw(rawData);

        // Set response handler
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("Authorization", GameManager.instance.token);

        //UploadHandlerRaw    

        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            sendOTPRes = JsonUtility.FromJson<SendOTPRes>(request.downloadHandler.text);
            otpTxt.text = sendOTPRes.data.otp;
            PlayerPrefs.SetString("phone", phone);
            Debug.Log("User Phone : " + phone);
            registerPanel.SetActive(false);
            otpPanel.SetActive(true);
        }
    }

    public void GetProfileMain()
    {
        StartCoroutine(GetProfile());
    }

    IEnumerator GetProfile()
    {
        WWWForm form = new WWWForm();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.getProfile, form))
        {
            Debug.Log("NEtwork : " + api.result);
            api.SetRequestHeader("token", PlayerPrefs.GetString("token"));
            yield return api.SendWebRequest();
            Debug.Log("Http : " + api.downloadHandler.text);
            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log(api.downloadHandler.text);
                //sendOTPRes = JsonUtility.FromJson<SendOTPRes>(api.downloadHandler.text);
                //otpTxt.text = sendOTPRes.data.otp;
                //PlayerPrefs.SetString("phone", phone);
                //Debug.Log("User Phone : " + phone);
                //registerPanel.SetActive(false);
                //otpPanel.SetActive(true);

            }
        }

    }

    public void OnclickRegister()
    {
        bool isValid = IsValidEmail(registerMailInputfield.text);
        if (isValid)
        {
            StartCoroutine(PostRegister(registerMailInputfield.text, registerPasswordInputfield.text));
        }
        else
        {
            Debug.Log("Please Enter Valid Email");
        }

    }

    public static bool IsValidEmail(string email)
    {
        // Regular expression pattern for a valid email address
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        // Create Regex instance
        Regex regex = new Regex(pattern);

        // Check if the email matches the pattern
        return regex.IsMatch(email);
    }
}


#region Login
[Serializable]
public class LoginResponseData
{
    public User user;
    public TokenData tokenData;
}

[Serializable]
public class LoginResponse
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public LoginResponseData data;
}

[Serializable]
public class TokenData
{
    public string token;
}

[Serializable]
public class User
{
    public int amount;
    public string role;
    public string email;
    public string password;
    public string userPassword;
    public bool isBlock;
    public string referralCode;
    public string _id;
    public DateTime lastActivateAt;
    public DateTime createdAt;
    public DateTime updatedAt;
}
#endregion

#region OTPSEND
[System.Serializable]
public class SendOTPResData
{
    public string phoneNumber;
    public string otp;
    public DateTime expireAt;
    public string _id;
    public DateTime createdAt;
    public DateTime updatedAt;
}

[System.Serializable]
public class SendOTPRes
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public SendOTPResData data;
}


#endregion

#region VerifyOTP
[System.Serializable]
public class VerifyOtpResData
{
    public Users user;
    public TokenDatas tokenData;
}

[System.Serializable]
public class VerifyOtpRes
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public VerifyOtpResData data;
}

[System.Serializable]
public class TokenDatas
{
    public string token;
}

[System.Serializable]
public class Users
{
    public string phoneNumber;
    public int amount;
    public string role;
    public bool isBlock;
    public string referralCode;
    public string _id;
    public DateTime lastActivateAt;
    public DateTime createdAt;
    public DateTime updatedAt;
}


#endregion
