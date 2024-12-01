using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public AsyncOperation EstadoLoadScene; //TODO
    public string currentLevel {get; private set;} = "";

    private string SaveFilePath;
    private GameSaveData savedData;

    public void Awake()
    {   
        SaveFilePath = Application.persistentDataPath + "/SaveFile.json";
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {   
            
            Instance = this;
            DontDestroyOnLoad(this);
            LoadGameData();
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


    public void SaveGameData()
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(savedData, Formatting.Indented); 
            File.WriteAllText(SaveFilePath,jsonData);
            Debug.Log($"Guardando progreso en: {SaveFilePath}");

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error guardando fichero de guardado: {e.Message}");
        }
    }

    public void LoadGameData()
    {
        try
        {
            if (File.Exists(SaveFilePath))
            {
                string jsonData = File.ReadAllText(SaveFilePath);
                savedData = JsonConvert.DeserializeObject<GameSaveData>(jsonData);

                Debug.Log($"Cargado fichero de guardado desde {SaveFilePath}");
            }
            else
            {
                savedData = CreateDefaultSaveData();
                Debug.Log($"Creado nuevo fichero de guardado: {SaveFilePath}");
                SaveGameData();
            }
        }catch(System.Exception e){
            Debug.LogError($"Error cargando fichero de guardado: {e.Message}");
            savedData = CreateDefaultSaveData();
        }
        
    }
    

    public bool CheckLevelStatus(int index)
    {
        if(index >= savedData.unlockedLevels.Count) AumentaSaveData(index);

        return savedData.unlockedLevels[index];
    }

    private GameSaveData CreateDefaultSaveData()
    {
        return new GameSaveData
        {
            unlockedLevels = new List<bool> {true}
        };
    }

    private void AumentaSaveData(int nuevaCapacidad)
    {
        while(savedData.unlockedLevels.Count <= nuevaCapacidad)
        {
            savedData.unlockedLevels.Add(false);
        }
    }

}


[System.Serializable]
public struct GameSaveData
{
    public List<bool> unlockedLevels;
}