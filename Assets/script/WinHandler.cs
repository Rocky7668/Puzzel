using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinHandler : MonoBehaviour
{
    public List<Sprite> userSprites;
    public List<string> userNames;
    public List<string> userIds;

    public List<WinObjHandler> winObjHandlers;
    public Text oneMinTxt, twoMinTxt, threeMinTxt;

    public WinRes winRes;

    private void OnEnable()
    {
        NewUIManager.instance.ExitPanelsList.Remove(Panel.HelperObject);
        Invoke(nameof(OnCloseWinPage), 8f);
    }

    public void WinningSet(WinRes winResData)
    {

        Debug.Log("WinningSet Funcion called");
        winRes = winResData;
        string first = "Complete 1 minute " + winRes.oneMinutePercentage + "% usrs complete fill color.";
        string second = "Complete 2 minute " + winRes.twoMinutePercentage + "% usrs complete fill color.";
        string third = "Complete 3 minute " + winRes.threeMinutePercentage + "% usrs complete fill color.";

        WinnerDataSet(winRes.topThreeUsers.Count);

        oneMinTxt.text = first;
        twoMinTxt.text = second;
        threeMinTxt.text = third;
    }

    public void WinnerDataSet(int cnt)
    {
        if (cnt == 1)
        {
            int randomNum = Random.Range(0, userSprites.Count);
            winObjHandlers[0].WinDataSet(userSprites[randomNum], winRes.topThreeUsers[0].winningAmount.ToString(), winRes.topThreeUsers[0].userId);

            int randomNum2 = Random.Range(0, userSprites.Count);
            winObjHandlers[1].WinDataSet(userSprites[randomNum2], winRes.topThreeUsers[1].winningAmount.ToString(), userIds[randomNum2]);
            int randomNum3 = Random.Range(0, userSprites.Count);
            winObjHandlers[2].WinDataSet(userSprites[randomNum3], winRes.topThreeUsers[2].winningAmount.ToString(), userIds[randomNum3]);
        }
        else if (cnt == 2)
        {
            int randomNum = Random.Range(0, userSprites.Count);
            winObjHandlers[0].WinDataSet(userSprites[randomNum], winRes.topThreeUsers[0].winningAmount.ToString(), winRes.topThreeUsers[0].userId);
            int randomNum2 = Random.Range(0, userSprites.Count);
            winObjHandlers[1].WinDataSet(userSprites[randomNum2], winRes.topThreeUsers[1].winningAmount.ToString(), winRes.topThreeUsers[1].userId);

            int randomNum3 = Random.Range(0, userSprites.Count);
            winObjHandlers[2].WinDataSet(userSprites[randomNum3], winRes.topThreeUsers[2].winningAmount.ToString(), userIds[randomNum3]);
        }
        else if (cnt == 3)
        {
            int randomNum = Random.Range(0, userSprites.Count);
            winObjHandlers[0].WinDataSet(userSprites[randomNum], winRes.topThreeUsers[0].winningAmount.ToString(), winRes.topThreeUsers[0].userId);
            int randomNum2 = Random.Range(0, userSprites.Count);
            winObjHandlers[1].WinDataSet(userSprites[randomNum2], winRes.topThreeUsers[1].winningAmount.ToString(), winRes.topThreeUsers[1].userId);
            int randomNum3 = Random.Range(0, userSprites.Count);
            winObjHandlers[2].WinDataSet(userSprites[randomNum3], winRes.topThreeUsers[2].winningAmount.ToString(), winRes.topThreeUsers[2].userId);
        }


    }

    public void OnCloseWinPage()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            uimanager.instance.play.SetActive(false);
            uimanager.instance.home.SetActive(true);
            uimanager.instance.top.SetActive(true);

            NewUIManager.instance.OpenPanel(Panel.Home);
        }
    }


}

[System.Serializable]
public class WinRes
{
    public List<TopThreeUser> topThreeUsers;
    public int totalUsers;
    public int oneMinuteUsers;
    public int oneMinutePercentage;
    public int twoMinuteUsers;
    public int twoMinutePercentage;
    public int threeMinuteUsers;
    public int threeMinutePercentage;
}
[System.Serializable]
public class TopThreeUser
{
    public int rank;
    public string userId;
    public int winningAmount;
}


