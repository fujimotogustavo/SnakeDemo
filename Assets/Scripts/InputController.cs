using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public event Action DirectionalPressed;
    [SerializeField] private SnakeController snakeController;

    public bool isHoldingKey;
    private float holdTime;
    private float holdThreshold = 0.5f;
    private float holdMinThreshold = 0.1f;
    private float accelerationFactor = 0.15f;
    private KeyCode currentKey;

    private void Update()
    {
        if (isHoldingKey)
        {
            holdTime += Time.deltaTime;
            if (holdTime >= holdThreshold)
            {
                snakeController.AutoMove();
                holdTime = 0f;

                if (holdThreshold > holdMinThreshold)
                {
                    holdThreshold -= accelerationFactor;
                    holdThreshold = Mathf.Max(holdThreshold, holdMinThreshold);
                }
            }
        }
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && snakeController.ongoingDirection != OngoingDirection.Down)
        {
            SetDirection(KeyCode.W, OngoingDirection.Up);
        }
        else if (Input.GetKeyDown(KeyCode.S) && snakeController.ongoingDirection != OngoingDirection.Up)
        {
            SetDirection(KeyCode.S, OngoingDirection.Down);
        }
        else if (Input.GetKeyDown(KeyCode.A) && snakeController.ongoingDirection != OngoingDirection.Right)
        {
            SetDirection(KeyCode.A, OngoingDirection.Left);
        }
        else if (Input.GetKeyDown(KeyCode.D) && snakeController.ongoingDirection != OngoingDirection.Left)
        {
            SetDirection(KeyCode.D, OngoingDirection.Right);
        }

        if (Input.GetKeyUp(currentKey))
        {
            ResetKeyHold();
        }
    }

    private void SetDirection(KeyCode key, OngoingDirection direction)
    {
        snakeController.ongoingDirection = direction;
        currentKey = key;
        MoveSnakeInputPressed();
        StartKeyHold();
    }

    public void MoveSnakeInputPressed()
    {
        DirectionalPressed?.Invoke();
        snakeController.AutoMove();
    }

    private void StartKeyHold()
    {
        isHoldingKey = true;
        holdTime = 0f;
        holdThreshold = 0.5f;
    }

    private void ResetKeyHold()
    {
        isHoldingKey = false;
        holdTime = 0f;
        currentKey = KeyCode.None;
        holdThreshold = 0.5f;
    }
}
