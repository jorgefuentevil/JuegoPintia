using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitLoadPage : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(waiter());
    }
    IEnumerator waiter()
    {
        //Wait for 2 seconds
        yield return new WaitForSeconds(2);
        if (PlayerPrefs.HasKey("Tutorial"))
        {
            SceneManager.LoadScene("MainMenuScene");
        }
        else
        {
            SceneManager.LoadScene("GamePrincipalScene");
        }
        
    }
}
