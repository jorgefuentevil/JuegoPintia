using UnityEngine;
using UnityEngine.UI;

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

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ToggleVibracion(bool value)
    {

        if (value)
        {
            Debug.Log("Vibro brrrrrrr");
            
            PlayerPrefs.SetInt("VibracionEnabled", 1);
            #if UNITY_ANDROID || UNITY_IOS
                Handheld.Vibrate();
                Debug.Log("Vibro brrrrrrr");
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
    }

    public void HideTerms()
    {
        popupLegal.SetActive(false);
        textoLegal.SetActive(true);
    }
}
