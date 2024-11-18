using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEstadoInicial : MonoBehaviour{

[SerializeField] private GameObject MainMenuPanel;
[SerializeField] private GameObject PopUpEdadPanel;
[SerializeField] private GameObject MenuAjustesPanel;



    public void Awake(){
        MainMenuPanel.SetActive(true);
        MenuAjustesPanel.SetActive(true);
        MenuAjustesPanel.SetActive(false);

        if(PlayerPrefs.HasKey("AgeRange")){
            PopUpEdadPanel.SetActive(false);
        }else{
            PopUpEdadPanel.SetActive(true);
        }

    }
}
