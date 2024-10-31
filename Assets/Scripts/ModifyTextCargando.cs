using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModifyTextCargando : MonoBehaviour
{
    public TMP_Text cargandoText;
    // Start is called before the first frame update
    void Start()
    {
        cargandoText.text = "Cargando";
        InvokeRepeating("anadePunto", 0.2f, 0.2f);
    }

    // Update is called once per frame
    void anadePunto()
    {
        if(cargandoText.text == "Cargando...")
        {
            cargandoText.text = "Cargando";
        }
        else
        {
            cargandoText.text += ".";
        }
    }
    private void Update() {
        
    }
}
