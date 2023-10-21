using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputSystem inputSystem;
    Vector3 movementVector;
    private void Awake()
    {
        //GridManager grid = new GridManager();
        inputSystem = new InputSystem();
        inputSystem.PlayerActionMap.Movement.started += Movement;
    }
    private void Movement(InputAction.CallbackContext context)
    {
        movementVector.x = context.ReadValue<Vector2>().x;
        movementVector.z = context.ReadValue<Vector2>().y;
        movementVector.y = 0f;
        MovementAnimations();
    }
    private void MovementAnimations()
    {
        transform.right = movementVector;
        //transform.DOJump(transform.position + movementVector * 2, 3, 1, 0.1f);
    }
    private void OnEnable()
    {
        inputSystem.PlayerActionMap.Enable();
    }
    private void OnDisable()
    {
        inputSystem.PlayerActionMap.Disable();
    }
}
