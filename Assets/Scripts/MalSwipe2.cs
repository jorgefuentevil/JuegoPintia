using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeToMoveImage2 : MonoBehaviour{

    public float swipeSpeed = 20.0f;
    public float swipeThreshold = 150.0f;
    private Vector2 swipeVector;
    public Vector2 fixedPositionRight = new Vector2(150, 0);
    public Vector2 fixedPositionLeft = new Vector2(-150, 0);
    public Vector2 fixedPositionCenter = new Vector2(0, 0);

    public string rightSceneName;
    public string leftSceneName;

    private Vector2 startTouchPosition, endTouchPosition;
    private RectTransform imageRectTransform;

    // Estado para confirmación de swipe
    private bool awaitingConfirmation = false;
    private bool isSwipeRight = false;

    void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endTouchPosition = touch.position;
                swipeVector = endTouchPosition - startTouchPosition;

                if (Mathf.Abs(swipeVector.x) >= swipeThreshold)
                {
                    if (swipeVector.x > swipeThreshold)
                    {
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionRight, swipeSpeed * Time.deltaTime);
                        isSwipeRight = true;
                    }
                    else if (swipeVector.x < -swipeThreshold)
                    {
                        imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionLeft, swipeSpeed * Time.deltaTime);
                        isSwipeRight = false;
                    }
                }
                else
                {
                    imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (!awaitingConfirmation)
                {
                    imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
                    awaitingConfirmation=true;
                }else
                {
                    ConfirmSwipe(swipeVector);
                }
            }
        }
        else if (Input.GetMouseButton(0)) // Alternativa para pruebas en el editor usando el mouse
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                endTouchPosition = Input.mousePosition;
                Vector2 swipeVector = endTouchPosition - startTouchPosition;

                if (Mathf.Abs(swipeVector.x) >= swipeThreshold)
                {
                    if (!awaitingConfirmation)
                    {
                        if (swipeVector.x > swipeThreshold)
                        {
                            imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionRight, swipeSpeed * Time.deltaTime);
                            awaitingConfirmation = true;
                            isSwipeRight = true;
                        }
                        else if (swipeVector.x < -swipeThreshold)
                        {
                            imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionLeft, swipeSpeed * Time.deltaTime);
                            awaitingConfirmation = true;
                            isSwipeRight = false;
                        }
                    }
                    else
                    {
                        ConfirmSwipe(swipeVector);
                    }
                }
                else
                {
                    imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!awaitingConfirmation)
                {
                    imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void ConfirmSwipe(Vector2 swipeVector)
    {
        if ((isSwipeRight && swipeVector.x > swipeThreshold) || (!isSwipeRight && swipeVector.x < -swipeThreshold))
        {
            if (isSwipeRight)
            {
                Debug.Log("Confirmado cambio a: der " + rightSceneName);
                //SceneManager.LoadScene(rightSceneName);
            }
            else
            {
                Debug.Log("Confirmado cambio a: izq" + leftSceneName);
                //SceneManager.LoadScene(leftSceneName);
            }
        }
        else
        {
            imageRectTransform.anchoredPosition = Vector2.Lerp(imageRectTransform.anchoredPosition, fixedPositionCenter, swipeSpeed * Time.deltaTime);
        }

        awaitingConfirmation = false; // Resetear el estado para el próximo swipe
    }
}
