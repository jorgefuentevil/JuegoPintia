using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using TMPro;


public class HistoryManager : MonoBehaviour
{
    [SerializeField]
    private LocalizedAsset<TextAsset> jsonHistoria;
    [SerializeField] private TextMeshProUGUI nombrePersonajeText;
    [SerializeField] private TextMeshProUGUI usuarioText;
    [SerializeField] private TextMeshProUGUI preguntaText;
        [SerializeField] private TextMeshProUGUI anosText;

    [SerializeField] private TextMeshProUGUI respuestaIText;
    [SerializeField] private TextMeshProUGUI respuestaDText;
    [SerializeField] private int nPreguntas;

    private HistoryJsonRoot parsedHistorias;
    [SerializeField] private int anosThreshold=5;
    private int nAnosIni=10;
    private List<Decision> selectedHistoria;

    
    public void Start(){
        
        //Cargamos todas las decisiones del json
        parsedHistorias = JsonUtility.FromJson<HistoryJsonRoot>(jsonHistoria.LoadAsset().text);
        string nombreHistoriaText = parsedHistorias.historia;
        //Escogemos nPreguntas al "azar"
        for(int i=0; i<nPreguntas; i++){
            int randomIndex = Random.Range(0, parsedHistorias.decisions.Count);
            Decision decisionAzar = parsedHistorias.decisions[randomIndex];
            if(parsedHistorias.decisions[randomIndex].usada){
                i=i-1;
            }else{
                selectedHistoria.Add(decisionAzar);
                if(decisionAzar.res_der.siguiente != -1){
                    selectedHistoria.Add(parsedHistorias.decisions[decisionAzar.res_der.siguiente]);
                }
                if(decisionAzar.res_izq.siguiente != -1){
                    selectedHistoria.Add(parsedHistorias.decisions[decisionAzar.res_izq.siguiente]);
                }
            }
        }
        //El numero de preguntas puede variar si alguna respuesta tiene otra deceision asociada

        nPreguntas = selectedHistoria.Count;
        Dictionary<string, Sprite> retratosPersonajes = new Dictionary<string, Sprite>();

        //Cargar imagenes en diccionarioii

        //cargar imagenes a distintas z o activar solo la q sale

        //Cargar nombre de usuario

        SetLevelData(0);
        
    }
    public void SetLevelData(int level_index)
    {
        if (level_index < 0 || level_index >= nPreguntas)
        {
            Debug.Log("Error Level Index out of bounds " + level_index + " of " + nPreguntas);
            return;
        }
        Decision decision = parsedHistorias.decisions[level_index];
        nombrePersonajeText.SetText(decision.imagen);
        preguntaText.SetText(decision.desc);
        respuestaIText.SetText(decision.res_izq.respuesta);
        respuestaDText.SetText(decision.res_der.respuesta);
        int anos=anosThreshold*level_index+nAnosIni;
        anosText.SetText(anos.ToString());
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
