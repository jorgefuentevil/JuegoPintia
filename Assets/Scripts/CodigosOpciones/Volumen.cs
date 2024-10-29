using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volumen : MonoBehaviour
{
    public Slider slider;
    public float sliderValor;
    public Image imgMute;

    public void Start(){
        slider.value=PlayerPrefs.GetFloat("volumen",0.5f);
        AudioListener.volume=slider.value;
    }

    public void cambiaVolumen(float valor){
        sliderValor=valor;
        PlayerPrefs.GetFloat("volumen",valor);
        AudioListener.volume=slider.value;
        checkMute();
    }

    public void checkMute(){
        if(sliderValor==0){
            imgMute.enabled=true;
        }
        else{
            imgMute.enabled=false;
        }
    }
}
