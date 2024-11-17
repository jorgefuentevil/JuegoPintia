using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using TMPro;
using System.Linq;
using UnityEngine.AddressableAssets;




public class HistoryManager : MonoBehaviour
{   

    [Header("---- ASSETS ----")]
    [SerializeField] private LocalizedAsset<TextAsset> jsonHistoria;
    [SerializeField] private AssetLabelReference assetsPersonajes;

    [Header("---- UI GAME OBJECTS ----")]
    [SerializeField] private TextMeshProUGUI nombrePersonajeText;
    [SerializeField] private TextMeshProUGUI usuarioText;
    [SerializeField] private TextMeshProUGUI preguntaText;
    [SerializeField] private TextMeshProUGUI anosText;
    [SerializeField] private TextMeshProUGUI respuestaText;
    [SerializeField] private GameObject cartaPersonaje;
    [SerializeField] private GameObject flechaDerecha;
    [SerializeField] private GameObject flechaIzquierda;




    private int nPreguntas = 10;
    private HistoryJsonRoot parsedHistorias;
    private List<Decision> decisionesPartida;   //Almacena la lista de decisiones aleatorias con las que jugamos.
    private Decision decisionActual;            //Almacena la decisión actual. Puede ser decisión o respuesta a otra decision.
    private int numDecisionActual;              //Almacena el index de la decision actual. Solo decision, no respuestas. Solo incrementar en DECISIONES nuevas.


    public void Start()
    {   

        //Comentar desde aqui

/*         //Cargamos todas las decisiones del json
        parsedHistorias = JsonUtility.FromJson<HistoryJsonRoot>(jsonHistoria.LoadAsset().text);

        //Con cuantas preguntas aleatorias vamos a jugar
        nPreguntas = (nPreguntas > parsedHistorias.decisions.Count) ? parsedHistorias.decisions.Count : nPreguntas;
        int[] indexHistorias = Enumerable   //Genera n numeros aleatorios entre 0 y X.
                                .Range(0, parsedHistorias.decisions.Count)
                                .OrderBy(x => Random.value)
                                .Take(nPreguntas).ToArray();

        //Llenamos decisionesPartida con nPreguntas decisiones aleatorias.
        decisionesPartida = new(nPreguntas);
        for(int i = 0; i<nPreguntas; i++){
            decisionesPartida.Add(parsedHistorias.decisions[indexHistorias[i]]);
        } */

        //Hasta aqui. Si queremos probar funcionamiento sin tener historias.
        
        //Cargamos imágenes. 
        //TODO: Filtrar retratos y cargar solo los que vayamos a usar.
        Dictionary<string, Sprite> retratosPersonajes = new();
        Addressables.LoadAssetsAsync<Sprite>(assetsPersonajes, (sprite) =>
        {
            Debug.Log("Cargado retrato: " + sprite.name);
            retratosPersonajes.Add(sprite.name, sprite);
        }).WaitForCompletion();


        AnswerSelector selector = cartaPersonaje.AddComponent<AnswerSelector>();
        selector.historyManager = this;

        SetEstadoInicial();
    }




    public void SetRespuestaDerecha(){
        respuestaText.text = "Texto de la respuesta de la DERECHA DERECHAAAA";
        flechaIzquierda.SetActive(false);

    }

    public void SetRespuestaIzquierda(){
        respuestaText.text = "Texto de la respuesta de la IZQUIERDA IZQUIERDAAAAA";
        flechaDerecha.SetActive(false);



    }

    public void SetEstadoInicial(){
        respuestaText.text = "";
        flechaIzquierda.SetActive(true);
        flechaDerecha.SetActive(true);

    }
}









[System.Serializable]
public struct Decision
{
    public int id;
    public string imagen;
    public string desc;
    public bool usada;
    public ResDer res_der;
    public ResIzq res_izq;
}

[System.Serializable]
public struct ResDer
{
    public string respuesta;
    public int[] efectos;
    public int siguiente;
}

[System.Serializable]
public struct ResIzq
{
    public string respuesta;
    public int[] efectos;
    public int siguiente;
}

[System.Serializable]
public struct RespuestaDecision
{
    public int id;
    public string imagen;
    public string desc;
    public bool usada;
    public ResDer res_der;
    public ResIzq res_izq;
}

[System.Serializable]
public struct HistoryJsonRoot
{
    public string historia;
    public string imagen;
    public string idioma;
    public int nivel;
    public List<Decision> decisions;
    public List<RespuestaDecision> respuesta_decisions;
}
