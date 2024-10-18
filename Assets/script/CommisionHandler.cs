using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommisionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetCommisionPoints());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GetCommisionPoints()
    {
        WWWForm form = new WWWForm();

        using (var api = UnityWebRequest.Post(StaticData.baseURL + StaticData.commisionPoints, form))
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
                Debug.Log("commision Points --------- "+api.downloadHandler.text);
            }
        }
    }
}