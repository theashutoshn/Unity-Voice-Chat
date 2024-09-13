using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Agora.Rtc;
//using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelAudio;
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif


public class JoinAudioChannel : MonoBehaviour
{
    // Fill in your app ID
    private string _appID = "59fc3d61f2c040e29397e550c21db127";
    // Fill in your channel name
    private string _channelName = "AgoraVoiceTest";
    // Fill in a temporary token
    private string _token = "007eJxTYOjK+fn89067JZoK5XmcL47uECxZ/Ovk6VnxeX9mZJ+K/h6gwGBqmZZsnGJmmGaUbGBikGpkaWxpnmpqapBsZJiSZGhknvricVpDICPDe/c9TIwMEAji8zE4pucXJYblZyanhqQWlzAwAADGDCbt";
    internal IRtcEngine RtcEngine;
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
private ArrayList permissionList = new ArrayList() { Permission.Microphone };
#endif
    // Start is called before the first frame update
    private bool isMuted = false;

    public GameObject muteButton;
    public GameObject unMuteButton;


    void Start()
    {
        SetupAudioSDKEngine();
        InitEventHandler();
        SetupUI();
    }
    // Update is called once per frame
    void Update()
    {
        CheckPermissions();
    }
    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            // Destroy IRtcEngine
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }
    private void CheckPermissions() //android permission
    {
        #if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
                    foreach (string permission in permissionList) {
                            if (!Permission.HasUserAuthorizedPermission(permission)) 
                            {
                                Permission.RequestUserPermission(permission);
                            }
                }
                #endif
    }
    private void SetupUI() // Button Assign
    {
        GameObject go = GameObject.Find("Leave");
        go.GetComponent<Button>().onClick.AddListener(Leave);
        go = GameObject.Find("Join");
        go.GetComponent<Button>().onClick.AddListener(Join);
        //go = GameObject.Find("MuteIcon");
        //go.GetComponent<Button>().onClick.AddListener(ToggleButton);
        //go = GameObject.Find("UnMuteIcon");
        //go.GetComponent<Button>().onClick.AddListener(ToggleButton);
        


    }
    private void SetupAudioSDKEngine()
    {
        // Create an IRtcEngine instance
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext();  
        context.appId = _appID;
        context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
        context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
        // Initialize the IRtcEngine
        RtcEngine.Initialize(context);
    }
    public void Join()
    {
        PicoUIManager.Instance.StatusCheck("Status: " + _channelName.ToString() + " " + "Channel Joined");
        Debug.Log("Joining" + _channelName);
       
        // Enable the audio module
        RtcEngine.EnableAudio();
        // Set channel media options
        ChannelMediaOptions options = new ChannelMediaOptions();
        // Publish the audio stream captured by the microphone
        options.publishMicrophoneTrack.SetValue(true);
        // Automatically subscribe to all audio streams
        options.autoSubscribeAudio.SetValue(true);
        // Set the channel profile to live broadcast
        options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        // Set the user role to broadcaster
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Join the channel
        RtcEngine.JoinChannel(_token, _channelName, 0, options);
    }
    public void Leave()
    {
        PicoUIManager.Instance.StatusCheck("Status: " + _channelName.ToString() + " " + "Channel Left");
        
         Debug.Log("Leaving " + _channelName);
        // Leave the channel
        RtcEngine.LeaveChannel();
        // Disable the audio module
        RtcEngine.DisableAudio();
    }

    public void ToggleButton()
    {
        isMuted = !isMuted;
        Mute(isMuted);
    }
   

    public void Mute(bool mute)
    {
        if (RtcEngine != null)
        {
            // Mute or unmute the local audio stream
            RtcEngine.MuteLocalAudioStream(mute);

            if (mute)
            {
                muteButton.SetActive(true);
                unMuteButton.SetActive(false);
                Debug.Log("Microphone muted.");
                PicoUIManager.Instance.MicrophoneStatus("Microphone: Muted");
            }
            else
            {
                muteButton.SetActive(false);
                unMuteButton.SetActive(true);
                Debug.Log("Microphone unmuted.");
                PicoUIManager.Instance.MicrophoneStatus("Microphone: Unmuted");
            }
        }
    }

    






    // Create an instance of the user callback class and set the callback
    private void InitEventHandler()
    {
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }
    // Implement your own callback class by inheriting the IRtcEngineEventHandler interface class
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly JoinAudioChannel _audioSample;
        internal UserEventHandler(JoinAudioChannel audioSample)
        {
            _audioSample = audioSample;
        }
        // This callback is triggered when the local user successfully joins the channel
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            PicoUIManager.Instance.UserStatus("User Status: " + "User Joined");
            Debug.Log("OnJoinChannelSuccess _channelName");
        }
        // This callback is triggered when a remote user successfully joins the channel
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            PicoUIManager.Instance.RemoteUser("Remote User Status: " + "Remote User Joined");
            Debug.Log("Remote user joined");
        }
        // This callback is triggered when a remote user leaves the current channel
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            PicoUIManager.Instance.RemoteUser("Remote User Status: " + "Remote User Left");
            Debug.Log("Remote user Left");
        }

       
    }

}
