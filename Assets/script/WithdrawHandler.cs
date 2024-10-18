using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class WithdrawHandler : MonoBehaviour
{
    public InputField amountInputfield, bankNameInputfield, IFSCInputfield, accountHolderNameInputfield, accountNumberInputfield;

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
            }
        }
    }
}
