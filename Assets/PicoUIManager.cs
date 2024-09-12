using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PicoUIManager : MonoBehaviour
{
    public static PicoUIManager Instance;

    public TextMeshProUGUI statusText;
    public TextMeshProUGUI userText;
    public TextMeshProUGUI remoteUserText;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StatusCheck(string status)
    {
        if (statusText != null)
        {
            statusText.text = status;
        }
    }

    public void UserStatus(string user)
    {
        if (userText != null)
        {
            userText.text = user;
        }
    }

    public void RemoteUser(string remote)
    {
        if (remoteUserText != null)
        {
            remoteUserText.text = remote;
        }
    }
}
