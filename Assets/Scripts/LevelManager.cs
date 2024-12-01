using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using TMPro;
using UnityEngine.Localization;
using System.Linq;
using DG.Tweening;
using CandyCoded.HapticFeedback;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject levelHolder;        //Panel padre de todos los niveles (LevelHolder en el editor).
    [SerializeField] private GameObject thisCanvas;         //Canvas padre de toda la UI
    [SerializeField] private LocalizedAsset<TextAsset>  jsonHistorias;
    [SerializeField] private TextMeshProUGUI contadorText;
    [SerializeField] private GameObject flechaIzq;
    [SerializeField] private GameObject flechaDer;
    [SerializeField] private TextMeshProUGUI personajeText;
    [SerializeField] private TextMeshProUGUI descripcionText;
    [SerializeField] private AssetLabelReference assetsPersonajes;

    [Header("---- Boton Jugar ----")]
    [SerializeField] private TextMeshProUGUI textoBotonJugar;
    [SerializeField] private Image candadoImagen;
    [SerializeField] private Button botonJugarObjeto;

    [Header("---- TTS Manager ----")]
    [SerializeField] private TTS textToSpeechManager;

    private LevelsJsonRoot parsedNiveles;
    private int numberOfLevels = 1;
    private Rect panelDimensions;
    private PageSwiper swiper;
    private PauseMenu pause;
    private GameObject botonFinPartida; 

    private float posShowFlechaDer;
    private float posShowFlechaIzq;
    private float posHideFlechaDer;
    private float posHideFlechaIzq;


    public void Start()
    {      
    
        //Cargamos Historias del Json
        parsedNiveles = JsonUtility.FromJson<LevelsJsonRoot>(jsonHistorias.LoadAsset().text);
        
        numberOfLevels = parsedNiveles.num_historias;
        Dictionary<string, Sprite> retratosPersonajes = new Dictionary<string, Sprite>();


        //Cargamos Sprites de personajes .....
        Addressables.LoadAssetsAsync<Sprite>(assetsPersonajes, (sprite) =>
        {
            Debug.Log("Cargado retrato: " + sprite.name);
            retratosPersonajes.Add(sprite.name, sprite);
        }).WaitForCompletion();

        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        swiper = levelHolder.AddComponent<PageSwiper>();
        swiper.totalPages = numberOfLevels;
        swiper.levelManager = this;
        panelDimensions = levelHolder.GetComponent<RectTransform>().rect;

        for (int i = 0; i < numberOfLevels; i++)
        {
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(thisCanvas.transform, false);
            panel.transform.SetParent(levelHolder.transform);
            panel.name = "Page-" + (i + 1);
            panel.GetComponent<RectTransform>().localPosition = new Vector2(panelDimensions.width * (i), 0);

            Sprite aux;
            if(!retratosPersonajes.TryGetValue(parsedNiveles.historias[i].imagen, out aux)){
                Debug.Log("Error cargando retrato: " + parsedNiveles.historias[i].imagen);
                aux = retratosPersonajes.ElementAt(0).Value;
            }

            panel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = aux;
        }

        posShowFlechaDer = flechaDer.transform.position.x;
        posShowFlechaIzq = flechaIzq.transform.position.x;

        posHideFlechaDer = posShowFlechaDer + 100;
        posHideFlechaIzq = posShowFlechaIzq - 100;

        flechaDer.transform.position = new(posHideFlechaDer,flechaDer.transform.position.y);

        Destroy(panelClone);
        Destroy(levelHolder.transform.GetChild(0).gameObject);

        SetLevelData(0);

        Canvas.ForceUpdateCanvases();

        GameObject.FindGameObjectWithTag("MainMenuManager").GetComponent<MainMenuEstadoInicial>().TerminaTransicion();

    }

    //level_index es [0,numLvl-1]
    public void SetLevelData(int level_index)
    {
        if (level_index < 0 || level_index >= numberOfLevels)
        {
            Debug.Log("Error Level Index out of bounds " + level_index + " of " + numberOfLevels);
            return;
        }
        
        flechaIzq.transform.DOMoveX(level_index <= 0 ? posHideFlechaIzq : posShowFlechaIzq, 0.5f);
        flechaDer.transform.DOMoveX(level_index >= numberOfLevels - 1 ? posHideFlechaDer : posShowFlechaDer, 0.5f);

        BotonJugarManager(level_index);

        contadorText.SetText(level_index + 1 + "/" + numberOfLevels);
        personajeText.SetText(parsedNiveles.historias[level_index].personaje);
        descripcionText.SetText(parsedNiveles.historias[level_index].desc);
        string textToSpeech = parsedNiveles.historias[level_index].personaje+"\n"+parsedNiveles.historias[level_index].desc;
        textToSpeechManager.StartSpeaking(textToSpeech);       
    }

    private void BotonJugarManager(int level_index)
    {   
        //Está desbloqueado
        if(GameManager.Instance.CheckLevelStatus(level_index))
        {
            textoBotonJugar.color = Color.white;
            botonJugarObjeto.interactable = true;
            candadoImagen.color = new Color(0f,0f,0f,0f);
        }
        else
        {
            textoBotonJugar.color = new Color(0f,0f,0f,0f);
            botonJugarObjeto.interactable = false;
            candadoImagen.color = Color.white;
        }
    }


    public void BindFlechaDerecha()
    {
        Vibracion();
        swiper.BindFlechaDerecha();
        
    }

    public void BindFlechaIzquierda()
    {
        Vibracion();
        swiper.BindFlechaIzquierda();
    }

    public void JuegaHistoria()
    {   
        if(!GameManager.Instance.CheckLevelStatus(swiper.currentPage-1)) return;
        Vibracion();
        StartCoroutine(AuxTransicion());
        botonFinPartida=pause.getBtnFinPartida();
        botonFinPartida.SetActive(true);
    }

    private IEnumerator AuxTransicion()
    {   
        GameObject.FindGameObjectWithTag("MainMenuManager").GetComponent<MainMenuEstadoInicial>().EmpiezaTransicion();
        yield return new WaitForSeconds(1);
        GameManager.Instance.CambiaEscenaGamePrincipal(parsedNiveles.historias[swiper.currentPage-1].archivo);
    }

    public void Vibracion()
    {
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro cambiando el lvl");
        }
    }

}

[System.Serializable]
public struct Historia
{
    public string personaje;
    public string desc;
    public int coste;
    public string imagen;
    public string atributo;
    public string archivo;
}
[System.Serializable]
public struct LevelsJsonRoot
{
    public string lenguaje;
    public int num_historias;
    public Historia[] historias;
}
