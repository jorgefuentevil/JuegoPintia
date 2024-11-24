using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.Localization.SmartFormat;
using Newtonsoft.Json;




public class HistoryManager : MonoBehaviour
{

    [Header("---- ASSETS ----")]
    [SerializeField] private TextAsset jsonHistoria;
    [SerializeField] private TextAsset jsonTutorial;
    [SerializeField] private AssetLabelReference assetsPersonajes;
    [SerializeField] private Sprite reversoCarta;
    [SerializeField] private Sprite cartaExplicacion;
    private readonly Dictionary<string, Sprite> retratosPersonajes = new();




    [Header("---- UI GAME OBJECTS ----")]
    [SerializeField] private TextMeshProUGUI nombrePersonajeText;
    [SerializeField] private TextMeshProUGUI usuarioText;
    [SerializeField] private TextMeshProUGUI preguntaText;
    [SerializeField] private TextMeshProUGUI anosText;
    [SerializeField] private TextMeshProUGUI respuestaText;
    [SerializeField] private TextMeshProUGUI explicacionText;
    [SerializeField] private GameObject cartaPersonaje;
    [SerializeField] private GameObject flechaDerecha;
    [SerializeField] private GameObject flechaIzquierda;
    [SerializeField] private GameObject tickDerecho;
    [SerializeField] private GameObject tickIzquierdo;
    [SerializeField] private GameObject popUpMuertePanel;
    [SerializeField] private TTS textToSpeachManager;

    private Vector3 posicionFlechaDerecha;
    private Vector3 posicionFlechaIzquierda;
    private Vector3 posicionTickDerecha;
    private Vector3 posicionTickIzquierda;
    
    private Sprite cartaActual;
    private Image imagenCartaPersonaje;


    [Header("---- ICONOS SUPERIORES")]
    [SerializeField] private IconManager iconManager;


    //Parámetros de la partida//
    private readonly short puntuacionMax = 20;
    private short puntuacionDinero = 10;
    private short puntuacionSocial = 10;
    private short puntuacionSalud = 10;
    private short puntuacionEspecifico = 10;


    private int nPreguntas = 10;
    private HistoryJsonRoot parsedHistorias;
    private List<Decision> decisionesPartida;   //Almacena la lista de decisiones aleatorias con las que jugamos.
    private Decision decisionActual;            //Almacena la decisión actual. Puede ser decisión o respuesta a otra decision.
    private int numDecisionActual;              //Almacena el index de la decision actual. Solo decision, no respuestas. Solo incrementar en DECISIONES nuevas.
    private Vector3 posicionInicial;
    private AnswerSelector selector;


    public CardType tipoCartaActual;

    private EndType tipoFin;

    private readonly Color sombreadoCarta = new(0.4078431f, 0.4078431f, 0.4078431f);
    private readonly Color colorNormal = new(1, 1, 1);




    private enum EndType
    {
        DINERO,
        SOCIAL,
        SALUD,
        ESPECIFICO,
        VICTORIA
    }


    private Decision muerteDinero = new Decision(-1,"tumba","muerte","¡Has perdido todo tu dinero, eres una decepción para tu familia!", new Respuesta("Que...", new short[] {0,0,0,0},null,-1), new Respuesta("Que...", new short[] {0,0,0,0},null,-1));



    public void Start()
    {
        posicionFlechaDerecha=flechaDerecha.transform.position;
        posicionFlechaIzquierda=flechaIzquierda.transform.position;
        posicionTickIzquierda=tickIzquierdo.transform.position;
        posicionTickDerecha=tickDerecho.transform.position;

        imagenCartaPersonaje = cartaPersonaje.GetComponent<Image>();
        posicionInicial = cartaPersonaje.transform.position;
        tipoCartaActual = CardType.NORMAL;

        //Cargamos todas las decisiones del json 
        if (PlayerPrefs.HasKey("Tutorial"))
        {
            parsedHistorias = JsonConvert.DeserializeObject<HistoryJsonRoot>(jsonHistoria.text);
        }
        else
        {
            parsedHistorias = JsonConvert.DeserializeObject<HistoryJsonRoot>(jsonTutorial.text);
            PlayerPrefs.SetInt("Tutorial", 1);
        }

        //Elegimos si queremos preguntas aleatorias o no.
        if (parsedHistorias.aleatoria)
        {
            CargaPreguntasAleatorias();
        }
        else
        {
            CargaAllPreguntas();
        }
        Debug.LogFormat("Cargada historia: {0}; Tiene {1} historias; NumHistorias={2}", parsedHistorias.historia, parsedHistorias.decisiones.Count, nPreguntas);


        //Cargamos imágenes. 
        //TODO: Filtrar retratos y cargar solo los que vayamos a usar.
        Addressables.LoadAssetsAsync<Sprite>(assetsPersonajes, (sprite) =>
        {
            Debug.Log("Cargado retrato: " + sprite.name);
            retratosPersonajes.Add(sprite.name, sprite);
        }).WaitForCompletion();


        selector = cartaPersonaje.AddComponent<AnswerSelector>();
        selector.historyManager = this;

        decisionActual = decisionesPartida[0];

        SetTextosDecision();

        SetEstadoDefault();
    }

