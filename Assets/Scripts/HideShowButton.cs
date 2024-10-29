using UnityEngine;
using UnityEngine.UI;

public class HideShowObjects : MonoBehaviour
{
    public GameObject objectToHide;    // GameObject que queremos ocultar
    public GameObject objectToShow;    // GameObject que queremos mostrar
    void Start(){

    }
    public void OnButtonClick()
    {
        // Cambiamos la visibilidad de los GameObjects
        if (objectToHide != null && objectToShow != null)
        {
            objectToHide.SetActive(false);
            Debug.LogError("Cambio");
            objectToShow.SetActive(true);
        }
        else
        {
            Debug.LogError("Asegúrate de asignar ambos GameObjects en el Inspector.");
        }
    }
}