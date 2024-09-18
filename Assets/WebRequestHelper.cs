using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestHelper : MonoBehaviour
{
    // GET Request
    public IEnumerator GetRequest(string url, Action<string, string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                callback(null, webRequest.downloadHandler.text);
            }
            else
            {
                // Extract error body if available
                string errorBody = webRequest.downloadHandler != null ? webRequest.downloadHandler.text : null;
                callback(webRequest.error + (errorBody != null ? " Body: " + errorBody : ""), null);
            }
        }
    }

    // POST Request
    public IEnumerator PostRequest(string url, string jsonData, Action<string, string> callback)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                callback(null, webRequest.downloadHandler.text);
            }
            else
            {
                // Extract error body if available
                string errorBody = webRequest.downloadHandler != null ? webRequest.downloadHandler.text : null;
                callback(webRequest.error + (errorBody != null ? " Body: " + errorBody : ""), null);
            }
        }
    }

    // PUT Request
    public IEnumerator PutRequest(string url, string jsonData, Action<string, string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, jsonData))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                callback(null, webRequest.downloadHandler.text);
            }
            else
            {
                // Extract error body if available
                string errorBody = webRequest.downloadHandler != null ? webRequest.downloadHandler.text : null;
                callback(webRequest.error + (errorBody != null ? " Body: " + errorBody : ""), null);
            }
        }
    }

    // DELETE Request
    public IEnumerator DeleteRequest(string url, Action<string, string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                callback(null, webRequest.downloadHandler.text);
            }
            else
            {
                // Extract error body if available
                string errorBody = webRequest.downloadHandler != null ? webRequest.downloadHandler.text : null;
                callback(webRequest.error + (errorBody != null ? " Body: " + errorBody : ""), null);
            }
        }
    }

    // Example of GET request with headers
    public IEnumerator GetWithHeaders(string url, string token, Action<string, string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                callback(null, webRequest.downloadHandler.text);
            }
            else
            {
                // Extract error body if available
                string errorBody = webRequest.downloadHandler != null ? webRequest.downloadHandler.text : null;
                callback(webRequest.error + (errorBody != null ? " Body: " + errorBody : ""), null);
            }
        }
    }
}
