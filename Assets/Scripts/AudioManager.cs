using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----- Audio Sources -----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("----- Audio Clips -----")]
    [SerializeField] private AudioClip musicaDeFondo;
    [SerializeField] public AudioClip swipeCard;
    
    public static AudioManager instance;



    //Persistencia entre escenas;
    public void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }




    private void Start(){
        musicSource.clip = musicaDeFondo;
        musicSource.Play();
    }


    public void PlaySFX(AudioClip clip){
        sfxSource.PlayOneShot(clip);
    }
}