    private void CargaPreguntasAleatorias()
    {
        //Con cuantas preguntas aleatorias vamos a jugar
        nPreguntas = (nPreguntas > parsedHistorias.decisiones.Count) ? parsedHistorias.decisiones.Count : nPreguntas;
        int[] indexHistorias = Enumerable   //Genera n numeros aleatorios entre 0 y X.
                                .Range(0, parsedHistorias.decisiones.Count)
                                .OrderBy(x => Random.value)
                                .Take(nPreguntas).ToArray();

        //Llenamos decisionesPartida con nPreguntas decisiones aleatorias.
        decisionesPartida = new(nPreguntas);
        for (int i = 0; i < nPreguntas; i++)
        {
            decisionesPartida.Add(parsedHistorias.decisiones[indexHistorias[i]]);
        }
    }

    private void CargaAllPreguntas()
    {
        nPreguntas = parsedHistorias.decisiones.Count;
        decisionesPartida = new(nPreguntas);
        decisionesPartida.AddRange(parsedHistorias.decisiones);
    }

    private void SetTextosDecision()
    {
        cartaActual = retratosPersonajes[decisionActual.imagen];
        nombrePersonajeText.text = decisionActual.personaje;
        preguntaText.text = decisionActual.desc;
        anosText.SetText(numDecisionActual + " Años de Aventura");
        if (PlayerPrefs.GetInt("TTSEnable") == 1)
        {
            textToSpeachManager.StartSpeaking(decisionActual.desc);
        }
    }


    public void ShowRespuestaDerecha()
    {
        respuestaText.text = decisionActual.res_der.respuesta;
        //flechaIzquierda.SetActive(false);
        imagenCartaPersonaje.DOColor(sombreadoCarta, 0.2f);
        imagenCartaPersonaje.sprite = reversoCarta;
        iconManager.PreviewEfectos(decisionActual.res_der.efectos);
        if (PlayerPrefs.GetInt("TTSEnable") == 1)
        {
            textToSpeachManager.StartSpeaking(decisionActual.res_der.respuesta);
        }
    }

    public void ShowRespuestaIzquierda()
    {
        respuestaText.text = decisionActual.res_izq.respuesta;
        //flechaDerecha.SetActive(false);
        imagenCartaPersonaje.DOColor(sombreadoCarta, 0.2f);
        imagenCartaPersonaje.sprite = reversoCarta;
        iconManager.PreviewEfectos(decisionActual.res_izq.efectos);
        Debug.Log(PlayerPrefs.GetInt("TTSEnable"));
        if (PlayerPrefs.GetInt("TTSEnable") == 1)
        {
            textToSpeachManager.StartSpeaking(decisionActual.res_izq.respuesta);
        }
    }

    public void ConfirmaRespuestaDerecha()
    {
        ConfirmaRespuesta(decisionActual.res_der.efectos);
        //CheckFinPartida();
        ChangeNextDecision(decisionActual.res_der);
        anosText.SetText(numDecisionActual + 1 + " Años de Aventura");
    }


    public void ConfirmaRespuestaIzquierda()
    {   
        if(tipoCartaActual == CardType.NORMAL){
            ConfirmaRespuesta(decisionActual.res_izq.efectos);
            //CheckFinPartida();
            ChangeNextDecision(decisionActual.res_izq);
            anosText.SetText(numDecisionActual + 1 + " Años de Aventura");
        }
    }

    private void ConfirmaRespuesta(short[] efectos)
    {
        cartaPersonaje.SetActive(false); // Escondemos carta

        iconManager.AplicaEfectos(efectos, puntuacionMax);
        UpdatePuntuacion(efectos);

        respuestaText.text = "";
        imagenCartaPersonaje.color = colorNormal;
        //Devolvemos al centro
        cartaPersonaje.transform.position = posicionInicial;
        imagenCartaPersonaje.sprite = reversoCarta;

        cartaPersonaje.SetActive(true);
    }

    public void SetEstadoDefault()
    {
        respuestaText.text = "";
        explicacionText.text = "";
        //flechaIzquierda.SetActive(true);
        //flechaDerecha.SetActive(true);
        imagenCartaPersonaje.DOColor(colorNormal, 0.2f);
        imagenCartaPersonaje.sprite = cartaActual;
        iconManager.SetEstadoDefault();
        selector.SetEstadoDefault();
    }

    public void bindBtnDerecha(){
        selector.bindBtnDerecha();
    }

    public void bindBtnIzquierda(){
        selector.bindBtnIzquierda();
    }

    public void ShowTickDerecha(){

        flechaDerecha.transform.DOMove(posicionTickDerecha, 2);
        tickDerecho.transform.DOMove(posicionFlechaDerecha, 2);
    }

