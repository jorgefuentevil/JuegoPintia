using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour{
    public GameObject levelHolder;
    public GameObject levelIcon;
    public GameObject thisCanvas;
    public int numberOfLevels = 8;
    private Rect panelDimensions;
    
    private Sprite[] images;
    void Start(){
        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        PageSwiper swiper = levelHolder.AddComponent<PageSwiper>();
        swiper.totalPages = numberOfLevels;



        panelDimensions = levelHolder.GetComponent<RectTransform>().rect;


        Debug.Log(images.Length);

        for (int i = 0; i < numberOfLevels; i++){
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(thisCanvas.transform,false);
            panel.transform.SetParent(levelHolder.transform);
            panel.name = "Page-"+(i+1);
            panel.GetComponent<RectTransform>().localPosition = new Vector2(panelDimensions.width * (i),0);
            LoadImages(panel,i);
        }
        Destroy(panelClone);
        Destroy(levelHolder.transform.GetChild(0).gameObject);
    }


    private void LoadImages(GameObject parentPanel, int i){
        GameObject icon = Instantiate(levelIcon) as GameObject;
        //icon.transform.SetParent(thisCanvas.transform,false);
        icon.transform.SetParent(parentPanel.transform);
        //icon.GetComponent<Image>().sprite = images[i];
        icon.name = "Nivel "+ (i+1);
    }
}


