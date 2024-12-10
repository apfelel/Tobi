using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private CatchMiniGame _catchMiniGame;
    
    private PlayerInputs _input;
    private InputAction _move, _interact, _collection, _pause;
    private FishingRod _fishingRod;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _input = new();
        _interact = _input.Player.Interact;
        _move = _input.Player.Move;
        _collection = _input.Player.Collection;
        _pause = _input.Player.Pause;
    }
    private void OnEnable()
    {
        _input.Enable();
        
        _interact.performed += Interact;
        _interact.Enable();

        _move.performed += DirectionInput;
        _move.Enable();
        
        _collection.performed += ToggleCardCollection;
        _collection.Enable();
        
        _pause.performed += TogglePauseMenu;
        _pause.Enable();
    }


    private void OnDisable()
    {
        _input.Disable();
        
        _interact.performed -= Interact;
        _interact.Disable();

        _move.performed -= DirectionInput;
        _move.Disable();
        
        _collection.performed -= ToggleCardCollection;
        _collection.Disable();
        
        _pause.performed -= TogglePauseMenu;
        _pause.Disable();
    }

    private void Start()
    {
        _fishingRod = GetComponent<FishingRod>();
        _catchMiniGame = GameManager.Instance.catchMiniGame;
    }

    private void DirectionInput(InputAction.CallbackContext obj)
    {
        if (UIManager.Instance.inMenu) return;
        _catchMiniGame.TrySolve(obj.ReadValue<Vector2>());
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (UIManager.Instance.inMenu) return;
        _fishingRod.Interact();
    }
    private void ToggleCardCollection(InputAction.CallbackContext obj)
    {
        UIManager.Instance.ToggleCardCollection();
    }
    private void TogglePauseMenu(InputAction.CallbackContext obj)
    {
        UIManager.Instance.TogglePauseMenu();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
