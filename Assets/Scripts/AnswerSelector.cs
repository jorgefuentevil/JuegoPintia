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
    private CardState estadoCarta;

    public HistoryManager historyManager;

    private AudioManager audioManager;


    private enum CardState
    {
        INICIAL,
        FLIPPED_DERECHA,
        FLIPPED_IZQUIERDA,
        SWIPED_DERECHA,
        SWIPED_IZQUIERDA
    }

    private void Awake(){
        // Encuentra el GameObject con el tag "AudioManager" y obtiene el componente AudioManager
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

    }


    public void Start()
    {
        imageLocation = transform.position;
        estadoCarta = CardState.INICIAL;

    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        if ((estadoCarta == CardState.FLIPPED_DERECHA && difference < 0) || (estadoCarta == CardState.FLIPPED_IZQUIERDA && difference > 0) || historyManager.tipoCartaActual == CardType.EXPLICACION)
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


    public void bindBtnDerecha(){
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro en btn drcha");
        }
        GestionarSwipeDerecha();
    }

    public void bindBtnIzquierda(){
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro en btn izq");
        }
        GestionarSwipeIzquierda();
    }

    private void GestionarSwipeDerecha()
    {

        if (historyManager.tipoCartaActual == CardType.NORMAL)
        {
            switch (estadoCarta)
            {
                case CardState.INICIAL: //Muestra atributos y respuesta derecha
                    StartCoroutine(RotateAndShowDerecha());
                    
                    Debug.Log("Carta rotada a la derecha");
                    estadoCarta = CardState.FLIPPED_DERECHA;
                    break;

                case CardState.FLIPPED_IZQUIERDA:   //Vuelve al estado inicial.
                    StartCoroutine(RotateAndDefault());

                    Debug.Log("Volvemos a Inicial desde izquierda");
                    estadoCarta = CardState.INICIAL;
                    break;

                case CardState.FLIPPED_DERECHA:     //Confirmamos seleccion derecha
                    Debug.Log("Confirmamos Derecha");
                    //estadoCarta = CardState.SWIPED_DERECHA;
                    
                    StartCoroutine(SlideDesapareceDerecha());
                    break;

                default:
                    Debug.Log("Error: Estado Carta desconocido");
                    break;
            }
        }
        else
        {
            StartCoroutine(SlideDesapareceDerecha());
        }
    }

    private void GestionarSwipeIzquierda()
    {
        if (historyManager.tipoCartaActual == CardType.NORMAL)
        {
            switch (estadoCarta)
            {
                case CardState.INICIAL: //Muestra atributos y respuesta izquierda
                    StartCoroutine(RotateAndShowIzquierda());
                    
                    Debug.Log("Carta rotada a la izquierda");
                    estadoCarta = CardState.FLIPPED_IZQUIERDA;
                    break;

                case CardState.FLIPPED_DERECHA:             //Vuelve al estado inicial.
                    StartCoroutine(RotateAndDefault());
                    
                    Debug.Log("Volvemos a Inicial desde derecha");
                    estadoCarta = CardState.INICIAL;
                    break;

                case CardState.FLIPPED_IZQUIERDA:           //Confirmamos seleccion izquierda
                                                            //estadoCarta = CardState.SWIPED_IZQUIERDA;
                    Debug.Log("Confirmamos Izquierda");
                    StartCoroutine(SlideDesapareceIzquierda());


                    break;

                default:
                    Debug.Log("Error: Estado Carta desconocido");
                    break;
            }
        }
        else
        {
            StartCoroutine(SlideDesapareceIzquierda());
        }
    }

    IEnumerator SlideDesapareceDerecha()
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
        historyManager.ConfirmaRespuestaDerecha();
        historyManager.HideTickDerecha();

    }

    IEnumerator SlideDesapareceIzquierda()
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

        historyManager.ConfirmaRespuestaIzquierda();
        historyManager.HideTickIzquierda();
    }


    IEnumerator RotateAndShowDerecha()
    {
        float t = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 180f, 0) * startRotation;
        bool respuestaActiva = false;
        audioManager.PlayFlipSFX();
        historyManager.ShowTickDerecha();
        while (t <= 1f)
        {
            t += Time.deltaTime / 0.67f;  // 0:40 segundos
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.SmoothStep(0f, 1f, t));

            if (!respuestaActiva && t >= 0.5f)
            {
                respuestaActiva = true;
                historyManager.ShowRespuestaDerecha();
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
        audioManager.PlayFlipSFX();
        historyManager.ShowTickIzquierda();
        while (t <= 1f)
        {
            t += Time.deltaTime / 0.67f;  // 0:40 segundos
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.SmoothStep(0f, 1f, t));

            if (!respuestaActiva && t >= 0.5f)
            {
                respuestaActiva = true;
                historyManager.ShowRespuestaIzquierda();
            }
            yield return null;
        }
        Debug.Log("Rotación terminada");
    }

    public IEnumerator RotateAndDefault()
    {
        float t = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 180f, 0) * startRotation;
        bool respuestaActiva = true;
        audioManager.PlayFlipSFX();
        if(estadoCarta == CardState.FLIPPED_DERECHA)
            historyManager.HideTickDerecha();
        else
            historyManager.HideTickIzquierda();
        while (t <= 1f)
        {
            t += Time.deltaTime / 0.67f;  // 0:40 segundos
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.SmoothStep(0f, 1f, t));

            if (respuestaActiva && t >= 0.5f)
            {
                historyManager.SetEstadoDefault();
                respuestaActiva = false;
            }
            yield return null;
        }
        Debug.Log("Rotación terminada");
    }


    public IEnumerator RotateAndExplicacion(string explicacion)
    {
        float t = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 180f, 0) * startRotation;
        bool respuestaActiva = true;
        audioManager.PlayFlipSFX();
        while (t <= 1f)
        {
            t += Time.deltaTime / 0.67f;  // 0:40 segundos
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.SmoothStep(0f, 1f, t));

            if (respuestaActiva && t >= 0.5f)
            {
                historyManager.SetEstadoExplicacion(explicacion);
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



    public void SetEstadoDefault()
    {
        //transform.position = imageLocation;
        estadoCarta = CardState.INICIAL;
    }
}