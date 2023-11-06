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
    public int _outfitType;
    public GameObject[] outfits;

    void Update(){
        _animators[0].SetFloat("vertical", _moveInput.y);
        _animators[0].SetFloat("horizontal", _moveInput.x);
        if(_outfitType != 0 && _animators[_outfitType] != null){
            for (int i = 0; i < outfits.Length; i++)
            {
                outfits[i].SetActive(false);
            }
            outfits[_outfitType-1].SetActive(true);
            _animators[_outfitType].SetFloat("vertical", _moveInput.y);
            _animators[_outfitType].SetFloat("horizontal", _moveInput.x);
        }
    }

    private void FixedUpdate()
    {
        // Use FixedUpdate for physics-based movement
        Vector2 currentPos = _playerRigidBody.position;
        Vector2 inputVector = _moveInput * _moveSpeed * Time.fixedDeltaTime;
        Vector2 newPos = currentPos + inputVector;
        _playerRigidBody.MovePosition(newPos);
    }
}
