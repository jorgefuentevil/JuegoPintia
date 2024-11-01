using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour{
    public GameObject levelHolder;
    public GameObject levelIcon;
    public GameObject thisCanvas;
    public int numberOfLevels = 6;
    private Rect panelDimensions;
    
    private Sprite[] images;
    void Start(){
        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        images = Resources.LoadAll<Sprite>("Assets/Images/Personajes");

        for (int i = 0; i < numberOfLevels; i++){
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(thisCanvas.transform,false);
            panel.transform.SetParent(levelHolder.transform);
            panel.name = "Page-"+(i+1);
            panel.GetComponent<RectTransform>().localPosition = new Vector2(panelDimensions.width * (i),0);
                
        }
        Destroy(panelClone);
    }


    private void LoadImages(GameObject panel, int i){
        GameObject icon = Instantiate(levelIcon) as GameObject;
        //icon.transform.SetParent(thisCanvas.transform,false);
        icon.transform.SetParent(panel.transform);
        icon.GetComponent<Image>().sprite = images[i];
        icon.name = "Nivel "+ (i+1);
    }
}


