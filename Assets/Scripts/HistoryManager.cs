using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


public class HistoryManager : MonoBehaviour
{
    [SerializeField]
    private LocalizedAsset<TextAsset> jsonHistoria;

    private HistoryJsonRoot parsedHistorias;

    public void Start(){
        parsedHistorias = JsonUtility.FromJson<HistoryJsonRoot>(jsonHistoria.LoadAsset().text);
        
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
    public object siguiente;
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
