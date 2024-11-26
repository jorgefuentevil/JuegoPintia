using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrincipalEstadoInicial : MonoBehaviour
{

    [SerializeField] private GameObject GamePrincipalPanel;
    [SerializeField] private GameObject PopUpFinPanel;
    [SerializeField] private GameObject MenuAjustesPanel;
    [SerializeField] private GameObject AudioManager;


    public void Awake()
    {   
        if(GameObject.FindGameObjectWithTag("AudioManager") == null){
            Instantiate(AudioManager);
        }

        GamePrincipalPanel.SetActive(true);
        PopUpFinPanel.SetActive(false);
        MenuAjustesPanel.SetActive(true);
        MenuAjustesPanel.SetActive(false);
    }
}
