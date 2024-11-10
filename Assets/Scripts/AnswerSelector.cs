using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerSelector : MonoBehaviour, IDragHandler, IEndDragHandler{
    private Vector3 panelLocation;
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;
    private int totalPages = 5;
    private int currentPage = 3;
    private Image image;
 public GameObject RespuestaIzq;
    public GameObject RespuestaDer;
    public bool direccion;

    

    // Start is called before the first frame update
    void Start(){
        panelLocation = transform.position;
        image = GetComponent<Image>();
    }

    public void OnDrag(PointerEventData data){
        float difference = data.pressPosition.x - data.position.x;
        if(difference < 0){
            direccion = true;
        }else{
            direccion = false;
        }
        transform.position = panelLocation - new Vector3(difference, 0, 0);
        switch(currentPage)
        {
            case 2:
                if(!direccion && image.transform.position.x>621){
                    transform.rotation = Quaternion.Euler(new Vector3(0, difference*0.76f, 0));
                }
                Debug.Log(image.transform.position.x);
                //if(!direccion && image.transform.position.x>518){
                break;
            case 3:
                if(image.transform.position.x<847 && image.transform.position.x>380){
                    transform.rotation = Quaternion.Euler(new Vector3(0, difference*0.76f, 0));
                }
                /**
                if(direccion && image.transform.position.x>660){
                    //Cambiar sprite a RespuestaDerecha
                    pass;
                }
                if(!direccion && image.transform.position.x<496,75){
                    //Cambiar sprite a Respuesta izq
                    pass;
                }**/
                break;
            case 4:
                if(direccion && image.transform.position.x<621){
                    transform.rotation = Quaternion.Euler(new Vector3(0, difference*0.76f, 0));
                }
                //if(direccion && image.transform.position.x<721)
                
                Debug.Log(image.transform.position.x);
                break;
        }
    }
    public void OnEndDrag(PointerEventData data){
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentThreshold){
            Vector3 newLocation = panelLocation;
            if(percentage > 0 && currentPage < totalPages){
                currentPage++;
                newLocation += new Vector3(-Screen.width/5, 0, 0);

            }else if(percentage < 0 && currentPage > 1){
                currentPage--;
                newLocation += new Vector3(Screen.width/5, 0, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
            if(currentPage ==1){
                    //Cambio escena
                    Debug.Log("cambiando a derecha");
            }else if(currentPage ==2){
                image.color = new Color(0.2f,0.2f,0.2f,1f);
                RespuestaDer.SetActive(true);
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }else if (currentPage ==3){
                image.color = new Color(1f,1f,1f,1f);
                RespuestaIzq.SetActive(false);
                RespuestaDer.SetActive(false);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }else if(currentPage ==4){
                    Debug.Log("cambiando color");
                    image.color = new Color(0.2f,0.2f,0.2f,1f);
                    RespuestaIzq.SetActive(true);
                    transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
            }else if(currentPage ==5){
                    //Cambio escena
                    Debug.Log("cambiando a izquierda");
            }
        }else{
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds){
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
//TODO no sirve de nada
    float GetRotationYInRange(Image image)
    {
        float rotationY = image.transform.localRotation.eulerAngles.y;
        return rotationY > 180 ? rotationY - 360 : rotationY;
    }
}