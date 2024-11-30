using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.Android;
using UnityEngine.Localization.Settings;


public class TTS : MonoBehaviour
{
    [SerializeField] private string language = "es";
    // Start is called before the first frame update
    void Start()
    {
        var selectedLocale = LocalizationSettings.SelectedLocale;
        language = selectedLocale.Identifier.Code;
        TextToSpeech.Instance.Setting(language,1,1);

    }

    public void StartSpeaking(string textToSpeak){
        if (PlayerPrefs.GetInt("TTSEnable") == 1)
        {
            TextToSpeech.Instance.StartSpeak(textToSpeak);
        }
    }
    public void StopSpeak(){
        TextToSpeech.Instance.StopSpeak();
    }
    public void OnSpeakStart(){
        Debug.Log("Talking start...");
    }
    public void OnSpeakStop(){
        Debug.Log("Talking stop...");
    }
}
