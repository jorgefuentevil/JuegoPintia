using UnityEngine;
using UnityEngine.UI;

public class HideShowCanva : MonoBehaviour
{
    public GameObject objectToHide;    // Objeto que queremos ocultar
    public GameObject objectToShow;    // Objeto que queremos mostrar
    private Button button;             // Botón para controlar la visibilidad

    void Start()
    {
        // Intentamos obtener el componente Button y mostramos un mensaje en caso de error
        button = GetComponent<Button>();

        // Asignamos la función al evento onClick del botón
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        Debug.Log("Se ha hecho clic en el botón.");
        // Ocultamos el objeto objectToHide y mostramos el objeto objectToShow
        objectToHide.SetActive(false);
        objectToShow.SetActive(true);
    }
}