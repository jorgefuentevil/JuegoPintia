using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Vibracion 
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer=new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity=unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrar=currentActivity.Call<AndroidJavaObject>("getSystemService","vibrar");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrar;
#endif


    public static void Vibrar(long ms=250){
        if(IsAndroid()){
            vibrar.Call("vibrar",ms);
        }
        else{
            Handheld.Vibrate();
        }
    }

    public static void Cancel(){
        if(IsAndroid()){
            vibrar.Call("cancel");
        }
    }
    public static bool IsAndroid(){
        #if UNITY_ANDROID && !UNITY_EDITOR
            return true;
        #else
            return false;
        #endif
    }
}
