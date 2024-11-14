using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject botonVibracion;
    [SerializeField] private GameObject popupLegal;
    [SerializeField] private GameObject textoLegal;

    public void Start(){
        popupLegal.SetActive(false);
        textoLegal.SetActive(true);
    }

    public void Pause(){
        pauseMenu.SetActive(true);
        mainMenu.SetActive(false);

    }

    public void Resume(){
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ToggleVibracion(){

    }

    public void ShowTerms(){
        popupLegal.SetActive(true);
        textoLegal.SetActive(false);
    }

    public void HideTerms(){
        popupLegal.SetActive(false);
        textoLegal.SetActive(true);
    }
}
