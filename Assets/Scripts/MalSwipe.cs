using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwipeToMoveImage : MonoBehaviour
{
    // Velocidad de movimiento de la imagen
    public float swipeSpeed = 1.0f;

    // Distancia mínima para reconocer el swipe
    public float swipeThreshold = 50.0f;

    // Posiciones fijas para derecha e izquierda
    public Vector2 fixedPositionRight = new Vector2(500, 0);
    public Vector2 fixedPositionLeft = new Vector2(-500, 0);
    public Vector2 fixedPositionCenter= new Vector2(0,0);

    // Nombres de las escenas para cambiar
    public string rightSceneName;
    public string leftSceneName;

    // Variables internas
    private Vector2 startTouchPosition, endTouchPosition;
    private RectTransform imageRectTransform;

    void Start()
    {
        // Obtener el componente RectTransform de la imagen
        imageRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Detectar toque en dispositivos táctiles
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Registrar la posición inicial del toque
                startTouchPosition = touch.position;
                Debug.Log("Pulso en : " + startTouchPosition);

            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // Obtener la posición de movimiento del toque
                endTouchPosition = touch.position;

                // Calcular la diferencia entre la posición inicial y final
                Vector2 difference = endTouchPosition - startTouchPosition;
                
                // Determinar si el swipe supera el umbral
                endTouchPosition = touch.position;
                Vector2 swipeVector = endTouchPosition - startTouchPosition;

                if (Mathf.Abs(swipeVector.x) >= swipeThreshold)
                {   
                    if (swipeVector.x > swipeThreshold)
                    {
                        // Mover a la posición fija a la derecha
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionRight, swipeSpeed * Time.deltaTime);

                    }
                    // Si se desliza a la izquierda
                    else if(swipeVector.x < -swipeThreshold)
                    {
                        // Mover a la posición fija a la izquierda
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionLeft, swipeSpeed * Time.deltaTime);
                    }
                }else{
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
                    }
                

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("Solto en : " + endTouchPosition);
                Vector2 swipeVector = endTouchPosition - startTouchPosition;
                // Si se desliza a la derecha
                if (swipeVector.x > swipeThreshold)
                {
                    Debug.Log("Cambiando a: " + rightSceneName);
                    // Mover a la posición fija a la derecha
                    imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionRight, swipeSpeed * Time.deltaTime);
                    // Cambiar a la escena de la derecha
                    SceneManager.LoadScene(rightSceneName);
                }
                // Si se desliza a la izquierda
                else if (swipeVector.x < -swipeThreshold)
                {
                    Debug.Log("Cambiando a: " + leftSceneName);
                    // Mover a la posición fija a la izquierda
                    imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionLeft, swipeSpeed * Time.deltaTime);
                    // Cambiar a la escena de la izquierda
                    SceneManager.LoadScene(leftSceneName);
                }else{
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
                }
            }
        }
        else if (Input.GetMouseButton(0)) // Alternativa para pruebas en el editor usando el mouse
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
                Debug.Log("Pulso en : " + startTouchPosition);

            }
            else if (Input.GetMouseButton(0))
            {
                endTouchPosition = Input.mousePosition;
                Vector2 swipeVector = endTouchPosition - startTouchPosition;                
                if (Mathf.Abs(swipeVector.x) >= swipeThreshold)
                {
                    if (swipeVector.x > swipeThreshold)
                    {
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionRight, swipeSpeed * Time.deltaTime);
                    }
                    else if(swipeVector.x < -swipeThreshold)
                    {
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionLeft, swipeSpeed * Time.deltaTime);
                    }
                }else{
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
                    }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                endTouchPosition = Input.mousePosition;
                Vector2 swipeVector = endTouchPosition - startTouchPosition;
                Debug.Log("Solto en : " + endTouchPosition);
                if (Mathf.Abs(swipeVector.x) >= swipeThreshold)
                {
                    if (swipeVector.x > swipeThreshold)
                    {
                        Debug.Log("Cambiando a: " + rightSceneName);
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionRight, swipeSpeed * Time.deltaTime);
                        SceneManager.LoadScene(rightSceneName);
                    }
                    else if(swipeVector.x < -swipeThreshold)
                    {
                        Debug.Log("Cambiando a: " + leftSceneName);
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionLeft, swipeSpeed * Time.deltaTime);
                        SceneManager.LoadScene(leftSceneName);
                    }
                    else{
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
                    }
                }else{
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
                    }
            }
        }
    }
}