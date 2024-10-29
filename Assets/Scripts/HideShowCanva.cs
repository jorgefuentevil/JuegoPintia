using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideShowImage : MonoBehaviour
{
   public GameObject objectToHide;      // object que queremos ocultar al hacer clic
    public GameObject objectToShow;      // object que queremos mostrar al hacer clic
    private Button button;             // Imagen que escucha el clic

    void Start()
    {
        // Obtenemos el componente Image en el objeto donde está asignado este script
        button = GetComponent<Button>();
        // Añadimos el listener al componente Image
        
    }

    void OnImageClick()
    {
        // Cambiamos la visibilidad de los object
        objectToHide.gameObject.SetActive(false);
        objectToShow.gameObject.SetActive(true);
    }
}
