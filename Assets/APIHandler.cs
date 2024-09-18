using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class NestedObject
{
    public string nestedKey1;
    public string nestedKey2;
}

[System.Serializable]
public class PostData
{
    public string key1;
    public string key2;
    public NestedObject nestedObject;
}

public class APIHandler : MonoBehaviour
{
    private string url = "http://localhost:5000/submit-agoda-id";
    public int index;


    private WebRequestHelper webRequestHelper;
    void Start()
    {
        //StartCoroutine(PostData());

        webRequestHelper = gameObject.AddComponent<WebRequestHelper>();

        // Example GET request
        StartCoroutine(webRequestHelper.GetRequest("https://jsonplaceholder.typicode.com/posts/1", (error, response) =>
        {
            if (error == null)
            {
                Debug.Log("GET Response: " + response);
            }
            else
            {
                Debug.LogError("GET Error: " + error);
            }
        }));

        PostData postData = new PostData
        {
            key1 = "value1",
            key2 = "value2",
            nestedObject = new NestedObject
            {
                nestedKey1 = "nestedValue1",
                nestedKey2 = "nestedValue2"
            }
        };

        string jsonString = JsonUtility.ToJson(postData);

        StartCoroutine(webRequestHelper.PostRequest("http://localhost:5000/submit-agoda-id", jsonString, (error, response) =>
        {
            if (error == null)
            {
                Debug.Log("POST Response: " + response);
            }
            else
            {
                Debug.LogError("POST Error: " + error);
            }
        }));
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator PostData()
    {
        PostData postData = new PostData
        {
            key1 = "value1",
            key2 = "value2",
            nestedObject = new NestedObject
            {
                nestedKey1 = "nestedValue1",
                nestedKey2 = "nestedValue2"
            }
        };

        string jsonString = JsonUtility.ToJson(postData);
        //Debug.Log("HELLO" + jsonString);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);


        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Set the content type to JSON
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);
                Debug.Log("Stats" + json);


            }
        }
    }



   
}

