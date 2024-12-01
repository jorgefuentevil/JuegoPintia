using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CandyCoded.HapticFeedback;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler{
    private Vector3 panelLocation;
    private float percentThreshold = 0.2f;
    private float easing = 0.5f;
    public int totalPages = 1;
    public int currentPage = 1;
    public LevelManager levelManager;

    AudioManager audioManager;

    private void Awake(){
        // Encuentra el GameObject con el tag "AudioManager" y obtiene el componente AudioManager
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

    }

    // Start is called before the first frame update
    void Start(){
        panelLocation = transform.position;
    }
    public void OnDrag(PointerEventData data){
        float difference = data.pressPosition.x - data.position.x;
        transform.position = panelLocation - new Vector3(difference, 0, 0);
    }
    public void OnEndDrag(PointerEventData data){
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentThreshold){
            Vector3 newLocation = panelLocation;
            if(percentage > 0 && currentPage < totalPages){
                currentPage++;
                Vibracion();
                audioManager.PlaySlideSFX();
                newLocation += new Vector3(-Screen.width, 0, 0);
            }else if(percentage < 0 && currentPage > 1){
                currentPage--;
                Vibracion();
                audioManager.PlaySlideSFX();
                newLocation += new Vector3(Screen.width, 0, 0); 
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
            levelManager.SetLevelData(currentPage-1);
        }else{
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
            levelManager.SetLevelData(currentPage-1);
        }
    }


    public void BindFlechaDerecha()
    {
        OnEndDrag(new (null){
            pressPosition = Vector3.right * Screen.width
        });
    }

    public void BindFlechaIzquierda()
    {
        OnEndDrag(new (null){
            pressPosition = Vector3.left * Screen.width
        });
    }


    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds){
        float t = 0f;
        while(t <= 1.0){
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    public void Vibracion()
    {
        if(PlayerPrefs.GetInt("VibracionEnabled")==1){
            HapticFeedback.HeavyFeedback();
            Debug.Log("vibro cambiando el lvl");
        }
    }
}