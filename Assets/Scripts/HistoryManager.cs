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
using UnityEngine.ResourceManagement.AsyncOperations;
using Unity.VisualScripting;
using UnityEngine.Localization;

public class HistoryManager : MonoBehaviour
{
    [Header("---- ASSETS ----")]
    [SerializeField] private TextAsset jsonHistoriaFallback;
    [SerializeField] private LocalizedAsset<TextAsset> historiaTutorial;
    [SerializeField] private AssetLabelReference assetsPersonajes;
    [SerializeField] private Sprite spriteReversoCarta;
    [SerializeField] private Sprite spriteCartaExplicacion;
    private readonly Dictionary<string, Sprite> retratosPersonajes = new();


    [Header("---- UI GAME OBJECTS ----")]
    [SerializeField] private TextMeshProUGUI nombrePersonajeText;
    [SerializeField] private TextMeshProUGUI usuarioText;
    [SerializeField] private TextMeshProUGUI preguntaText;
    [SerializeField] private TextMeshProUGUI anosText;
    [SerializeField] private TextMeshProUGUI respuestaText;
    [SerializeField] private TextMeshProUGUI explicacionText;
    [SerializeField] private GameObject cartaPersonaje;
    [SerializeField] private PopUpFinalPartida popupFin;


    [SerializeField] private TTS textToSpeechManager;

    [Header("--- ICON MANAGER ----")]
    [SerializeField] private IconManager iconManager;

    private int nPreguntas = 10;
    private HistoryJsonRoot parsedHistorias;
    private List<Decision> decisionesPartida;   //Almacena la lista de decisiones aleatorias con las que jugamos.
    private Decision decisionActual;            //Almacena la decisión actual. Puede ser decisión o respuesta a otra decision.
    private int numDecisionActual;              //Almacena el index de la decision actual. Solo decision, no respuestas. Solo incrementar en DECISIONES nuevas.
    private Vector3 posicionInicial;
    private Respuesta respuestaActual;


    [System.NonSerialized] public AnswerSelector selector;
    [System.NonSerialized] public MaquinaEstadosCartas maquinaEstados;

    private Sprite spriteCartaActual;
    private Image imagenCartaPersonaje;

    //Parámetros de la partida//
    private readonly short puntuacionMax = 20;
    private short puntuacionDinero;
    private short puntuacionSocial;
    private short puntuacionSalud;
    private short puntuacionEspecifico;


    private readonly Color sombreadoCarta = new(0.4078431f, 0.4078431f, 0.4078431f);
    private readonly Color colorNormal = new(1, 1, 1);

    private bool trueIfVictoria = false;



