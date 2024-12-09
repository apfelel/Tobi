using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInputs _input;
    private InputAction _move, _interact;
    private FishingRod _fishingRod;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _input = new();
        _interact = _input.Player.Interact;
        _move = _input.Player.Move;
    }
    private void OnEnable()
    {
        _input.Enable();
        
        _interact.performed += Interact;
        _interact.Enable();

        _move.performed += DirectionInput;
        _move.Enable();

    }
    private void OnDisable()
    {
        _input.Disable();
        _interact.performed -= Interact;
        _interact.Disable();

        _move.performed -= DirectionInput;
        _move.Disable();
    }

    private void Start()
    {
        _fishingRod = GetComponent<FishingRod>();
    }

    private void DirectionInput(InputAction.CallbackContext obj)
    {
        Debug.Log(obj.ReadValue<Vector2>().ToString());
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        _fishingRod.Interact();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
