using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer audioMixer; 

    private const string MUSIC_VOLUME_PARAMETER = "MusicVolume";
    private const string SFX_VOLUME_PARAMETER = "SFXVolume";

    private void Awake(){
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else{
            Destroy(gameObject); 
        }
    }

    public void SetMusicVolume(float volume){
        audioMixer.SetFloat(MUSIC_VOLUME_PARAMETER, Mathf.Log10(volume) * 20);
    }

    public float GetMusicVolume(){
        float value;
        bool result = audioMixer.GetFloat(MUSIC_VOLUME_PARAMETER, out value);
        return result ? Mathf.Pow(10, value / 20) : 0f;
    }

    public void SetSFXVolume(float volume){
        audioMixer.SetFloat(SFX_VOLUME_PARAMETER, Mathf.Log10(volume) * 20); 
    }

    public float GetSFXVolume(){
        float value;
        bool result = audioMixer.GetFloat(SFX_VOLUME_PARAMETER, out value);
        return result ? Mathf.Pow(10, value / 20) : 0f; 
    }
}
