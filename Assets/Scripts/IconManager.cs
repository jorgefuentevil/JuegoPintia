using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{

    [Header("ICONO DINERO")]
    [SerializeField] private GameObject dineroVariacion;
    [SerializeField] private Image dineroContorno;
    [SerializeField] private Image dineroFill;

    [Header("ICONO SOCIAL")]
    [SerializeField] private GameObject socialVariacion;
    [SerializeField] private Image socialContorno;
    [SerializeField] private Image socialFill;

    [Header("ICONO SALUD")]
    [SerializeField] private GameObject saludVariacion;
    [SerializeField] private Image saludContorno;
    [SerializeField] private Image saludFill;

    [Header("ICONO ESPECÍFICO")]
    [SerializeField] private GameObject especificoVariacion;
    [SerializeField] private Image especificoContorno;
    [SerializeField] private Image especificoFill;

    [Header("MISC")]
    [SerializeField] private short puntuacionInicial;
    [SerializeField] private short multiplicadorNormal;
    [SerializeField] private short multiplicadorDoble;

    private short puntuacionDinero;
    private short puntuacionSocial;
    private short puntuacionSalud;
    private short puntuacionEspecifico;

    public void Start()
    {
        //Inicializamos puntuaciones
        puntuacionDinero = puntuacionSocial = puntuacionSalud = puntuacionEspecifico = puntuacionInicial;

        //Escondemos variaciones

        dineroVariacion.SetActive(false);
        socialVariacion.SetActive(false);
        saludVariacion.SetActive(false);
        especificoVariacion.SetActive(false);
    }




    public void PreviewEfectos(short dinero, short social, short salud, short especifico)
    {

        if (dinero != 0)
        {
            dineroVariacion.transform.localScale = Vector3.one * ((Math.Abs(dinero) == 1) ? 0.5f : 0.8f);
            dineroVariacion.SetActive(true);
        }
        if (social != 0)
        {
            socialVariacion.transform.localScale = Vector3.one * ((Math.Abs(social) == 1) ? 0.5f : 0.8f);
            socialVariacion.SetActive(true);
        }
        if (salud != 0)
        {
            saludVariacion.transform.localScale = Vector3.one * ((Math.Abs(salud) == 1) ? 0.5f : 0.8f);
            saludVariacion.SetActive(true);
        }
        if (especifico != 0)
        {
            especificoVariacion.transform.localScale = Vector3.one * ((Math.Abs(especifico) == 1) ? 0.5f : 0.8f);
            especificoVariacion.SetActive(true);
        }
    }

    public void SetEstadoInicial(){
        dineroVariacion.SetActive(false);
        socialVariacion.SetActive(false);
        saludVariacion.SetActive(false);
        especificoVariacion.SetActive(false);
    }

}
