using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;


    public void Awake(){
        if(PlayerPrefs.HasKey("musicVolume")){
            LoadVolume();
        }else{
            SetMusicVolume();
            SetSFXVolume();
        }
    }


    public void SetMusicVolume(){
        float volume = musicSlider.value;
        audioMixer.SetFloat("music",volume);
        PlayerPrefs.SetFloat("musicVolume",volume);
    }

    public void SetSFXVolume(){
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX",volume);
        PlayerPrefs.SetFloat("sfxVolume",volume);
    }

    private void LoadVolume(){
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }
}