    public void HideTickDerecha(){
        tickDerecho.transform.DOMove(posicionTickDerecha, 2);
        flechaDerecha.transform.DOMove(posicionFlechaDerecha, 2);;
    }

    public void ShowTickIzquierda(){
        flechaIzquierda.transform.DOMove(posicionTickIzquierda, 2);
        tickIzquierdo.transform.DOMove(posicionFlechaIzquierda, 2);
    }

    public void HideTickIzquierda(){
        flechaIzquierda.transform.DOMove(posicionFlechaIzquierda, 2);
        tickIzquierdo.transform.DOMove(posicionTickIzquierda, 2);
    }

    public void SetEstadoExplicacion(string textoExplicacion)
    {
        respuestaText.text = "";
        explicacionText.text = textoExplicacion;
        imagenCartaPersonaje.sprite = cartaActual;
        iconManager.SetEstadoDefault();
        //flechaIzquierda.SetActive(true);
        //flechaDerecha.SetActive(true);
        imagenCartaPersonaje.DOColor(sombreadoCarta, 0.2f);
        if (PlayerPrefs.GetInt("TTSEnable") == 1)
        {
            textToSpeachManager.StartSpeaking(textoExplicacion);
        }
    }

    private void UpdatePuntuacion(short[] efectos)
    {
        puntuacionDinero += efectos[0];
        puntuacionSocial += efectos[1];
        puntuacionSalud += efectos[2];
        puntuacionEspecifico += efectos[3];
        Debug.LogFormat("Puntuacion Actual: {0} - {1} - {2} - {3}", puntuacionDinero, puntuacionSocial, puntuacionSalud, puntuacionEspecifico);
    }

    private bool CheckFinPartida()
    {
        if (puntuacionDinero >= puntuacionMax || puntuacionDinero <= 0)
        {
            tipoFin = EndType.DINERO;
            return true;

        }
        else if (puntuacionSocial >= puntuacionMax || puntuacionSocial <= 0)
        {
            tipoFin = EndType.SOCIAL;
            return true;

        }
        else if (puntuacionSalud >= puntuacionMax || puntuacionSalud <= 0)
        {
            tipoFin = EndType.SALUD;
            return true;

        }
        else if (puntuacionEspecifico >= puntuacionMax || puntuacionEspecifico <= 0)
        {
            tipoFin = EndType.ESPECIFICO;
            return true;

        }
        else if (numDecisionActual == nPreguntas - 1)
        {
            tipoFin = EndType.VICTORIA;
            return true;
        }
        return false;
    }
    private void ChangeNextDecision(Respuesta respuesta)
    {
        //Carta normal con explicación -> Mostrar explicacion
        if (tipoCartaActual == CardType.NORMAL && respuesta.explicacion != null)
        {
            tipoCartaActual = CardType.EXPLICACION;
            cartaActual = cartaExplicacion;
            StartCoroutine(selector.RotateAndExplicacion(respuesta.explicacion));
        }
        //Carta normal sin explicación / Explicacion -> Mostrar siguiente carta
        else if ((tipoCartaActual == CardType.NORMAL && respuesta.explicacion == null) || tipoCartaActual == CardType.EXPLICACION)
        {
            cartaPersonaje.transform.eulerAngles = new Vector3(0, 180, 0);
            explicacionText.text = "";
            //Checkeamos fin de partida
            if (CheckFinPartida())
            {
                decisionActual = muerteDinero;  //Habría que comprobar que tipo de muerte es.
                tipoCartaActual = CardType.PRE_MUERTE;
            }
            else
            {

                decisionActual = (respuesta.siguiente != -1) ? parsedHistorias.decisiones_respuesta[respuesta.siguiente] : decisionesPartida[++numDecisionActual];
                tipoCartaActual = CardType.NORMAL;
            }
            SetTextosDecision();
            StartCoroutine(selector.RotateAndDefault());
        }
        else if(tipoCartaActual == CardType.PRE_MUERTE){
            popUpMuertePanel.SetActive(true);
        }
    }
}


public struct Decision
{
    public Decision(int _id, string _imagen, string _personaje, string _desc, Respuesta _res_der, Respuesta _res_izq)
    {
        id = _id;
        imagen = _imagen;
        personaje = _personaje;
        desc = _desc;
        res_der = _res_der;
        res_izq = _res_izq;
    }

    public int id;
    public string imagen;
    public string personaje;
    public string desc;
    public Respuesta res_der;
    public Respuesta res_izq;
}

public struct Respuesta
{   
    public Respuesta(string _respuesta, short[] _efectos, string _explicacion, int _siguiente){
        respuesta = _respuesta;
        efectos = _efectos;
        explicacion = _explicacion;
        siguiente = _siguiente;
    }

    public string respuesta;
    public short[] efectos;
    public string explicacion;
    public int siguiente;
}



public struct HistoryJsonRoot
{
    public string historia;
    public string idioma;
    public int nivel;
    public bool aleatoria;
    public List<Decision> decisiones;
    public List<Decision> decisiones_respuesta;
}




