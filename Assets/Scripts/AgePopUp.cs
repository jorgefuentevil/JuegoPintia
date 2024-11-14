using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgePopUp : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;

    // Método para seleccionar el rango de edad
    public void SelectAgeRange(int ageRange)
    {
        // Guarda el rango de edad en PlayerPrefs
        PlayerPrefs.SetInt("AgeRange", ageRange);
        PlayerPrefs.Save();                     // Guarda los valores
        Debug.Log("Rango de edad: "+ PlayerPrefs.GetInt("AgeRange"));
        popupPanel.SetActive(false);            // Cierra el popup

    }
}
