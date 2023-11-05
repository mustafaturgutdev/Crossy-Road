using DG.Tweening;
using GridSystem.Square;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputSystem inputSystem;
    Vector3 movementVector;
    GridManager gridManager;
    BiomeManager biomeManager;
    Vector3 localScale;
    Cell currentCell;
    private void Awake()
    {
        localScale = transform.localScale;
        inputSystem = new InputSystem();
        inputSystem.PlayerActionMap.Movement.started += Movement;
        inputSystem.PlayerActionMap.Movement.started += UpdateBiome;
    }
    private void UpdateBiome(InputAction.CallbackContext context)
    {
        biomeManager.UpdateBiomes(currentCell.GridPosition.Row);
    }
    private void Movement(InputAction.CallbackContext context)
    {
        movementVector.x = context.ReadValue<Vector2>().x;
        movementVector.z = context.ReadValue<Vector2>().y;
        movementVector.y = 0f;
        currentCell = gridManager.GetCell(transform.position + movementVector);
        if (gridManager.IsObstacle(transform.position, movementVector) || !gridManager.HasTile(transform.position, movementVector))
        {
            return;
        }
        MovementAnimations();
    }
    private void MovementAnimations()
    {
        transform.forward = movementVector;
        transform.DOScale(localScale * 0.9f ,0.1f).OnComplete(() =>
        {
            transform.DOScale(localScale, 0.1f);
        });
        //burasý düzenlenecek
        transform.DOJump(currentCell.GameObject.transform.position, 1, 1, 0.1f);
    }
    public void Initialize(GridManager gridManager, BiomeManager biomeManager)
    {
        transform.position = gridManager.TileGrid.GetWorldPosition(new GridVector(0, 0));
        this.gridManager = gridManager;
        this.biomeManager = biomeManager;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Car")) Destroy(gameObject);
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
