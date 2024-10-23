using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WithdrawHandler : MonoBehaviour
{
    public InputField amountInputfield, bankNameInputfield, IFSCInputfield, accountHolderNameInputfield, accountNumberInputfield;

    public MainUserWithdraw mainUserWithdraw;

    public void WithdrawAmount()
    {
        StartCoroutine(BankUpdate());
    }

    public IEnumerator BankUpdate()
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

public class MainUserWithdraw
{
    public string message;
    public string status;
    public int statusCode;
    public bool success;
    public WithdrawData data;
}


