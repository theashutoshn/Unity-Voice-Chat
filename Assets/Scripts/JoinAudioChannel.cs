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
    private string _channelName = "UnityVoiceTest";
    // Fill in a temporary token
    private string _token = "007eJxTYCgyy9zb2GzpONU4a6Pdt9ZzDlVLHzZMshG8zD5XoJnb6JUCg6llWrJxiplhmlGygYlBqpGlsaV5qqmpQbKRYUqSoZH5tP5HaQ2BjAwnDzMwMzJAIIjPxxCal1lSGZafmZwaklpcwsAAAEZjIoQ=";
    internal IRtcEngine RtcEngine;
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID) 
private ArrayList permissionList = new ArrayList() { Permission.Microphone };
#endif
    // Start is called before the first frame update
    



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
        UIManager.Instance.StatusCheck("Channel Joined");
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
        UIManager.Instance.StatusCheck("Channel Left");
        
         Debug.Log("Leaving " + _channelName);
        // Leave the channel
        RtcEngine.LeaveChannel();
        // Disable the audio module
        RtcEngine.DisableAudio();
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
            //UIManager.Instance.UserStatus("User Joined");
            Debug.Log("OnJoinChannelSuccess _channelName");
        }
        // This callback is triggered when a remote user successfully joins the channel
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            //UIManager.Instance.RemoteUser("RemoteUser Joined");
            Debug.Log("Remote user joined");
        }
        // This callback is triggered when a remote user leaves the current channel
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            //Debug.Log("Remote user Left");
        }

       
    }

}
