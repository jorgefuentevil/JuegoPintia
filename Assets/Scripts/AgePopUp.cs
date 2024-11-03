using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgePopUp : MonoBehaviour
{
    public GameObject popupPanel;

    // Start is called before the first frame update
    void Start()
    {
        // Verifica si es la primera vez que se abre el juego
        if (PlayerPrefs.HasKey("FirstTime") == false)
        {
            popupPanel.SetActive(true);         // Muestra el popUp
        }
        else
        {
            popupPanel.SetActive(false);        // No muestra el popUp
        }
        
    }

    // Método para seleccionar el rango de edad
    public void SelectAgeRange(int ageRange)
    {
        // Guarda el rango de edad en PlayerPrefs
        PlayerPrefs.SetInt("AgeRange", ageRange);
        PlayerPrefs.SetInt("FirstTime", 1);     // Guarda un valor arbitrario para indicar que no es la primera vez
        PlayerPrefs.Save();                     // Guarda los valores
        popupPanel.SetActive(false);            // Cierra el popup

    }
}
