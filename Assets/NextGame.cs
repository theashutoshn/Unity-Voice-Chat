using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextGame : MonoBehaviour
{
    // The package name of Game2 (Replace with the actual package name of Game2)
    private string game2PackageName = "com.AshXR.NewApp";
    

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    public void LaunchGame2()
    {

        try
        {
           
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            
            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

           
            AndroidJavaObject intent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", game2PackageName);

           
            if (intent != null)
            {
                // Start the app
                currentActivity.Call("startActivity", intent);
                CloseGame1();
            }
            else
            {
                Debug.LogError("Game2 is not installed.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to launch Game2: " + e.Message);
        }

        
    }


    private void CloseGame1()
    {
        
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("finish"); // finish the activity and close the app

        
       //System.Diagnostics.Process.GetCurrentProcess().Kill(); // to completely kill the application
    }
}
