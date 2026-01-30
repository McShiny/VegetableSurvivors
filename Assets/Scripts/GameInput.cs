using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    public static GameInput Instance {  get; private set; }

    public event EventHandler OnFiredProjectile;
    
    private PlayerInputActions playerInputActions;

    private void Awake() {
        Instance = this;
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Fire.performed += Fire_performed;

    }

    private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnFiredProjectile?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;

    }

}
