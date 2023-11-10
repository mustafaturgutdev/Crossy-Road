using DG.Tweening;
using GridSystem.Square;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform eagleTransform;
    bool eagleCheck = false;
    InputSystem inputSystem;
    Vector3 movementVector;
    GridManager gridManager;
    BiomeManager biomeManager;
    Vector3 localScale;
    Cell nextCell;

    int maxScore = 0;

    private void Awake()
    {
        localScale = transform.localScale;
        inputSystem = new InputSystem();
        inputSystem.PlayerActionMap.Movement.started += Movement;
        inputSystem.PlayerActionMap.Movement.started += UpdateBiome;
    }
    private void UpdateBiome(InputAction.CallbackContext context)
    {
        biomeManager.UpdateBiomes(nextCell.GridPosition.Row);
    }
    private void Movement(InputAction.CallbackContext context)
    {
        if (eagleCheck) return;

        movementVector.x = context.ReadValue<Vector2>().x;
        movementVector.z = context.ReadValue<Vector2>().y;
        movementVector.y = 0f;

        GridVector nextGridPos = gridManager.TileGrid.GetGridPosition(transform.position + movementVector);
        if (gridManager.TileGrid.TryGetCell(nextGridPos, out Cell cell))
        {
            if (gridManager.TileGrid.TryGetValue(cell, out Tile tile))
            {
                if (tile is Water)
                {
                    if (gridManager.ObstacleGrid.TryGetValue(nextGridPos, out Obstacle obstacle))
                    {
                        if (obstacle is Wood wood)
                        {
                            foreach (var pair in wood.Slots)
                            {
                                if (pair.Value.GridPosition == cell.GridPosition)
                                {
                                    MovementAnimations(pair.Key.position, EagleKillCheck);
                                }
                            }
                        }

                    }
                    else
                    {
                        MovementAnimations(tile.transform.position, EagleKillCheck);
                    }

                }
                else if (gridManager.ObstacleGrid.TryGetValue(nextGridPos, out Obstacle obstacle))
                {
                    return;
                }
                MovementAnimations(tile.transform.position, EagleKillCheck);
                //if (gridManager.ObstacleGrid.TryGetValue)


            }
            nextCell = cell;
        }


        //eðer nextCell water biom ise burada water biomdan bir fonksiyon çaðýracaksýn bu fonksiyon kontrol edecek currentCell available mý deðil mi
        //ya da water biom mu bakmak yerine bir sonraki tile water tipi mi onu kontrol edebilir o çok bir þey fark etmez
        //if (gridManager.TileGrid.TryGetValue(nextCell.GridPosition, out Tile tile) && tile.TileType == TileType.Water)
        //{
        //    if (gridManager.ObstacleGrid.TryGetValue(nextCell.GridPosition, out Obstacle obstacle))
        //    {
        //        Wood wood = obstacle as Wood;
        //    }
        //}
        //if (gridManager.IsObstacle(transform.position, movementVector) || !gridManager.HasTile(transform.position, movementVector, out Tile tile1))
        //{
        //    return;
        //}

    }

    private void EagleKillCheck()
    {
        if (maxScore < nextCell.GridPosition.Row)
            maxScore = nextCell.GridPosition.Row;
        else if (maxScore > nextCell.GridPosition.Row + 3)
        {
            CameraController.isFollowing = false;
            eagleCheck = true;
            eagleTransform.gameObject.SetActive(true);
            Vector3 eaglePosition = eagleTransform.position;
            Sequence sequence = DOTween.Sequence();
            sequence.
                Append(eagleTransform.DOMove(transform.position + Vector3.up, 2f).SetEase(Ease.InQuad)).
                Append(transform.
                    DOMove(transform.position + new Vector3(eaglePosition.x, eaglePosition.y, -eaglePosition.z), 2f).
                    SetEase(Ease.InQuad).
                    OnComplete(() => gameObject.SetActive(false)));
        }

    }
    private void MovementAnimations(Vector3 movePosition, Action onComplete = null)
    {




        transform.forward = movementVector;
        transform.DOScale(localScale * 0.9f, 0.1f).OnComplete(() =>
        {
            transform.DOScale(localScale, 0.1f);
        });
        transform.DOJump(movePosition, 1, 1, 0.1f).OnComplete(() => onComplete?.Invoke());
    }
    public void Initialize(GridManager gridManager, BiomeManager biomeManager)
    {
        transform.position = gridManager.TileGrid.GetWorldPosition(new GridVector(0, 0));
        this.gridManager = gridManager;
        this.biomeManager = biomeManager;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Car") || collider.CompareTag("Train")) gameObject.SetActive(false);
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
