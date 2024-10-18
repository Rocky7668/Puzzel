using UnityEngine;
using UnityEngine.UI;

public class WinObjHandler : MonoBehaviour
{
    public Text userNameTxt, userIdTxt;
    public Image userImg;

    public void WinDataSet(Sprite userSprite, string userName, string userId)
    {
        userImg.sprite = userSprite;
        userNameTxt.text = userName;
        userIdTxt.text = userId;
    }
}
