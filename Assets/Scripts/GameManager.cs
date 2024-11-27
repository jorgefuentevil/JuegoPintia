using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public AsyncOperation EstadoLoadScene; //TODO
    public string currentLevel {get; private set;} = null;

    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }


    public void CambiaEscenaMainMenu(){
        SceneManager.LoadScene("MainMenuScene");
    }

    public void CambiaEscenaGamePrincipal(string nivel){
        currentLevel = nivel;
        SceneManager.LoadScene("GamePrincipalScene");
    }

    public void CambiaEscenaTutorial(){
        currentLevel = "Tutorial";
        SceneManager.LoadScene("GamePrincipalScene");
    }

}
