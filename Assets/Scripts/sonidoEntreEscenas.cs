using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sonidoEntreEscenas : MonoBehaviour
{
    
    private sonidoEntreEscenas instance;
    public sonidoEntreEscenas Instance{
        get{
            return instance;
        }
    }
    private void Awake(){
        
        var noDestruirEntreEscenas=FindObjectsOfType<sonidoEntreEscenas>(); //elimina la escena actual si se encuentra duplicada
        if(noDestruirEntreEscenas.Length > 1){
            Destroy(gameObject);
        }
        if(instance!=null && instance!=this){
            Destroy(gameObject);
            return;
        }else{
            instance=this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
