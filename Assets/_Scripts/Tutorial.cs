using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour, IInteractable
{
    [SerializeField] RectTransform _interactableButton;
    [SerializeField] float _buttonAnimationSpeed;
    [SerializeField] Dialogue _dialogue;

    
    Coroutine _interactableButtonCo;

    public void Interact() {
        if (DialogueManager.Instance != null) {
            DialogueManager.Instance.StartDialogue(_dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            _interactableButtonCo = StartCoroutine(ChangeUIVisibility(true));
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.CompareTag("Player") && Keyboard.current.eKey.wasPressedThisFrame){
            Interact();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        StopCoroutine(_interactableButtonCo);
        if (other.gameObject.CompareTag("Player")) {
            StartCoroutine(ChangeUIVisibility(false));
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
