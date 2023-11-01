using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishController : PlayerController
{
    private InputSystem inputs;
    float angle;
    private void Awake()
    {
        inputs = new InputSystem();
        inputs.PlayerAction.Movement.started += Movement;
    }
    public override void Movement(InputAction.CallbackContext callbackContext)
    {
        base.Movement(callbackContext);
    }
    private void Update()
    {
        Animation();
    }
    public override void Animation()
    {
        angle = Mathf.Clamp(angle,-120,-60);
        Quaternion targetRotation = Quaternion.Euler(angle, 0, 0);
        transform.rotation = targetRotation;
    }
    private void OnEnable()
    {
        inputs.Enable();
    }
    private void OnDisable()
    {
        inputs.Disable();
    }
}
