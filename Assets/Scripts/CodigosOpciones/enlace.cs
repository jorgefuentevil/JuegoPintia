using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enlace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enlaces(string url){
        Application.OpenURL(url);
    }
}
