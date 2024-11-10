using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModifyTextCargando : MonoBehaviour
{
    [SerializeField]private TMP_Text cargandoText;
    private string textoInicial = null;
    private short contador = 0;

    // Start is called before the first frame update
    void Start()
    {   
        InvokeRepeating("anadePunto", 0.2f, 0.2f);
    }

    void anadePunto()
    {   
        if(textoInicial == null){
            textoInicial = cargandoText.text;
            Debug.Log(textoInicial);
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
