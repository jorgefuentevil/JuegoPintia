using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicaOpciones : MonoBehaviour
{
    public controladorEscenas panelOpciones;
    // Start is called before the first frame update
    void Start()
    {
        panelOpciones=GameObject.FindGameObjectWithTag("opciones").GetComponent<controladorEscenas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowOpciones(){
        panelOpciones.vistaOpciones.SetActive(true);
    }
}
