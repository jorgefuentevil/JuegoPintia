using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject levelHolder;        //Panel padre de todos los niveles (LevelHolder en el editor).
    [SerializeField] private GameObject levelIcon;          //Creo que no lo necesitamos.
    [SerializeField] private GameObject thisCanvas;         //Canvas padre de toda la UI
    [SerializeField] private TextAsset jsonHistorias;       //Json con las historias. TEMPORAL - Sustituir por addresable y locales.
    [SerializeField] private TextMeshProUGUI contadorText;
    [SerializeField] private TextMeshProUGUI personajeText;
    [SerializeField] private TextMeshProUGUI descripcionText;
    [SerializeField] private AssetLabelReference assetsPersonajes;

    private Dictionary<string, Sprite> retratosPersonajes = new Dictionary<string, Sprite>();
    private JsonRoot parsedHistorias;
    private int numberOfLevels = 1;
    private Rect panelDimensions;


    void Start()
    {
        //Cargamos Historias del Json
        parsedHistorias = JsonUtility.FromJson<JsonRoot>(jsonHistorias.text);
        numberOfLevels = parsedHistorias.num_historias;

        //Cargamos Sprites de personajes .....
        Addressables.LoadAssetsAsync<Sprite>(assetsPersonajes, (sprite) =>
        {
            Debug.Log("Cargado retrato: " + sprite.name);
            retratosPersonajes.Add(sprite.name, sprite);
        }).WaitForCompletion();

        Debug.Log(retratosPersonajes.Count);


        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        PageSwiper swiper = levelHolder.AddComponent<PageSwiper>();
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
            if(!retratosPersonajes.TryGetValue(parsedHistorias.historias[i].imagen, out aux)){
                Debug.Log("Error cargando retrato: " + parsedHistorias.historias[i].imagen);
                aux = retratosPersonajes["Agricultor_1"];
            }

            panel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = aux;
        }
        Destroy(panelClone);
        Destroy(levelHolder.transform.GetChild(0).gameObject);

        SetLevelData(0);

        Canvas.ForceUpdateCanvases();
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
        personajeText.SetText(parsedHistorias.historias[level_index].personaje);
        descripcionText.SetText(parsedHistorias.historias[level_index].desc);
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
public struct JsonRoot
{
    public string lenguaje;
    public int num_historias;
    public List<Historia> historias;
}




