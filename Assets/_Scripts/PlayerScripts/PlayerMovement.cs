using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    Vector2 _moveInput;
    [SerializeField] Rigidbody2D _playerRigidBody;
    Controls _playerControls;

    private void Awake()
    {
        _playerControls = new Controls();
        _playerControls.PlayerControls.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _playerControls.PlayerControls.Move.canceled += ctx => _moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        _playerControls.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.PlayerControls.Disable();
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
