using DG.Tweening;
using GridSystem.Square;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform eagleTransform;
    [SerializeField] private TextMeshProUGUI scoreText;
    bool eagleCheck = false;
    InputSystem inputSystem;
    Vector3 movementVector;
    GridManager gridManager;
    BiomeManager biomeManager;
    Vector3 localScale;
    Cell nextCell;

    int maxScore = 0;

    #region mobilemovement
    Vector2 touchOrigin;
    Vector2 endPoint;
    #endregion
    private void MobileMovementStarted(InputAction.CallbackContext context)
    {
        touchOrigin = context.ReadValue<Vector2>();
        Debug.Log("Works");
    }
    private void MobileMovementEnd(InputAction.CallbackContext context)
    {
        endPoint = context.ReadValue<Vector2>();
        movementVector = endPoint - touchOrigin;
        movementVector = movementVector.normalized;
        if (movementVector.x > 0.5f)
        {
            movementVector = Vector3.right;
        }
        else if (movementVector.x < -0.5f)
        {
            movementVector = Vector3.left;
        }
        else if (movementVector.y > 0.5f)
        {
            movementVector = Vector3.up;
        }
        else if (movementVector.y < -0.5f)
        {
            movementVector = Vector3.down;
        }
        else
        {
            movementVector = Vector3.zero;
        }
    }
    private void PcMovement(InputAction.CallbackContext context)
    {
        if (eagleCheck) return;
      
        movementVector.x = context.ReadValue<Vector2>().x;
        movementVector.z = context.ReadValue<Vector2>().y;
        movementVector.y = 0f;

        GridVector nextGridPos = gridManager.TileGrid.GetGridPosition(transform.position + movementVector);
        if (nextGridPos.Column < -5 || nextGridPos.Column > 5) return;
        if (gridManager.TileGrid.TryGetCell(nextGridPos, out Cell cell))
        {
            if (gridManager.TileGrid.TryGetValue(cell, out Tile tile))
            {
                if (tile is Water)
                {
                    if (!gridManager.ObstacleGrid.TryGetValue(nextGridPos, out _))
                    {
                        MovementAnimations(tile.transform.position, Kill);
                    }
                }
                else if (gridManager.ObstacleGrid.TryGetValue(nextGridPos, out Obstacle obstacle))
                {
                    return;
                }
                MovementAnimations(tile.transform.position, EagleKillCheck);


            }
            nextCell = cell;
        }
    }
    private void Awake()
    {
        localScale = transform.localScale;
        inputSystem = new InputSystem();

#if UNITY_ANDROID
        inputSystem.PlayerActionMap.Movement.started += MobileMovementStart;
        inputSystem.PlayerActionMap.Movement.canceled += MobileMovementEnd;
        inputSystem.PlayerActionMap.Movement.started += UpdateBiome;
#elif UNITY_STANDALONE

        inputSystem.PlayerActionMap.Movement.started += PcMovement;
        inputSystem.PlayerActionMap.Movement.started += UpdateBiome;
#endif
    }
    private void UpdateBiome(InputAction.CallbackContext context)
    {
        biomeManager.UpdateBiomes(nextCell.GridPosition.Row);
    }
    
    private void UpdateScore(int score)
    {
        maxScore = score;
        scoreText.text = maxScore.ToString();
    }
    private void EagleKillCheck()
    {
        if (maxScore < nextCell.GridPosition.Row)
        {
            UpdateScore(nextCell.GridPosition.Row);
        }

        else if (maxScore > nextCell.GridPosition.Row + 3)
        {
            gameObject.GetComponent<Collider>().enabled = false;
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
                    OnComplete(() =>
                    {
                        eagleTransform.localPosition = new Vector3(0, 20, -90);
                        eagleTransform.gameObject.SetActive(false);
                        Kill();
                    }));
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
        transform.position = gridManager.TileGrid.GetWorldPosition(GridVector.Zero);
        this.gridManager = gridManager;
        this.biomeManager = biomeManager;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Car") || collider.CompareTag("Train")) Kill();
    }

    private async void Kill()
    {

        gameObject.SetActive(false);
        await biomeManager.Restart();
        CameraController.isFollowing = true;
        gameObject.GetComponent<Collider>().enabled = true;
        eagleCheck = false;
        UpdateScore(0);
        gameObject.SetActive(true);
        transform.position = gridManager.TileGrid.GetWorldPosition(GridVector.Zero);
        gridManager.TileGrid.TryGetCell(GridVector.Zero, out nextCell);

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
