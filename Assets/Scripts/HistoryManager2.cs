using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HistoryManager2 : MonoBehaviour
{   
    [Header("---- ASSETS ----")]
    [SerializeField] private TextAsset jsonHistoria;
    [SerializeField] private TextAsset jsonTutorial;
    [SerializeField] private AssetLabelReference assetsPersonajes;
    [SerializeField] private Sprite reversoCarta;
    [SerializeField] private Sprite cartaExplicacion;
    private readonly Dictionary<string, Sprite> retratosPersonajes = new();





    public void InicializaJuego(){
        
    }
}
