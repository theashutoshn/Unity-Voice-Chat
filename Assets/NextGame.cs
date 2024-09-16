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
            // Get the current Android activity
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            // Get the package manager
            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

            // Create an intent to launch the app
            AndroidJavaObject intent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", game2PackageName);

            // If the intent is null, the app is not installed
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
        // Call this to stop Game1 from running in the background
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("finish"); // Finish the activity and close the app

        // Alternatively, you can kill the process to ensure it fully closes
       // System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}
