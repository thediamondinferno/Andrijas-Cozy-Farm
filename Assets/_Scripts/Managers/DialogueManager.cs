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

    private Dialogue _currentDialogue;
    private int _currentLineIndex;
    private bool _isShowingText;
    private bool _skipRequest;

    bool _dialogueInProgress;

    
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        _dialogueInProgress = true;
        _currentDialogue = dialogue;
        _currentLineIndex = 0;
        _speakerImage.sprite = dialogue.speakerSprite;
        CinematicOn();
    }

    private IEnumerator TypeSentence(string sentence) {
        AudioClip nextAudioClip =  _audioClips[Random.Range(0,_audioClips.Length)];
        _audioSource.PlayOneShot(nextAudioClip);
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

    void CinematicOn(){
        LeanTween.moveLocalY(_cinematicBot.gameObject, _cinematicBot.localPosition.y + 100, 1f).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveLocalY(_cinematicTop.gameObject, _cinematicTop.localPosition.y - 100, 1f).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveLocalY(_dialoguePanel.gameObject, _dialoguePanel.transform.localPosition.y + 400, 1f).setEase(LeanTweenType.easeInQuad);
    }

    void CinematicOff(){
        LeanTween.moveLocalY(_cinematicBot.gameObject, _cinematicBot.localPosition.y - 100, 1f).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveLocalY(_cinematicTop.gameObject, _cinematicTop.localPosition.y + 100, 1f).setEase(LeanTweenType.easeInQuad);
        LeanTween.moveLocalY(_dialoguePanel.gameObject, _dialoguePanel.transform.localPosition.y - 400, 1f).setEase(LeanTweenType.easeInQuad);
        ShopManager.Instance.OpenShop();
        _dialogueInProgress = false;
    }


    bool _justPressed;
    void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_justPressed && _dialogueInProgress) {
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
