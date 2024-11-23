using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSManager : MonoBehaviour {
    TextToSpeech tts;
    void Start()
    {

        tts = GetComponent<TextToSpeech>();
    }
    public void Speak(string texto)
    {
        tts.Speak(texto, (string msg) =>
        {
            tts.ShowToast(msg);
        });
    }
    public void ChangeSpeed()
    {
        tts.SetSpeed(0.5f);
    }
    public void ChangeLanguage()
    {
        tts.SetLanguage(TextToSpeech.Locale.UK);
    }
    public void ChangePitch()
    {
        tts.SetPitch(0.6f);
    }
}
