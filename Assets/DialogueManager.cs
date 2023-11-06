using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Image _speakerImage;
    [SerializeField] RectTransform _cinematicTop;
    [SerializeField] RectTransform _cinematicBot;
    [SerializeField] float _cinematicSpeed;

    private Dialogue _currentDialogue;
    private int _currentLineIndex;
    private bool _isShowingText;
    private bool _skipRequest;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        _currentDialogue = dialogue;
        _currentLineIndex = 0;
        _speakerImage.sprite = dialogue.speakerSprite;
        CinematicOn();
    }

    private IEnumerator TypeSentence(string sentence) {
        _dialogueText.text = "";
        _isShowingText = true;

        foreach (char letter in sentence.ToCharArray()) {
            if (_skipRequest) {
                _dialogueText.text = sentence;
                _skipRequest = false;
                break;
            }
            _dialogueText.text += letter;
            yield return null;
        }

        _isShowingText = false;
    }

    public void DisplayNextSentence() {
        if (_isShowingText) {
            _skipRequest = true; // Skip to the end of the current typing sentence
            return;
        }

        if (_currentLineIndex < _currentDialogue.lines.Length - 1) {
            _currentLineIndex++;
            StartCoroutine(TypeSentence(_currentDialogue.lines[_currentLineIndex]));
        } else {
            EndDialogue();
        }
    }

    private void EndDialogue() {
        CinematicOff();
    }

    // IEnumerator CinematicOn(){
    //     _cinematicBot.gameObject.SetActive(true);
    //     _cinematicTop.gameObject.SetActive(true);
    //     _dialoguePanel.SetActive(true);
    //     while(_cinematicBot.transform.localPosition.y < 0){
    //         _cinematicBot.transform.localPosition += new Vector3(0, 1f, 0);
    //         _cinematicTop.transform.localPosition -= new Vector3(0, 1f, 0);
    //         yield return new WaitForSeconds(Time.deltaTime * _cinematicSpeed);
    //     }
    //     while(_dialoguePanel.transform.localPosition.y < 100){
    //         _dialoguePanel.transform.localPosition += new Vector3(0, 1f, 0);
    //         yield return new WaitForSeconds(Time.deltaTime * _cinematicSpeed);
    //     }
    //     StartCoroutine(TypeSentence(_currentDialogue.lines[_currentLineIndex]));
    // }
    void CinematicOn(){
        LeanTween.moveLocalY(_cinematicBot.gameObject, _cinematicBot.localPosition.y + 100, 1f).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveLocalY(_cinematicTop.gameObject, _cinematicTop.localPosition.y - 100, 1f).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveLocalY(_dialoguePanel.gameObject, _dialoguePanel.transform.localPosition.y + 400, 1f).setEase(LeanTweenType.easeInQuad);
    }

    void CinematicOff(){
        LeanTween.moveLocalY(_cinematicBot.gameObject, _cinematicBot.localPosition.y - 100, 1f).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveLocalY(_cinematicTop.gameObject, _cinematicTop.localPosition.y + 100, 1f).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveLocalY(_dialoguePanel.gameObject, _dialoguePanel.transform.localPosition.y - 400, 1f).setEase(LeanTweenType.easeInQuad);
    }

    // IEnumerator CinematicOff(){
    //     while(_dialoguePanel.transform.localPosition.y > -300){
    //         _dialoguePanel.transform.localPosition -= new Vector3(0, 1f, 0);
    //         yield return new WaitForSeconds(Time.deltaTime * _cinematicSpeed);
    //     }
    //     while(_cinematicBot.transform.localPosition.y > -100){
    //         _cinematicBot.transform.localPosition -= new Vector3(0, 1f, 0);
    //         _cinematicTop.transform.localPosition += new Vector3(0, 1f, 0);
    //         yield return new WaitForSeconds(Time.deltaTime * _cinematicSpeed);
    //     }
    //     _cinematicBot.gameObject.SetActive(false);
    //     _cinematicTop.gameObject.SetActive(false);
    //     _dialoguePanel.SetActive(false);
    // }

    bool _justPressed;
    void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_justPressed) {
            _justPressed = true;
            DisplayNextSentence();
            StartCoroutine(justPressedCooldown());
        }
    }

    IEnumerator justPressedCooldown(){
        yield return new WaitForSeconds(.1f);
        _justPressed = false;
    }
}
