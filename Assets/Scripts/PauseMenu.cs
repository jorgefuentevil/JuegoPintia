using UnityEngine;
using UnityEngine.UI;
using CandyCoded.HapticFeedback;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject botonVibracion;
    [SerializeField] private GameObject popupLegal;
    [SerializeField] private GameObject textoLegal;



    public void Start()
    {
        popupLegal.SetActive(false);
        textoLegal.SetActive(true);
        gameObject.AddComponent<AudioSource>();

        //Carga ajustes de vibracion
        if (PlayerPrefs.GetInt("VibracionEnabled") == 1)
        {
            botonVibracion.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            botonVibracion.GetComponent<Toggle>().isOn = false;
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro al entrar en ajustes");
        }

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro en volver");
        }
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

}
