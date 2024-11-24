using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.Android;

public class TTS : MonoBehaviour
{
    [SerializeField] private string language = "es-ES";
    // Start is called before the first frame update
    void Start()
    {
        TextToSpeech.Instance.Setting(language,1,1);

    }

    public void StartSpeaking(string textToSpeak){
        TextToSpeech.Instance.StartSpeak(textToSpeak);
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
