using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using CandyCoded.HapticFeedback;

public class AnswerSelector : MonoBehaviour, IDragHandler, IEndDragHandler
{


    private readonly float percentThreshold = 0.3f;
    private readonly float easing = 0.5f;

    private Vector3 imageLocation;

    public HistoryManager historyManager;
    public MaquinaEstadosCartas maquinaEstados;

    private AudioManager audioManager;


    private void Awake()
    {
        // Encuentra el GameObject con el tag "AudioManager" y obtiene el componente AudioManager
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        imageLocation = transform.position;
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        Debug.Log(difference);
        if ((maquinaEstados.EstaShowRespuestaDerecha() && difference < 0) ||
            (maquinaEstados.EstaShowRespuestaIzquierda() && difference > 0) ||
            maquinaEstados.EstaShowExplicacion()||
            maquinaEstados.EstaPreMuerte())
        {
            transform.position = imageLocation - new Vector3(difference, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        Debug.Log(percentage);
        if (Mathf.Abs(percentage) >= percentThreshold)
        {

            if (PlayerPrefs.GetInt("VibracionEnabled") == 1)
            {
                HapticFeedback.HeavyFeedback();
                Debug.Log("vibro hacia la der");
            }

            //Desliza hacia la derecha
            if (percentage < 0)
            {
                Debug.Log("Deslizamos derecha");
                GestionarSwipeDerecha();
            }
            //Desliza hacia la izquierda
            else
            {
                Debug.Log("Deslizamos izquierda");
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

        if (PlayerPrefs.GetInt("VibracionEnabled") == 1)
        {
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro hacia la der");
        }

        if (maquinaEstados.EstaEligeCarta())
        {
            //Cambiamos a Show_Respuesta_Derecha
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.SHOW_RESPUESTA_DERECHA);
        }
        else if (maquinaEstados.EstaShowRespuestaDerecha())
        {
            //Cambiamos a Commit_Respuesta_Derecha
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.COMMIT_RESPUESTA_DERECHA);
        }
        else if (maquinaEstados.EstaShowRespuestaIzquierda())
        {
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.ELIGE_CARTA);
        }
        else if (maquinaEstados.EstaShowExplicacion())
        {
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.COMMIT_EXPLICACION_DERECHA);
        }
        else if(maquinaEstados.EstaPreMuerte())
        {
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.COMMIT_MUERTE_DERECHA);
        }


    }

    private void GestionarSwipeIzquierda()
    {

        if (PlayerPrefs.GetInt("VibracionEnabled") == 1)
        {
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro hacia la der");
        }

        if (maquinaEstados.EstaEligeCarta())
        {
            //Cambiamos a Show_Respuesta_Izquierda
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.SHOW_RESPUESTA_IZQUIERDA);
        }
        else if (maquinaEstados.EstaShowRespuestaIzquierda())
        {
            //Cambiamos a Commit_Respuesta_Izquierda
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.COMMIT_RESPUESTA_IZQUIERDA);

        }
        else if (maquinaEstados.EstaShowRespuestaDerecha())
        {
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.ELIGE_CARTA);
        }
        else if (maquinaEstados.EstaShowExplicacion())
        {
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.COMMIT_EXPLICACION_IZQUIERDA);
        }
        else if(maquinaEstados.EstaPreMuerte())
        {
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.COMMIT_MUERTE_IZQUIERDA);
        }


    }

    public void ShowEligeCarta()
    {
        StartCoroutine(RotateAndAccion(historyManager.SetEstadoEligeCarta));
    }

    public void ShowRespuestaDerecha()
    {
        StartCoroutine(RotateAndAccion(historyManager.SetEstadoShowRespuestaDerecha));
    }

    public void ShowRespuestaIzquierda()
    {
        StartCoroutine(RotateAndAccion(historyManager.SetEstadoShowRespuestaIzquierda));
    }

    public void CommitRespuestaDerecha()
    {
        StartCoroutine(SlideAndActionDerecha(historyManager.SetEstadoCommitRespuestaDerecha));
    }

    public void CommitRespuestaIzquierda()
    {
        StartCoroutine(SlideAndActionIzquierda(historyManager.SetEstadoCommitRespuestaIzquierda));
    }

    public void ShowExplicacion()
    {
        StartCoroutine(RotateAndAccion(historyManager.SetEstadoShowExplicacion));
    }

    public void CommitExplicacionDerecha()
    {
        StartCoroutine(SlideAndActionDerecha(historyManager.SetEstadoCommitExplicacion));
    }

    public void CommitExplicacionIzquierda()
    {
        StartCoroutine(SlideAndActionIzquierda(historyManager.SetEstadoCommitExplicacion));
    }

    public void CommitMuerteDerecha()
    {
        StartCoroutine(SlideAndActionDerecha(historyManager.SetEstadoCommitMuerte));
    }

    public void CommitMuerteIzquierda()
    {
        StartCoroutine(SlideAndActionIzquierda(historyManager.SetEstadoCommitMuerte));
    }



    public void RightArrowPressed()
    {
        GestionarSwipeDerecha();
    }

    public void LeftArrowPressed()
    {
        GestionarSwipeIzquierda();
    }

    public void RightCheckPressed()
    {
        GestionarSwipeDerecha();
    }

    public void LeftCheckPressed()
    {
        GestionarSwipeIzquierda();
    }


    private IEnumerator RotateAndAccion(Action funcionMitad)
    {
        float t = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 180f, 0) * startRotation;
        bool respuestaActiva = false;
        audioManager.PlayFlipSFX();

        while (t <= 1f)
        {
            t += Time.deltaTime / 0.67f;  // 0:40 segundos
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.SmoothStep(0f, 1f, t));

            if (!respuestaActiva && t >= 0.5f)
            {
                respuestaActiva = true;
                funcionMitad?.Invoke();
            }
            yield return null;
        }

        maquinaEstados.AvisaFinalAnimacion();
    }

    private IEnumerator SlideAndActionDerecha(Action funcionFinal)
    {

        Vector3 startpos = transform.position;
        Vector3 endpos = new(transform.position.x + Screen.width, transform.position.y - Screen.height / 2, transform.position.z);
        float t = 0f;
        audioManager.PlaySlideSFX();
        while (t <= 1.0)
        {
            //TODO: Cambiar easing, añadir diferentes easings para cada animacion.
            t += Time.deltaTime / easing;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        funcionFinal?.Invoke();
    }

    private IEnumerator SlideAndActionIzquierda(Action funcionFinal)
    {

        Vector3 startpos = transform.position;
        Vector3 endpos = new(transform.position.x - Screen.width, transform.position.y - Screen.height / 2, transform.position.z);
        float t = 0f;
        audioManager.PlaySlideSFX();

        while (t <= 1.0)
        {
            //TODO: Cambiar easing, añadir diferentes easings para cada animacion.
            t += Time.deltaTime / easing;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        funcionFinal?.Invoke();
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