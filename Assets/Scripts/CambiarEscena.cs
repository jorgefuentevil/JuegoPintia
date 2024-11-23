using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CandyCoded.HapticFeedback;

public class CambiarEscena : MonoBehaviour
{
    public void cambiarEscena(string nombre)
    {
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro al jugar");
        }
        SceneManager.LoadScene(nombre);
    }
}
