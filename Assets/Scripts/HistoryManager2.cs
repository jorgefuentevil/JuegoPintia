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

public class HistoryManager2 : MonoBehaviour
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
    [SerializeField] private TTS textToSpeechManager;

    private Sprite spritecartaActual;
    private Image imagenCartaPersonaje;
    private int nPreguntas = 10;
    private List<Decision> decisionesPartida;
    private HistoryJsonRoot parsedHistorias;
    private AnswerSelector selector;





    private readonly Decision muerteDineroPoco = new(-1, "Mendigo_1", "muerte", "¡Has perdido todo tu dinero, eres una decepción para tu familia!", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));
    private readonly Decision muerteDineroMucho = new(-1, "Ladron_1", "muerte", "¡No puedes ir con tanto dinero por la calle! Un ladrón te roba en mitad de la noche", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));

    private readonly Decision muerteSocialPoco = new(-1, "tumba", "muerte", "Nadie te considera su amigo. Deberías comportarte mejor con el resto", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));
    private readonly Decision muerteSocialMucho = new(-1, "tumba", "muerte", "Eres una persona exitosa, pero tus enemigos te envidian. Eso nunca es bueno...", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));

    private readonly Decision muerteSaludPoco = new(-1, "tumba", "muerte", "Tienes que cuidarte más, con tan poca salud no llegarás a viejo", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));
    private readonly Decision muerteSaludMucho = new(-1, "tumba", "muerte", "Te creías el mas fuerte de la aldea, pero había alguien mejor que tú...", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));


    private readonly Decision muerteEspecificoPoco = new(-1, "tumba", "muerte", "Especifico Poco", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));
    private readonly Decision muerteEspecificoMucho = new(-1, "tumba", "muerte", "Especifico mucho", new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1), new Respuesta("Que...", new short[] { 0, 0, 0, 0 }, null, -1));


    public void Start()
    {
        imagenCartaPersonaje = cartaPersonaje.GetComponent<Image>();
        selector = cartaPersonaje.AddComponent<AnswerSelector>();
    }


    public void InicializaJuego()
    {
        CargaHistoria();
        LoadRetratos();

    }



    















    private void CargaHistoria()
    {
        string historiaToLoad = GameManager.Instance.currentLevel;
        historiaToLoad ??= "Tutorial";

        parsedHistorias = JsonConvert.DeserializeObject<HistoryJsonRoot>(jsonHistoria.text);

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

    private void LoadRetratos()
    {
        //Cargamos imágenes. 
        //TODO: Filtrar retratos y cargar solo los que vayamos a usar.
        Addressables.LoadAssetsAsync<Sprite>(assetsPersonajes, (sprite) =>
        {
            Debug.Log("Cargado retrato: " + sprite.name);
            retratosPersonajes.Add(sprite.name, sprite);
        }).WaitForCompletion();
    }
}
