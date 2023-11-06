using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {get; private set;}

    [SerializeField] float _moveSpeed = 5f;
    Vector2 _moveInput;
    [SerializeField] Rigidbody2D _playerRigidBody;
    PlayerControls _playerControls;

    [SerializeField] Animator[] _animators;
    [SerializeField] AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        _playerControls = new PlayerControls();
        _playerControls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _playerControls.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        _playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Player.Disable();
    }
    public int outfitType;
    public GameObject[] outfits;

    public void ActivateOutfit(int _outfitType){
        outfitType = _outfitType;
        foreach(var outfit in outfits){
            outfit.SetActive(false);
        }
        outfits[outfitType-1].SetActive(true);
    }

    void Update(){
        _animators[0].SetFloat("vertical", _moveInput.y);
        _animators[0].SetFloat("horizontal", _moveInput.x);
        if(outfitType != 0 && _animators[outfitType] != null){
            _animators[outfitType].SetFloat("vertical", _moveInput.y);
            _animators[outfitType].SetFloat("horizontal", _moveInput.x);
        }
    }

    private void FixedUpdate()
    {
        // Use FixedUpdate for physics-based movement
        Vector2 currentPos = _playerRigidBody.position;
        Vector2 inputVector = _moveInput * _moveSpeed * Time.fixedDeltaTime;
        Vector2 newPos = currentPos + inputVector;
        _playerRigidBody.MovePosition(newPos);
        if(!_audioSource.isPlaying && _moveInput != Vector2.zero) _audioSource.Play();
        if(_moveInput == Vector2.zero) _audioSource.Stop();
    }
}
