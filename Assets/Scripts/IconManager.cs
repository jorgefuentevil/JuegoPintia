using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

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

    [Header("FLECHAS SELECCION")]
    [SerializeField] private GameObject flechaDerecha;
    [SerializeField] private GameObject flechaIzquierda;
    [SerializeField] private GameObject tickDerecha;
    [SerializeField] private GameObject tickIzquierda;

    private Vector3 posicionShowDerecha;
    private Vector3 posicionShowIzquierda;
    private Vector3 posicionHideDerecha;
    private Vector3 posicionHideIzquierda;

    private readonly short puntuacionInicial = 10;
    private readonly short multiplicadorNormal = 1;
    private readonly short multiplicadorDoble = 2;


    private readonly Color colorRojo = Color.red;
    private readonly Color colorVerde = Color.green;
    private readonly Color colorBlanco = Color.white;

    // ---- Parámetros animaciones ---- //
    private readonly float easeInVariaciones = 0.3f;
    private readonly float easeOutVariaciones = 0.3f;
    private readonly float scaleVariacionSimple = 0.5f;
    private readonly float scaleVariacionDoble = 0.8f;

    public void Start()
    {

        //Guardamos posiciones de animaciones
        posicionShowDerecha = flechaDerecha.transform.position;
        posicionShowIzquierda = flechaIzquierda.transform.position;
        posicionHideIzquierda = tickIzquierda.transform.position;
        posicionHideDerecha = tickDerecha.transform.position;

        //Escondemos variaciones
        dineroVariacion.transform.localScale = Vector3.zero;
        socialVariacion.transform.localScale = Vector3.zero;
        saludVariacion.transform.localScale = Vector3.zero;
        especificoVariacion.transform.localScale = Vector3.zero;

        //Ponemos fill a la mitad
        dineroFill.fillAmount = 0.5f;
        socialFill.fillAmount = 0.5f;
        saludFill.fillAmount = 0.5f;
        especificoFill.fillAmount = 0.5f;

    }

    public void PreviewEfectos(short[] stats)
    {
        PreviewEfectoIndividual(dineroVariacion, stats[0]);
        PreviewEfectoIndividual(socialVariacion, stats[1]);
        PreviewEfectoIndividual(saludVariacion, stats[2]);
        PreviewEfectoIndividual(especificoVariacion, stats[3]);
    }

    private void PreviewEfectoIndividual(GameObject variacion, short stat)
    {
        if (stat == 0) return;

        float scale = (Math.Abs(stat) == 1) ? scaleVariacionSimple : scaleVariacionDoble;
        variacion.transform.DOScale(scale, easeInVariaciones);
    }

    public void AplicaEfectos(short[] stats, short puntuacionMax)
    {

        AplicaEfectoIndividual(dineroFill, dineroContorno, stats[0], puntuacionMax);
        AplicaEfectoIndividual(socialFill, socialContorno, stats[1], puntuacionMax);
        AplicaEfectoIndividual(saludFill, saludContorno, stats[2], puntuacionMax);
        AplicaEfectoIndividual(especificoFill, especificoContorno, stats[3], puntuacionMax);
    }


    private void AplicaEfectoIndividual(Image fillImage, Image contornoImage, short valorStat, short puntuacionMax)
    {
        if (valorStat == 0) return;

        float fill = fillImage.fillAmount + ((float)valorStat / puntuacionMax);
        Color color = (valorStat > 0) ? colorVerde : colorRojo;
        Sequence seq = DOTween.Sequence();

        seq.Append(contornoImage.DOColor(color, 0.3f));
        seq.Insert(0, fillImage.DOColor(color, 0.3f));
        seq.Append(fillImage.DOFillAmount(fill, 1.2f));
        seq.Insert(1, contornoImage.DOColor(colorBlanco, 1.5f));
        seq.Insert(1, fillImage.DOColor(colorBlanco, 1.5f));
    }


    public void SetEstadoEligeCarta()
    {   
        //Esconde circulos variacion.
        dineroVariacion.transform.DOScale(0, easeOutVariaciones);
        socialVariacion.transform.DOScale(0, easeOutVariaciones);
        saludVariacion.transform.DOScale(0, easeOutVariaciones);
        especificoVariacion.transform.DOScale(0, easeOutVariaciones);
        //Esconde Ticks
        tickDerecha.transform.DOMove(posicionHideDerecha, 0.5f);
        tickIzquierda.transform.DOMove(posicionHideIzquierda, 0.5f);
        //Enseña Flechas
        flechaDerecha.transform.DOMove(posicionShowDerecha, 0.5f);
        flechaIzquierda.transform.DOMove(posicionShowIzquierda, 0.5f);
    }


    public void SetEstadoShowRespuestaDerecha()
    {
        flechaDerecha.transform.DOMove(posicionHideDerecha, 0.5f);
        tickDerecha.transform.DOMove(posicionShowDerecha, 0.5f);
    }

    public void SetEstadoShowRespuestaIzquierda()
    {
        flechaIzquierda.transform.DOMove(posicionHideIzquierda, 0.5f);
        tickIzquierda.transform.DOMove(posicionShowIzquierda, 0.5f);
    }

    public void SetEstadoShowExplicacion()
    {   
        //Esconde Flechas
        flechaDerecha.transform.DOMove(posicionHideDerecha, 0.5f);
        flechaIzquierda.transform.DOMove(posicionHideIzquierda, 0.5f);
        //Enseña Ticks
        tickDerecha.transform.DOMove(posicionShowDerecha, 0.5f);
        tickIzquierda.transform.DOMove(posicionShowIzquierda, 0.5f);
        //Esconde circulos variacion.
        dineroVariacion.transform.DOScale(0, easeOutVariaciones);
        socialVariacion.transform.DOScale(0, easeOutVariaciones);
        saludVariacion.transform.DOScale(0, easeOutVariaciones);
        especificoVariacion.transform.DOScale(0, easeOutVariaciones);

    }

    public void SetEstadoCommit()
    {
        //Esconde Flechas
        flechaDerecha.transform.DOMove(posicionHideDerecha, 0.5f);
        flechaIzquierda.transform.DOMove(posicionHideIzquierda, 0.5f);
        //Esconde Ticks
        tickDerecha.transform.DOMove(posicionHideDerecha, 0.5f);
        tickIzquierda.transform.DOMove(posicionHideIzquierda, 0.5f);
    }


}
