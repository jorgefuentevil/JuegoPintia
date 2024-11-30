using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuEstadoInicial : MonoBehaviour{

    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject PopUpEdadPanel;
    [SerializeField] private GameObject MenuAjustesPanel;
    [SerializeField] private GameObject AudioManagerPrefab;
    [SerializeField] private GameObject PanelTransicion;

    public void Awake(){

        if(GameObject.FindGameObjectWithTag("AudioManager") == null){
            Instantiate(AudioManagerPrefab);
        }

        MainMenuPanel.SetActive(true);
        MenuAjustesPanel.SetActive(true);
        MenuAjustesPanel.SetActive(false);
    
        PanelTransicion.SetActive(true);
        PanelTransicion.GetComponent<Image>().raycastTarget=false;

        if(PlayerPrefs.HasKey("AgeRange")){
            PopUpEdadPanel.SetActive(false);
        }else{
            PopUpEdadPanel.SetActive(true);
        }

    }


    public void TerminaTransicion()
    {
        PanelTransicion.GetComponent<Image>().DOFade(0, 1).onComplete = () =>
        {
            PanelTransicion.SetActive(false);
            PanelTransicion.GetComponent<Image>().raycastTarget=true;
        };
    }

    public void EmpiezaTransicion()
    {
        PanelTransicion.GetComponent<Image>().DOFade(1, 1).onComplete = () =>
        {
            PanelTransicion.SetActive(true);
            PanelTransicion.GetComponent<Image>().raycastTarget=false;
        };
    }
}