    private readonly Decision muerteDineroPoco = new(-1, "Mendigo_1", "muerte", "¡Has perdido todo tu dinero, eres una decepción para tu familia y tus amigos!", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));
    private readonly Decision muerteDineroMucho = new(-1, "Ladron_1", "muerte", "¡No puedes ir con tanto dinero por la calle! Un ladrón te roba en mitad de la noche", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));

    private readonly Decision muerteSocialPoco = new(-1, "tumba", "muerte", "Nadie te considera su amigo. Deberías comportarte mejor con el resto", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));
    private readonly Decision muerteSocialMucho = new(-1, "tumba", "muerte", "Eres una persona exitosa, pero tus enemigos te envidian. Eso nunca es bueno...", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));

    private readonly Decision muerteSaludPoco = new(-1, "tumba", "muerte", "Tienes que cuidarte más, con tan poca salud no llegarás a viejo", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));
    private readonly Decision muerteSaludMucho = new(-1, "tumba", "muerte", "Te creías el mas fuerte de la aldea, pero había alguien mejor que tú...", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));


    private readonly Decision muerteEspecificoPoco = new(-1, "tumba", "muerte", "Especifico Poco", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));
    private readonly Decision muerteEspecificoMucho = new(-1, "tumba", "muerte", "Especifico mucho", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));

    private readonly Decision decisionVictoria = new(-1, "tumba", "VICTORIA", "Victoria suuuuu", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));



    public void Start()
    {
        imagenCartaPersonaje = cartaPersonaje.GetComponent<Image>();
        posicionInicial = cartaPersonaje.transform.position;

        selector = cartaPersonaje.AddComponent<AnswerSelector>();
        selector.historyManager = this;


        maquinaEstados = gameObject.AddComponent<MaquinaEstadosCartas>();
        maquinaEstados.historyManager = this;
        maquinaEstados.selector = selector;
        maquinaEstados.iconManager = iconManager;

        selector.maquinaEstados = maquinaEstados;
        iconManager.selector = selector;


        puntuacionDinero = puntuacionSocial = puntuacionSalud = puntuacionEspecifico = (short)(puntuacionMax / 2);

        maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.INICIALIZANDO);

    }


    public void SetEstadoInicializando()
    {
        CargaHistoria();
        AsyncOperationHandle<IList<Sprite>> asyncOperationHandle = LoadRetratos();

        decisionActual = decisionesPartida[0];

        imagenCartaPersonaje.color = colorNormal;
        imagenCartaPersonaje.sprite = spriteReversoCarta;
        respuestaText.text = "";
        explicacionText.text = "";

        GameObject.FindGameObjectWithTag("GamePrincipalManager").GetComponent<GamePrincipalEstadoInicial>().TerminaTransicion();


        //TODO: Meter aqui animacion de inicio

        asyncOperationHandle.WaitForCompletion();
        SetElementosDecision();

    }


    private void SetElementosDecision()
    {
        //TODO: ¿ABSTRAER A UN ASSET MANAGER?
        spriteCartaActual = retratosPersonajes[decisionActual.imagen];
        nombrePersonajeText.text = decisionActual.personaje;
        preguntaText.text = decisionActual.desc;
        //TODO: ESTA STRING DEBERÍA ESTAR LOCALIZADA
        anosText.text = $"{numDecisionActual} Años de Aventura";
    }


    public void SetEstadoEligeCarta()
    {
        iconManager.SetEstadoEligeCarta();
        respuestaText.text = "";
        explicacionText.text = "";
        imagenCartaPersonaje.DOColor(colorNormal, 0.2f);
        imagenCartaPersonaje.sprite = spriteCartaActual;
        //TODO: TTS HABLA DESCRIPCION
    }


    public void SetEstadoShowRespuestaDerecha()
    {
        respuestaText.text = decisionActual.res_der.respuesta;
        imagenCartaPersonaje.DOColor(sombreadoCarta, 0.2f);
        imagenCartaPersonaje.sprite = spriteReversoCarta;
        iconManager.PreviewEfectos(decisionActual.res_der.efectos);
        //TODO: TTS HABLA RESPUESTA
    }

    public void SetEstadoShowRespuestaIzquierda()
    {
        respuestaText.text = decisionActual.res_izq.respuesta;
        imagenCartaPersonaje.DOColor(sombreadoCarta, 0.2f);
        imagenCartaPersonaje.sprite = spriteReversoCarta;
        iconManager.PreviewEfectos(decisionActual.res_izq.efectos);
        //TODO: TTS HABLA RESPUESTA
    }

    public void SetEstadoCommitRespuestaDerecha()
    {
        respuestaActual = decisionActual.res_der;
        CommitRespuesta();

    }

    public void SetEstadoCommitRespuestaIzquierda()
    {
        respuestaActual = decisionActual.res_izq;
        CommitRespuesta();

    }

    private void CommitRespuesta()
    {
        cartaPersonaje.SetActive(false);

        iconManager.AplicaEfectos(respuestaActual.efectos, puntuacionMax);
        UpdatePuntuacion(respuestaActual.efectos);

        respuestaText.text = "";
        explicacionText.text = "";

        imagenCartaPersonaje.color = colorNormal;
        cartaPersonaje.transform.position = posicionInicial;
        imagenCartaPersonaje.sprite = spriteReversoCarta;
        cartaPersonaje.SetActive(true);

        ChangeNextCarta();


    }

    public void SetEstadoCommitExplicacion()
    {
        cartaPersonaje.SetActive(false);

        explicacionText.text = "";
        respuestaText.text = "";

        imagenCartaPersonaje.color = colorNormal;
        cartaPersonaje.transform.SetPositionAndRotation(posicionInicial, Quaternion.identity);
        imagenCartaPersonaje.sprite = spriteReversoCarta;


        cartaPersonaje.SetActive(true);

        ChangeNextCarta();
    }

    public void SetEstadoShowExplicacion()
    {
        respuestaText.text = "";
        explicacionText.text = respuestaActual.explicacion;
        imagenCartaPersonaje.sprite = spriteReversoCarta;
        imagenCartaPersonaje.DOColor(sombreadoCarta, 0.2f);
        //TODO: IconManager estado Explicacion????
        //TODO: TTS HABLA EXPLICACION
    }

    public void SetEstadoCommitMuerte()
    {
        Debug.Log($"Fin de Partida por {(trueIfVictoria ? "victoria" : "derrota")}");
        popupFin.MostrarPopup(trueIfVictoria);
    }

    private void CargaHistoria()
    {
        string historiaToLoad = GameManager.Instance.currentLevel;

        AsyncOperationHandle<TextAsset> opHandle = historiaToLoad.Equals("Tutorial") ? historiaTutorial.LoadAssetAsync<TextAsset>() : Addressables.LoadAssetAsync<TextAsset>(historiaToLoad);

        opHandle.WaitForCompletion();

        TextAsset assetHistoria = (opHandle.Status == AsyncOperationStatus.Succeeded) ? opHandle.Result : jsonHistoriaFallback;

        parsedHistorias = JsonConvert.DeserializeObject<HistoryJsonRoot>(assetHistoria.text);

        if (parsedHistorias.aleatoria)
        {
            CargaPreguntasAleatorias();
        }
        else
        {
            CargaAllPreguntas();
        }
        Debug.LogFormat("Cargada historia: {0}; Tiene {1} historias; NumHistorias={2}", parsedHistorias.historia, parsedHistorias.decisiones.Count, nPreguntas);
    }

    private void CargaPreguntasAleatorias()
    {
        //Con cuantas preguntas aleatorias vamos a jugar
        nPreguntas = (nPreguntas > parsedHistorias.decisiones.Count) ? parsedHistorias.decisiones.Count : nPreguntas;
        List<int> indexHistorias = Enumerable   //Genera n numeros aleatorios entre 0 y X.
                                .Range(0, parsedHistorias.decisiones.Count)
                                .OrderBy(x => Random.value)
                                .Take(nPreguntas).ToList();

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

    private AsyncOperationHandle<IList<Sprite>> LoadRetratos()
    {
        //Cargamos imágenes. 
        //TODO: Filtrar retratos y cargar solo los que vayamos a usar.
        return Addressables.LoadAssetsAsync<Sprite>(assetsPersonajes, (sprite) =>
        {
            Debug.Log("Cargado retrato: " + sprite.name);
            retratosPersonajes.Add(sprite.name, sprite);
        });
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
            decisionActual = (puntuacionDinero >= puntuacionMax) ? muerteDineroMucho : muerteDineroPoco;
            return true;
        }

        else if (puntuacionSocial >= puntuacionMax || puntuacionSocial <= 0)
        {
            decisionActual = (puntuacionSocial >= puntuacionMax) ? muerteSocialMucho : muerteSocialPoco;
            return true;
        }

        else if (puntuacionSalud >= puntuacionMax || puntuacionSalud <= 0)
        {
            decisionActual = (puntuacionSalud >= puntuacionMax) ? muerteSaludMucho : muerteSaludPoco;
            return true;
        }

        else if (puntuacionEspecifico >= puntuacionMax || puntuacionEspecifico <= 0)
        {
            decisionActual = (puntuacionEspecifico >= puntuacionMax) ? muerteEspecificoMucho : muerteEspecificoPoco;
            return true;
        }
        //Priorizamos derrota a victoria....
        else if (numDecisionActual >= nPreguntas - 1)
        {
            trueIfVictoria = true;
            decisionActual = decisionVictoria;
            return true;
        }

        return false;
    }




    private void ChangeNextCarta()
    {
        /*4 Opciones
            - Normal con explicacion -> Mostrar Explicacion
            - FinPartida -> Sacar tarjeta muerte
            - Normal sin explicacion con respuesta
            - Normal sin explicacion sin respuesta
        */

        //Si tiene explicacion y no la está enseñando -> Enseña explicación
        if (respuestaActual.explicacion != null && !maquinaEstados.EstaShowExplicacion())
        {
            spriteCartaActual = spriteCartaExplicacion;
            //TODO: Set Elementos decision????
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.SHOW_EXPLICACION);
        }
        //Acabamos de hacer commit de una decisión -> Checkeamos si hemos ganado/perdido
        else if (CheckFinPartida())
        {
            //decision actual ya es la carta de victoria/derrota.
            SetElementosDecision();
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.PRE_MUERTE);
        }
        //Si tiene decision encadenada -> Pillamos la decision encadenada.
        else if (respuestaActual.siguiente != -1) //Comprobar si está commiting o no???
        {

            //Confiamos en que de verdad existe la siguiente decision xddd🍆🍆🍆
            decisionActual = parsedHistorias.decisiones_respuesta[respuestaActual.siguiente];
            SetElementosDecision();
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.ELIGE_CARTA);
        }
        //Si no tiene decision encadenada -> Pillamos la siguiente de la lista.
        else if (respuestaActual.siguiente == -1)
        {
            decisionActual = decisionesPartida[++numDecisionActual];
            Debug.Log($"Decision {numDecisionActual} de {nPreguntas}");
            SetElementosDecision();
            maquinaEstados.CambiarDeEstado(MaquinaEstadosCartas.GameState.ELIGE_CARTA);
        }
    }
}
