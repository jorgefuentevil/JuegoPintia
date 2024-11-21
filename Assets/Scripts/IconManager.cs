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

    [SerializeField] private GameObject popupFinPartida;

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
        PreviewEfectoIndividual(dineroVariacion,stats[0]);
        PreviewEfectoIndividual(socialVariacion,stats[1]);
        PreviewEfectoIndividual(saludVariacion,stats[2]);
        PreviewEfectoIndividual(especificoVariacion,stats[3]);
    }

    private void PreviewEfectoIndividual(GameObject variacion, short stat){
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

        checkFinParida();
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
        seq.Insert(1, contornoImage.DOColor(colorBlanco, 1.8f));
        seq.Insert(1, fillImage.DOColor(colorBlanco, 1.8f));
    }


    public void SetEstadoDefault()
    {
        dineroVariacion.transform.DOScale(0, easeOutVariaciones);
        socialVariacion.transform.DOScale(0, easeOutVariaciones);
        saludVariacion.transform.DOScale(0, easeOutVariaciones);
        especificoVariacion.transform.DOScale(0, easeOutVariaciones);
    }

    public void checkFinParida()
    {
        if (new[] { dineroFill, socialFill, saludFill, especificoFill }.Any(fill => fill.fillAmount == 0 || fill.fillAmount == 1))
            popupFinPartida.SetActive(true);
    }

}
