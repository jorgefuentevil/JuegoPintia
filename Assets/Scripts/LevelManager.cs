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

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject levelHolder;        //Panel padre de todos los niveles (LevelHolder en el editor).
    [SerializeField] private GameObject levelIcon;          //Creo que no lo necesitamos.
    [SerializeField] private GameObject thisCanvas;         //Canvas padre de toda la UI
    [SerializeField] private LocalizedAsset<TextAsset>  jsonHistorias;
    [SerializeField] private TextMeshProUGUI contadorText;
    [SerializeField] private GameObject flechaIzq;
    [SerializeField] private GameObject flechaDer;
    [SerializeField] private TextMeshProUGUI personajeText;
    [SerializeField] private TextMeshProUGUI descripcionText;
    [SerializeField] private AssetLabelReference assetsPersonajes;
    [SerializeField] private TTS textToSpeachManager;

    private LevelsJsonRoot parsedNiveles;
    private int numberOfLevels = 1;
    private Rect panelDimensions;
    private PageSwiper swiper;
    private Vector3 posFlechaDer;
    private Vector3 posFlechaIzq;


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
        posFlechaDer=flechaDer.transform.position;
        posFlechaIzq=flechaIzq.transform.position;


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

        flechaIzq.transform.DOMoveX(-700,2);

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
        contadorText.SetText(level_index + 1 + "/" + numberOfLevels);
        personajeText.SetText(parsedNiveles.historias[level_index].personaje);
        descripcionText.SetText(parsedNiveles.historias[level_index].desc);
        string textToSpeech = parsedNiveles.historias[level_index].personaje+"\n"+parsedNiveles.historias[level_index].desc;
        textToSpeachManager.StartSpeaking(textToSpeech);       
    }


    public void BindBtnDerecha()
    {
        Debug.Log("total lvl: "+numberOfLevels);        
        Vibracion();
        swiper.BindBtnDerecha();
        
        Debug.Log("lvl: "+swiper.getCurrentPage());
    }

    public void BindBtnIzquierda()
    {
        Debug.Log("lvl: "+swiper.getCurrentPage());
        Vibracion();
        swiper.BindBtnIzquierda();
        
    }

    public void JuegaHistoria()
    {
        //TODO: Aqui habría que pillar el nombre del fichero que contiene la historia.
        //La string que le pases al método da igual de momento.
        Vibracion();
        GameManager.Instance.CambiaEscenaGamePrincipal("este texto da igual xd");
    }

    public void Vibracion()
    {
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro cambiando el lvl");
        }
    }

    public GameObject getFlechaDer(){
        return flechaDer;
    }
    public GameObject getFlechaIzq(){
        return flechaIzq;
    }
    public Vector3 getPosFlechaDer(){
        return posFlechaDer;
    }
    public Vector3 getPosFlechaIzq(){
        return posFlechaIzq;
    }

}

[System.Serializable]
public struct Historia
{
    public string personaje;
    public string desc;
    public int coste;
    public string imagen;
}
[System.Serializable]
public struct LevelsJsonRoot
{
    public string lenguaje;
    public int num_historias;
    public Historia[] historias;
}
