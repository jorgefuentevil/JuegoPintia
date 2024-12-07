using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class PopUpFinalPartida : MonoBehaviour
{   

    [SerializeField] private LocalizedString stringTextoSuperiorVictoria;
    [SerializeField] private LocalizedString stringTextoDescripcionVictoria;
    [SerializeField] private LocalizedString stringTextoSuperiorDerrota;
    [SerializeField] private LocalizedString stringTextoDescripcionDerrota;
    [SerializeField] private TextMeshProUGUI textoSuperior;
    [SerializeField] private TextMeshProUGUI textoDescripcion;


    public void Awake() { }


    public void MostrarPopup(bool trueIfVictoria, int numYears)
    {
        gameObject.SetActive(true);
        if(trueIfVictoria)
        {   
            textoSuperior.text = stringTextoSuperiorVictoria.GetLocalizedString();
            textoDescripcion.text = stringTextoDescripcionVictoria.GetLocalizedString();
        }
        else
        {
            textoSuperior.text = stringTextoSuperiorDerrota.GetLocalizedString();
            stringTextoDescripcionDerrota.Arguments = new object[] {numYears};
            textoDescripcion.text = stringTextoDescripcionDerrota.GetLocalizedString();
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
        GameManager.Instance.CambiaEscenaGamePrincipal(GameManager.Instance.currentLevel, GameManager.Instance.currentLevelIndex);
    }

    private IEnumerator AuxHome()
    {
        GameObject.FindGameObjectWithTag("GamePrincipalManager").GetComponent<GamePrincipalEstadoInicial>().EmpiezaTransicion();
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.CambiaEscenaMainMenu();
    }

}
