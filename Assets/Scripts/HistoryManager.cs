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
    [SerializeField] private AssetLabelReference assetsPersonajes;
    [SerializeField] private Sprite reversoCarta;


    [Header("---- UI GAME OBJECTS ----")]
    [SerializeField] private TextMeshProUGUI nombrePersonajeText;
    [SerializeField] private TextMeshProUGUI usuarioText;
    [SerializeField] private TextMeshProUGUI preguntaText;
    [SerializeField] private TextMeshProUGUI anosText;
    [SerializeField] private TextMeshProUGUI respuestaText;
    [SerializeField] private GameObject cartaPersonaje;
    [SerializeField] private GameObject flechaDerecha;
    [SerializeField] private GameObject flechaIzquierda;
    [SerializeField] private GameObject popUpMuertePanel;

    [Header("---- ICONOS SUPERIORES")]
    [SerializeField] private IconManager iconManager;
    private AnswerSelector selector;

    //Parámetros de la partida//
    private readonly short puntuacionMax = 20;
    private short puntuacionDinero = 10;
    private short puntuacionSocial = 10;
    private short puntuacionSalud = 10;
    private short puntuacionEspecifico = 10;


    private int nPreguntas = 10;
    Dictionary<string, Sprite> retratosPersonajes = new();
    private HistoryJsonRoot parsedHistorias;
    private List<Decision> decisionesPartida;   //Almacena la lista de decisiones aleatorias con las que jugamos.
    private Decision decisionActual;            //Almacena la decisión actual. Puede ser decisión o respuesta a otra decision.
    private int numDecisionActual;              //Almacena el index de la decision actual. Solo decision, no respuestas. Solo incrementar en DECISIONES nuevas.
    private Sprite cartaActual;
    private Image imagenCartaPersonaje;
    private readonly Color sombreadoCarta = new Color(0.4078431f, 0.4078431f, 0.4078431f);
    private readonly Color colorNormal = new Color(1, 1, 1);
    private int preguntaAct=0;




    public void Start()
    {
        imagenCartaPersonaje = cartaPersonaje.GetComponent<Image>();

        //Cargamos todas las decisiones del json 
        parsedHistorias = JsonConvert.DeserializeObject<HistoryJsonRoot>(jsonHistoria.text);

        //Elegimos si queremos preguntas aleatorias o no.
        if (parsedHistorias.aleatoria)
        {
            CargaPreguntasAleatorias();
        }
        else{
            CargaAllPreguntas();            
        }
        Debug.LogFormat("Cargada historia: {0}; Tiene {1} historias; NumHistorias={2}",parsedHistorias.historia,parsedHistorias.decisiones.Count,nPreguntas);


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

    private void CargaAllPreguntas(){
        Debug.Log(parsedHistorias.decisiones.Count);
        nPreguntas = parsedHistorias.decisiones.Count;
        decisionesPartida = new(nPreguntas);
        decisionesPartida.AddRange(parsedHistorias.decisiones);
    }

    private void SetTextosDecision(){
        cartaActual = retratosPersonajes[decisionActual.imagen];
        nombrePersonajeText.text = decisionActual.personaje;
        preguntaText.text = decisionActual.desc;
    }


    public void ShowRespuestaDerecha()
    {
        respuestaText.text = decisionActual.res_der.respuesta;
        flechaIzquierda.SetActive(false);
        imagenCartaPersonaje.DOColor(sombreadoCarta, 0.2f);
        imagenCartaPersonaje.sprite = reversoCarta;
        iconManager.PreviewEfectos(decisionActual.res_der.efectos);
    }

    public void ShowRespuestaIzquierda()
    {
        respuestaText.text = decisionActual.res_der.respuesta;
        flechaDerecha.SetActive(false);
        imagenCartaPersonaje.DOColor(sombreadoCarta, 0.2f);
        imagenCartaPersonaje.sprite = reversoCarta;
        iconManager.PreviewEfectos(decisionActual.res_izq.efectos);
    }

    public void ConfirmaRespuestaDerecha()
    {
        iconManager.AplicaEfectos(decisionActual.res_der.efectos,puntuacionMax);
        UpdatePuntuacion(decisionActual.res_der.efectos);
        CheckFinPartida();
        ChangeNextDecision();
    }

    public void ConfirmaRespuestaIzquierda()
    {
        iconManager.AplicaEfectos(decisionActual.res_izq.efectos,puntuacionMax);
        UpdatePuntuacion(decisionActual.res_izq.efectos);
        CheckFinPartida();
        ChangeNextDecision();
    }

    public void SetEstadoDefault()
    {
        respuestaText.text = "";
        flechaIzquierda.SetActive(true);
        flechaDerecha.SetActive(true);
        imagenCartaPersonaje.DOColor(colorNormal, 0.2f);
        imagenCartaPersonaje.sprite = cartaActual;
        iconManager.SetEstadoDefault();
        selector.SetEstadoDefault();
    }

    private void UpdatePuntuacion(short[] efectos){
        puntuacionDinero += efectos[0];
        puntuacionSocial += efectos[1];
        puntuacionSalud += efectos[2];
        puntuacionEspecifico += efectos[3];
        Debug.LogFormat("Puntuacion Actual: {0} - {1} - {2} - {3}", puntuacionDinero, puntuacionSocial, puntuacionSalud, puntuacionEspecifico);
    }

    private void CheckFinPartida(){
        if (new[] { puntuacionDinero, puntuacionSocial, puntuacionSalud, puntuacionEspecifico }.Any(valor => valor <= 0 || valor >= puntuacionMax))
            popUpMuertePanel.SetActive(true);
        
    }
    private void ChangeNextDecision(){
        preguntaAct+=1;
        decisionActual = decisionesPartida[preguntaAct];
        SetTextosDecision();
        SetEstadoDefault();
    }
}


public struct Decision
{
    public int id;
    public string imagen;
    public string personaje;
    public string desc;
    public ResDer res_der;
    public ResIzq res_izq;
}

public struct DecisionAnswer
{
    public int id;
    public string imagen;
    public string desc;
    public ResDer res_der;
    public ResIzq res_izq;
}

public struct ResDer
{
    public string respuesta;
    public short[] efectos;
    public string explicacion;
    public int siguiente;
}

public struct ResIzq
{
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
    public List<DecisionAnswer> decision_answers;
}




