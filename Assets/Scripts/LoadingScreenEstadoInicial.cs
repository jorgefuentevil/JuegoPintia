using System.Collections;
using UnityEngine;

public class LoadingScreenEstadoInicial : MonoBehaviour
{


    [SerializeField] private GameObject AudioManagerPrefab;

    void Awake()
    {   
        
        if(GameObject.FindGameObjectWithTag("AudioManager") == null){
            Instantiate(AudioManagerPrefab);
        }

        StartCoroutine(Waiter());
    }

    IEnumerator Waiter()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(2);
        if ( true || PlayerPrefs.HasKey("Tutorial"))
        {
            GameManager.Instance.CambiaEscenaMainMenu();
        }
        else
        {
            GameManager.Instance.CambiaEscenaTutorial();
        }
        
    }
}
