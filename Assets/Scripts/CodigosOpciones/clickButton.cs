using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickButton : MonoBehaviour
{

    public Slider sfx;
    public AudioClip click;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio=gameObject.AddComponent<AudioSource>();
        audio.playOnAwake=false;
        audio.clip=click;

        if(sfx!=null){
            audio.volume=sfx.value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClick(){
        if(audio!=null && click!=null){
            audio.volume=sfx.value;
            audio.Play();
        }
    }
}
