using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ModifyTextCargando : MonoBehaviour
{
    [SerializeField]private TMP_Text cargandoText;
    private string textoInicial= null;
    private short contador = 0;
    [SerializeField] private LocalizedString localizedString;

    // Start is called before the first frame update
    void Start()
    {   
        InvokeRepeating("anadePunto", 0.2f, 0.2f);

        PlayerPrefs.SetInt("TTSEnable", 0);
    }

    void anadePunto()
    {   
        if(textoInicial == null){
            textoInicial = localizedString.GetLocalizedString();
        }
        if(contador == 3){
            cargandoText.text = textoInicial;
            contador = 0;
        }else{
            cargandoText.text+=".";
            contador++;
        }
    }
}
