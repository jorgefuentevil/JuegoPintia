using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject botonVibracion;

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
}
