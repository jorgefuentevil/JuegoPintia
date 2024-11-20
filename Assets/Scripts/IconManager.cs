using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    private Image dineroVariacionImage;
    private Image socialVariacionImage;
    private Image saludVariacionImage;
    private Image especificoVariacionImage;


    // ---- Parámetros animaciones ---- //
    private float easeInVariaciones = 0.3f;
    private float easeOutVariaciones = 0.3f;
    private float scaleVariacionSimple = 0.5f;
    private float scaleVariacionDoble = 0.8f;

    public void Start()
    {
        //Inicializamos puntuaciones
        puntuacionDinero = puntuacionSocial = puntuacionSalud = puntuacionEspecifico = puntuacionInicial;

        //Escondemos variaciones
        dineroVariacion.transform.DOScale(0,0);
        socialVariacion.transform.DOScale(0,0);
        saludVariacion.transform.DOScale(0,0);
        especificoVariacion.transform.DOScale(0,0);
    }

    public void PreviewEfectos(short dinero, short social, short salud, short especifico)
    {

        if (dinero != 0)
        {
            float scale = (Math.Abs(dinero) == 1) ? 0.5f : 0.8f;
            dineroVariacion.transform.DOScale(scale,0.3f);
        }
        if (social != 0)
        {
            float scale = (Math.Abs(social) == 1) ? 0.5f : 0.8f;
            socialVariacion.transform.DOScale(scale,0.3f);    
        }
        if (salud != 0)
        {
            float scale = (Math.Abs(salud) == 1) ? 0.5f : 0.8f;
            saludVariacion.transform.DOScale(scale,0.3f);

        }
        if (especifico != 0)
        {
            float scale = (Math.Abs(especifico) == 1) ? 0.5f : 0.8f;
            especificoVariacion.transform.DOScale(scale,0.3f);

        }
    }

    public void SetEstadoInicial()
    {
        dineroVariacion.transform.DOScale(0,0.3f);
        socialVariacion.transform.DOScale(0,0.3f);
        saludVariacion.transform.DOScale(0,0.3f);
        especificoVariacion.transform.DOScale(0,0.3f);
    }

}
