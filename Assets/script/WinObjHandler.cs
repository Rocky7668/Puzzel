using UnityEngine;
using UnityEngine.UI;

public class WinObjHandler : MonoBehaviour
{
    public Text userWinningTxt, userIdTxt;
    public Image userImg;

    public void WinDataSet(Sprite userSprite, string userwinning, string userId)
    {
        userImg.sprite = userSprite;
        userWinningTxt.text = "₹" + userwinning;
        userIdTxt.text = userId;
    }
}
