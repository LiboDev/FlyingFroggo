using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticFeedbackController : MonoBehaviour
{
    private AndroidJavaObject vibrator;

    void Start()
    {
        // Get the current Android activity
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        // Get the Vibrator service
        vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    }

    // Method to vibrate briefly for click feedback
    public void VibrateClick()
    {
        if (vibrator != null)
        {
            if (AndroidVersion >= 26)
            {
                // For Android 8.0 (API level 26) and above
                VibratePattern(new long[] { 0, 30 }, -1);
            }
            else
            {
                // For devices below Android 8.0
                vibrator.Call("vibrate", 30); // Vibrate for 30 milliseconds
            }
        }
    }

    public void VibratePattern(long[] pattern, int repeat)
    {
        if (vibrator != null)
        {
            // Check Android version to use VibrationEffect API
            if (AndroidVersion >= 26)
            {
                // Use the VibrationEffect class for API level 26 and above
                using (AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect"))
                {
                    if (vibrationEffectClass != null)
                    {
                        // Create the vibration effect
                        AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);

                        if (vibrationEffect != null)
                        {
                            // Vibrate using the created effect
                            vibrator.Call("vibrate", vibrationEffect);
                        }
                        else
                        {
                            Debug.LogError("Failed to create vibration effect.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to get VibrationEffect class.");
                    }
                }
            }
            else
            {
                Debug.LogWarning("Vibration patterns are not supported on this Android version. Using basic vibration.");
                // Fallback for devices below API level 26
                vibrator.Call("vibrate", pattern[0]); // Vibrate for the first duration in pattern as a fallback
            }
        }
        else
        {
            Debug.LogError("Vibrator service is not initialized.");
        }
    }

    public void Vibrate(long milliseconds)
    {
        if (vibrator != null)
        {
            vibrator.Call("vibrate", milliseconds);
        }
    }

    public void CancelVibration()
    {
        if (vibrator != null)
        {
            vibrator.Call("cancel");
        }
    }

    private int AndroidVersion
    {
        get
        {
            using (AndroidJavaClass versionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return versionClass.GetStatic<int>("SDK_INT");
            }
        }
    }
}
