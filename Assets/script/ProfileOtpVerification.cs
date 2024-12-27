using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileOtpVerification : MonoBehaviour
{
    public TextMeshProUGUI MobileNumberText;
    public InputField OtpInf;

    private void OnEnable()
    {
        NewUIManager.instance.isOtp = true;
    }

    private void OnDisable()
    {
        NewUIManager.instance.isOtp = false;
    }
}
