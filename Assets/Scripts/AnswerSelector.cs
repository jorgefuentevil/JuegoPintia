using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerSelector : MonoBehaviour, IDragHandler, IEndDragHandler
{

    [Header("---- Parámetros Animaciones ----")]
    [SerializeField] private float percentThreshold = 0.5f;
    [SerializeField] private float easing = 0.5f;

    private Vector3 imageLocation;
    private CardState estadoCarta;

    public HistoryManager historyManager;



    private enum CardState
    {
        INICIAL,
        FLIPPED_DERECHA,
        FLIPPED_IZQUIERDA,
        SWIPED_DERECHA,
        SWIPED_IZQUIERDA
    }


    public void Start()
    {
        imageLocation = transform.position;
        estadoCarta = CardState.INICIAL;

    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        if ((estadoCarta.Equals(CardState.FLIPPED_DERECHA) && difference < 0) || (estadoCarta.Equals(CardState.FLIPPED_IZQUIERDA) && difference > 0))
        {
            transform.position = imageLocation - new Vector3(difference, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            //Desliza hacia la derecha
            if (percentage < 0)
            {
                GestionarSwipeDerecha();
            }
            //Desliza hacia la izquierda
            else
            {
                GestionarSwipeIzquierda();
            }
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, imageLocation, easing));
        }
    }

    private void GestionarSwipeDerecha()
    {
        switch (estadoCarta)
        {
            case CardState.INICIAL: //Muestra atributos y respuesta derecha
                StartCoroutine(RotateAndShowDerecha());
                Debug.Log("Carta rotada a la derecha");
                estadoCarta = CardState.FLIPPED_DERECHA;
                break;

            case CardState.FLIPPED_IZQUIERDA:   //Vuelve al estado inicial.
                StartCoroutine(RotateAndHide());
                Debug.Log("Volvemos a Inicial desde izquierda");
                estadoCarta = CardState.INICIAL;
                break;

            case CardState.FLIPPED_DERECHA:     //Confirmamos seleccion derecha
                Debug.Log("Confirmamos Derecha");
                //estadoCarta = CardState.SWIPED_DERECHA;
                break;

            default:
                Debug.Log("Error: Estado Carta desconocido");
                break;
        }
    }

    private void GestionarSwipeIzquierda()
    {
        switch (estadoCarta)
        {
            case CardState.INICIAL: //Muestra atributos y respuesta derecha
                StartCoroutine(RotateAndShowIzquierda());
                Debug.Log("Carta rotada a la izquierda");
                estadoCarta = CardState.FLIPPED_IZQUIERDA;
                break;

            case CardState.FLIPPED_DERECHA:             //Vuelve al estado inicial.
                StartCoroutine(RotateAndHide());
                Debug.Log("Volvemos a Inicial desde derecha");
                estadoCarta = CardState.INICIAL;
                break;

            case CardState.FLIPPED_IZQUIERDA:           //Confirmamos seleccion izquierda
                //estadoCarta = CardState.SWIPED_IZQUIERDA;
                Debug.Log("Confirmamos Izquierda");
                break;

            default:
                Debug.Log("Error: Estado Carta desconocido");
                break;
        }
    }


    IEnumerator RotateAndShowDerecha()
    {
        float t = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 180f, 0) * startRotation;
        bool respuestaActiva = false;

        while (t <= 1f)
        {
            t += Time.deltaTime / 0.67f;  // 0:40 segundos
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.SmoothStep(0f, 1f, t));

            if (!respuestaActiva && t >= 0.5f)
            {
                respuestaActiva = true;
                historyManager.SetRespuestaDerecha();
            }
            yield return null;
        }
        Debug.Log("Rotación terminada");
    }


    IEnumerator RotateAndShowIzquierda()
    {
        float t = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 180f, 0) * startRotation;
        bool respuestaActiva = false;

        while (t <= 1f)
        {
            t += Time.deltaTime / 0.67f;  // 0:40 segundos
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.SmoothStep(0f, 1f, t));

            if (!respuestaActiva && t >= 0.5f)
            {
                respuestaActiva = true;
                historyManager.SetRespuestaIzquierda();
            }
            yield return null;
        }
        Debug.Log("Rotación terminada");
    }

    IEnumerator RotateAndHide()
    {
        float t = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 180f, 0) * startRotation;
        bool respuestaActiva = true;

        while (t <= 1f)
        {
            t += Time.deltaTime / 0.67f;  // 0:40 segundos
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.SmoothStep(0f, 1f, t));

            if (respuestaActiva && t >= 0.5f)
            {
                //RespuestaGeneral.SetActive(false);
                historyManager.SetEstadoInicial();
                respuestaActiva = false;
            }
            yield return null;
        }
        Debug.Log("Rotación terminada");
    }



    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        Debug.Log("Terminada");
    }
}