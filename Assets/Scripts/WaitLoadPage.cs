using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitLoadPage : MonoBehaviour
{
    public CambiarEscena sceneController;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waiter());
    }
    IEnumerator waiter()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(2);
        sceneController.cambiarEscena("MainMenuScene");
    }
}
