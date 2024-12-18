using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class GamePrincipalEstadoInicial : MonoBehaviour
{

    [SerializeField] private GameObject GamePrincipalPanel;
    [SerializeField] private GameObject PopUpFinPanel;
    [SerializeField] private GameObject MenuAjustesPanel;
    [SerializeField] private GameObject AudioManagerPrefab;
    [SerializeField] private GameObject PanelTransicion;


    public void Awake()
    {
        if (GameObject.FindGameObjectWithTag("AudioManager") == null)
        {
            Instantiate(AudioManagerPrefab);
        }

        DOTween.Init();


        GamePrincipalPanel.SetActive(true);
        PopUpFinPanel.SetActive(false);
        MenuAjustesPanel.SetActive(true);
        MenuAjustesPanel.GetComponent<PauseMenu>().SetMenuPlaying();
        MenuAjustesPanel.SetActive(false);
        

        PanelTransicion.SetActive(true);
        PanelTransicion.GetComponent<Image>().raycastTarget=false;

    }

    public void TerminaTransicion()
    {   
        if(PanelTransicion == null) PanelTransicion = GameObject.FindGameObjectWithTag("PanelTransicion");
        PanelTransicion.GetComponent<Image>().DOFade(0, 1).onComplete = () =>
        {
            PanelTransicion.SetActive(false);
            PanelTransicion.GetComponent<Image>().raycastTarget=true;
        };
    }

    public void EmpiezaTransicion()
    {   
        PanelTransicion.SetActive(true);
        PanelTransicion.GetComponent<Image>().raycastTarget=false;
        PanelTransicion.GetComponent<Image>().DOFade(1, 1);
    }



}
