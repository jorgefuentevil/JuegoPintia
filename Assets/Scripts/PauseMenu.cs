using UnityEngine;
using UnityEngine.UI;
using CandyCoded.HapticFeedback;
using TextSpeech;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject toggleVibracion;
    [SerializeField] private GameObject toggleTTS;
    [SerializeField] private GameObject popupLegal;
    [SerializeField] private GameObject textoLegal;
    [SerializeField] private GameObject botonFinPartida;
    [SerializeField] private TTS tts;



    public void Awake()
    {
        botonFinPartida.SetActive(false);
        popupLegal.SetActive(false);
        textoLegal.SetActive(true);
        gameObject.AddComponent<AudioSource>();

        //Carga ajustes de vibracion
        if (PlayerPrefs.GetInt("VibracionEnabled") == 1)
        {
            toggleVibracion.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            toggleVibracion.GetComponent<Toggle>().isOn = false;
        }

        if (PlayerPrefs.GetInt("TTSEnable") == 1)
        {
            toggleTTS.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            toggleTTS.GetComponent<Toggle>().isOn = false;
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        //mainMenu.SetActive(false);
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro al entrar en ajustes");
        }

    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        //mainMenu.SetActive(true);
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro en volver");
        }
    }

    public void FinishGame()
    {
        StartCoroutine(Fin());
    }

    public void ToggleVibracion(bool value)
    {

        if (value)
        {            
            PlayerPrefs.SetInt("VibracionEnabled", 1);
            #if UNITY_ANDROID || UNITY_IOS
                HapticFeedback.HeavyFeedback();
                Debug.Log("Vibro al pulsar el toggle");
            #endif
        }
        else
        {
            PlayerPrefs.SetInt("VibracionEnabled", 0);
            Debug.Log("No Vibro :(((((");

        }
    }

    public void ToggleTTS(bool value)
    {
        if (value)
        {            
            PlayerPrefs.SetInt("TTSEnable", 1);
        }
        else
        {
            PlayerPrefs.SetInt("TTSEnable", 0);
            tts.StopSpeak();
        }
    }

    public void ShowTerms()
    {
        popupLegal.SetActive(true);
        textoLegal.SetActive(false);
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro en entrar terms legales");
        }
    }

    public void HideTerms()
    {
        popupLegal.SetActive(false);
        textoLegal.SetActive(true);
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro en salir terms legales");
        }
    }


    public void SetMenuPlaying()
    {
        botonFinPartida.SetActive(true);
    }

    private IEnumerator Fin()
    {
        GameObject.FindGameObjectWithTag("GamePrincipalManager").GetComponent<GamePrincipalEstadoInicial>().EmpiezaTransicion();
        yield return new WaitForSeconds(1f);
        GameManager.Instance.CambiaEscenaMainMenu();
    }



}
