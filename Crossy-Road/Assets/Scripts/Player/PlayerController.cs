using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public virtual void Movement(InputAction.CallbackContext callbackContext)
    {
        transform.position += new Vector3(callbackContext.ReadValue<Vector2>().y * 5f, 0, -callbackContext.ReadValue<Vector2>().x * 5f);
    }
    public virtual void Animation()
    {
        
    }
}
