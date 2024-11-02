using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicaEscenas : MonoBehaviour
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

    public void mostrarOpciones(){
        panelOpciones.vistaOpciones.SetActive(true);
    }
}
