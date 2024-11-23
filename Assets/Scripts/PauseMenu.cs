using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject botonVibracion;
    [SerializeField] private GameObject popupLegal;
    [SerializeField] private GameObject textoLegal;

    public AudioSource src { get { return GetComponent<AudioSource>();}}
    public AudioClip clip;

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

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
        Debug.Log("clik en volver");
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            PlaySound();
        }
    }

    public void ToggleVibracion(bool value)
    {

        if (value)
        {
            
            PlayerPrefs.SetInt("VibracionEnabled", 1);
            #if UNITY_ANDROID || UNITY_IOS
                PlaySound();
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
        PlaySound();
    }

    public void HideTerms()
    {
        popupLegal.SetActive(false);
        textoLegal.SetActive(true);
        PlaySound();
    }

    public void PlaySound(){
        src.PlayOneShot(clip);
    }
}
