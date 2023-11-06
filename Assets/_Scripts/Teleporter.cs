using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _teleportEnd;
    [SerializeField] Image _sceneTransition;
    [SerializeField] RectTransform _interactableButton;
    [SerializeField] float _buttonAnimationSpeed; 
    GameObject _player;

    Coroutine _interactableButtonCo;

    [SerializeField] AudioSource _audioSource; 

    public void Interact(){
        StartCoroutine(FadeTransition());
    }
    

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            _interactableButtonCo = StartCoroutine(ChangeUIVisibility(true));
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("Player") && Keyboard.current.eKey.wasPressedThisFrame){
            _player = other.gameObject;
            Interact();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        StopCoroutine(_interactableButtonCo);
        if (other.gameObject.CompareTag("Player")) {
            StartCoroutine(ChangeUIVisibility(false));
        }
    }

    IEnumerator FadeTransition(){
        _audioSource.Play();
        int counter = 0;
        while(counter < 100){
            counter++;
            _sceneTransition.color += new Color(0, 0, 0, .01f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _player.transform.position = _teleportEnd.transform.position;
        while(counter > 0){
            counter--;
            _sceneTransition.color -= new Color(0, 0, 0, .01f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator ChangeUIVisibility(bool show) {
        Image temp = _interactableButton.GetComponent<Image>();
        if(show){
            while(_interactableButton.localPosition.y < .15f){
                _interactableButton.localPosition += new Vector3(0, .0015f, 0);
                temp.color += new Color(0, 0, 0, .01f);
                yield return new WaitForSeconds(Time.deltaTime * _buttonAnimationSpeed);
            }
        }
        else{
            while(_interactableButton.localPosition.y > 0){
                _interactableButton.localPosition -= new Vector3(0, .0015f, 0);
                temp.color -= new Color(0, 0, 0, .01f);
                yield return new WaitForSeconds(Time.deltaTime * _buttonAnimationSpeed);
            }
        }
    }
}
