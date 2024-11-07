using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicaEscenas : MonoBehaviour
{

    public void Awake(){
        var notDestroy=FindObjectsOfType<logicaEscenas>();
        if(notDestroy.Length>1){
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
