using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIHandler : MonoBehaviour
{
    private string url = "http://localhost:5000/submit-agoda-id";
    public int index;
    void Start()
    {
        StartCoroutine(GetData());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator GetData()
    {
        SimpleJSON.JSONNode postData = new SimpleJSON.JSONObject();
        postData["key1"] = "value1";
        postData["key2"] = "value2";
        string jsonString = postData.ToString();
        Debug.Log("HELLO" + jsonString);


        using (UnityWebRequest request = UnityWebRequest.Post(url, jsonString))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);

                Debug.Log("Stats" + stats);

                
            }
        }
    }
}
