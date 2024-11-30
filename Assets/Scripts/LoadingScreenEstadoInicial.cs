using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreenEstadoInicial : MonoBehaviour
{

    [SerializeField] private GameObject AudioManagerPrefab;
    [SerializeField] private GameObject PanelTransicion;

    void Awake()
    {       
        DOTween.Init();

        if(GameObject.FindGameObjectWithTag("AudioManager") == null){
            Instantiate(AudioManagerPrefab);
        }
        PanelTransicion.SetActive(false);
        PanelTransicion.GetComponent<Image>().color = new Color(0,0,0,0);

        StartCoroutine(Waiter());
    }

    private IEnumerator Waiter()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(2);

        PanelTransicion.SetActive(true);
        PanelTransicion.GetComponent<Image>().DOFade(1, 1);

        
        yield return new WaitForSeconds(1);


        if ( true || PlayerPrefs.HasKey("Tutorial")) //TODO: CAMBIAR ESTO
        {
            GameManager.Instance.CambiaEscenaMainMenu();
        }
        else
        {
            GameManager.Instance.CambiaEscenaTutorial();
        }
        
    }

}
