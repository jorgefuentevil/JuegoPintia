using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vibracion : MonoBehaviour
{
    public Toggle toggle; // Arrastra tu Toggle en el Inspector

    void Start()
    {
        // Asocia el método VibrateOnToggle al evento OnValueChanged del Toggle
        toggle.onValueChanged.AddListener(VibrateOnToggle);
    }

    // Método que se llama al cambiar el estado del Toggle
    private void VibrateOnToggle(bool isOn)
    {
        if (isOn)
        {
            Vibrar();
        }
    }

    // Método para vibrar el dispositivo tan solo funciona en android o ios
    private void Vibrar()
    {
        #if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
        #endif
    }

    // Importante: Elimina el listener cuando el objeto se destruye
    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(VibrateOnToggle);
    }
}

