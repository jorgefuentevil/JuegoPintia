using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrincipalEstadoInicial : MonoBehaviour
{

    [SerializeField] private GameObject GamePrincipalPanel;
    [SerializeField] private GameObject PopUpFinPanel;
    [SerializeField] private GameObject MenuAjustesPanel;
    [SerializeField] private GameObject AudioManagerPrefab;


    public void Awake()
    {   
        if(GameObject.FindGameObjectWithTag("AudioManager") == null){
            Instantiate(AudioManagerPrefab);
        }

        GamePrincipalPanel.SetActive(true);
        PopUpFinPanel.SetActive(false);
        MenuAjustesPanel.SetActive(true);
        MenuAjustesPanel.SetActive(false);
    }
}
