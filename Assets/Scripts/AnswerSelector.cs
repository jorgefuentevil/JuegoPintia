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
    private bool direccion;
    public float variacionAtr;

    private Animator anim;
    

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
        if ((currentPage == 2 && direccion) || (currentPage ==4 && !direccion)){
            transform.position = panelLocation - new Vector3(difference, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData data){
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        GameObject canvaImage = GameObject.FindGameObjectWithTag("CanvaImagen"); 
        anim = canvaImage.GetComponent<Animator>();
        if(Mathf.Abs(percentage) >= percentThreshold){
            Vector3 newLocation = panelLocation;
            if(percentage > 0 && currentPage < totalPages){
                currentPage++;
                if(currentPage ==4){
                    anim.Play("cardFlipIzq");
                    Debug.Log("Atributos a actualizar=on");
                }else if (currentPage ==3){
                    anim.Play("cardFlipVueltaDer");
                    Debug.Log("Atributos=off");
                }else if (currentPage ==5){
                    Debug.Log("cambiando a izquierda");
                }
            }else if(percentage < 0 && currentPage > 1){
                currentPage--;
                if(currentPage ==2){
                    anim.Play("cardFlipDer");
                    Debug.Log("Atributos a actualizar=on");
                }else if (currentPage ==3){
                    anim.Play("cardFlipVueltaIzq");
                    Debug.Log("Atributos=off");
                }else if(currentPage ==1){
                    Debug.Log("cambiando a derecha");
                }
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

    void cambiarAtributo(string atributo, float cantidad){
        GameObject[] attImage;
        attImage = GameObject.FindGameObjectsWithTag(atributo);
        attImage[0].GetComponent<Image>().fillAmount += cantidad*variacionAtr;
        return;
    }
}