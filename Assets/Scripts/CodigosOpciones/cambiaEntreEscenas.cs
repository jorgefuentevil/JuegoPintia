using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cambiaEntreEscenas : MonoBehaviour
{
    
    
    private void Awake(){
        
        var noDestruirEntreEscenas=FindObjectsOfType<cambiaEntreEscenas>(); //elimina la escena actual si se encuentra duplicada
        if(noDestruirEntreEscenas.Length > 1){
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
