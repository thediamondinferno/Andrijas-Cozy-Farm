using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _settingPanel;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;

    [SerializeField] Volume _globalVolume;
    [SerializeField] GameObject _postProcessingButton;
    [SerializeField] Sprite _postProcessingON;
    [SerializeField] Sprite _postProcessingOFF;

    [SerializeField] Image _sceneTransition;
    [SerializeField] GameObject _mainMenuHolder;

    void Awake(){
        DontDestroyOnLoad(gameObject);
    }

    void Start(){
        float musicVolume = AudioManager.Instance.GetMusicVolume();
        _musicSlider.value = musicVolume;

        float sfxVolume = AudioManager.Instance.GetSFXVolume();
        _sfxSlider.value = sfxVolume;

        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    bool _gameLoaded;
    public void StartGame(){
        if(!_gameLoaded){
            _gameLoaded = true;
            LeanTween.moveLocalY(_mainMenuHolder, _mainMenuHolder.transform.localPosition.y + 1000, .2f);
            StartCoroutine(FadeTransition());
        }
    }
    
    public void OpenSettings(){
        LeanTween.moveLocalY(_settingPanel, _settingPanel.transform.localPosition.y - 1000, 1);
    }

    public void CloseSettings(){
        LeanTween.moveLocalY(_settingPanel, _settingPanel.transform.localPosition.y + 1000, 1);
    }

    public void CloseGame(){
        Application.Quit();
    }

    bool _ppON = true;

    public void TogglePostProcessing(){
        _globalVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
        if(!_ppON) {
            _ppON = true;
            _globalVolume.weight = 1;
            _postProcessingButton.GetComponent<Image>().sprite = _postProcessingON;
        }
        else if(_ppON){
            _ppON = false;
            _globalVolume.weight = 0;
            _postProcessingButton.GetComponent<Image>().sprite = _postProcessingOFF;
        }
    }

    bool _menuOpen;

    void Update(){
        if(Keyboard.current.escapeKey.wasPressedThisFrame && !_menuOpen && !_escapeClicked){
            _escapeClicked = true;
            _menuOpen = true;
            LeanTween.moveLocalY(_mainMenuHolder, _mainMenuHolder.transform.localPosition.y - 1000, .2f);
            StartCoroutine(EscapeCooldown());
        }
        else if(Keyboard.current.escapeKey.wasPressedThisFrame && _menuOpen && !_escapeClicked){
            _escapeClicked = true;
            _menuOpen = false;
            LeanTween.moveLocalY(_mainMenuHolder, _mainMenuHolder.transform.localPosition.y + 1000, .2f);
            StartCoroutine(EscapeCooldown());
        }
    }

    bool _escapeClicked;
    IEnumerator EscapeCooldown(){
        yield return new WaitForSeconds(.2f);
        _escapeClicked = false;
    }

    IEnumerator FadeTransition(){
        int counter = 0;
        while(counter < 100){
            counter++;
            _sceneTransition.color += new Color(0, 0, 0, .01f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        SceneManager.LoadSceneAsync(1);
        yield return new WaitForSeconds(1);
        //SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        while(counter > 0){
            counter--;
            _sceneTransition.color -= new Color(0, 0, 0, .01f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
