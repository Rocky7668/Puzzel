using System;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;
using Task = System.Threading.Tasks.Task;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SliderScript : MonoBehaviour
{
    public static SliderScript Instance;
    [SerializeField] private Slider fillImage;
    public GameObject login, register, otp, term, splash;
    //[SerializeField] private Text loadingText;

    private int _percentage;

    private float speed = 0.009f;

    public TermsConditionRes termsConditionRes;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        FillBar();
    }

    private async void FillBar()
    {
        while (true)
        {
            //float value add
            fillImage.value += speed;
            _percentage = (int)(fillImage.value * 100);
            //loadingText.text = _percentage + "%";

            if (Math.Abs(fillImage.value - 1f) < 0.001f)
            {
                await Task.Delay(1300);
                Debug.Log("Fill Complete");
                if (PlayerPrefs.HasKey("token"))
                {
                    //ProfileHandler.instance.ProfileDataSet();
                    SceneManager.LoadScene(1);
                }
                else
                    StartCoroutine(RequestTermsAndCondition());
                //if (!PlayerPrefs.HasKey("login"))
                //{
                //    login.SetActive(true);
                //}
                //else
                //{
                //    string email = PlayerPrefs.GetString("email");
                //    string password = PlayerPrefs.GetString("password");
                //    Debug.Log("Email : " + email);
                //    Debug.Log("PassWord : " + password);
                //    StartCoroutine(LoginHandler.instance.RequestWithPost(email, password));
                //}
                //splash.SetActive(false);
                //if (!PlayerPrefs.HasKey("phone"))
                break;
            }
            await Task.Yield();
        }

    }

    public IEnumerator RequestTermsAndCondition()
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest api = UnityWebRequest.Post(StaticData.baseURL + StaticData.getTermsAndCondition, form))
        {
            yield return api.SendWebRequest();
            if (api.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Data Not Found");
            }
            else
            {
                Debug.Log(api.downloadHandler.text);
                termsConditionRes = JsonUtility.FromJson<TermsConditionRes>(api.downloadHandler.text);
                if (termsConditionRes.data.Count > 0)
                    LoginHandler.instance.termsTxt.text = termsConditionRes.data[0].termsAndConditions;
                term.SetActive(true);
            }
        }
    }

    public void LetsPlayClick()
    {
        login.SetActive(false);
        otp.SetActive(true);
    }
    public void loginBack()
    {
        Application.Quit();
    }
    public void VerifyClick()
    {
        otp.SetActive(false);
        term.SetActive(true);
    }
    public void otpBack()
    {
        register.SetActive(true);
        otp.SetActive(false);
    }
    public void ConfirmClick()
    {
        term.SetActive(false);
        register.SetActive(true);
        //SceneManager.LoadScene(1);
    }
    public void termback()
    {
        otp.SetActive(true);
        term.SetActive(false);
    }

    public void RegisterBtn()
    {
        register.SetActive(true);
        login.SetActive(false);
    }

    public void LoginBtn()
    {
        register.SetActive(false);
        login.SetActive(true);
    }

    private void OnLoadingFinished()
    {

        // if (PlayerPrefs.HasKey(PlayerPrefsNames.LoggedInPrefs) == false)
        {
            //open game play
            // UIManager.Instance.OpenAuthenticationScreen();
        }
        // else
        {
            //or login open
            // AuthenticationManager.Instance.Login();
            //  UIManager.Instance.OpenHomeScreen(); // For Testing Game Play.
        }



    }
}


#region TermsAndCondition
[Serializable]
public class TermsConditionResData
{
    public string _id;
    public string termsAndConditions;
    public DateTime createdAt;
    public DateTime updatedAt;
    public string id;
}

[Serializable]
public class TermsConditionRes
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public List<TermsConditionResData> data;
}
#endregion
