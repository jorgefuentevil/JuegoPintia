using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEstadoInicial : MonoBehaviour{

[SerializeField] private GameObject MainMenuPanel;
[SerializeField] private GameObject PopUpEdadPanel;
[SerializeField] private GameObject MenuAjustesPanel;



    public void Start(){
        MainMenuPanel.SetActive(true);
        //Activamos y Desactivamos para que se carguen los ajustes de volumen jejej Fix muy guarrete.
        MenuAjustesPanel.SetActive(true);
        MenuAjustesPanel.SetActive(false);

        if(PlayerPrefs.HasKey("AgeRange")){
            PopUpEdadPanel.SetActive(false);
        }else{
            PopUpEdadPanel.SetActive(true);
        }

    }
}
