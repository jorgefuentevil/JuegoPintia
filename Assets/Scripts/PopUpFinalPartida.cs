using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class PopUpFinalPartida : MonoBehaviour
{   
    //TODO: Personalizar mensajes y localizarlos
    [SerializeField] private LocalizedString stringTextoSuperior;
    [SerializeField] private LocalizedString stringTextoDescripcion;
    [SerializeField] private TextMeshProUGUI textoSuperior;
    [SerializeField] private TextMeshProUGUI textoDescripcion;


    public void Awake() { }


    public void MostrarPopup(bool trueIfVictoria)
    {
        gameObject.SetActive(true);
        if(trueIfVictoria)
        {   
            textoSuperior.text = "VICTORIA";
            textoDescripcion.text = "Has superado todas las decisiones con éxito";
        }
        else
        {
            textoSuperior.text = "FIN DE LA PARTIDA";
            textoDescripcion.text = "Uno de tus atributos ha llegado al máximo o al mínimo";  
        }
    }


    public void AccionBotonHome()
    {
        StartCoroutine(AuxHome());
    }

    public void AccionBotonReiniciar()
    {
        StartCoroutine(AuxReiniciar());
    }


    private IEnumerator AuxReiniciar()
    {
        GameObject.FindGameObjectWithTag("GamePrincipalManager").GetComponent<GamePrincipalEstadoInicial>().EmpiezaTransicion();
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.CambiaEscenaGamePrincipal(GameManager.Instance.currentLevel);
    }

    private IEnumerator AuxHome()
    {
        GameObject.FindGameObjectWithTag("GamePrincipalManager").GetComponent<GamePrincipalEstadoInicial>().EmpiezaTransicion();
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.CambiaEscenaMainMenu();
    }

}
