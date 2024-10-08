using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public event Action DirectionalPressed;
    [SerializeField] private SnakeController snakeController;

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && snakeController.ongoingDirection != OngoingDirection.Down)
        {
            snakeController.ongoingDirection = OngoingDirection.Up;
            MoveSnakeInputPressed();
        }
        else if (Input.GetKeyDown(KeyCode.S) && snakeController.ongoingDirection != OngoingDirection.Up)
        {
            snakeController.ongoingDirection = OngoingDirection.Down;
            MoveSnakeInputPressed();
        }
        else if (Input.GetKeyDown(KeyCode.A) && snakeController.ongoingDirection != OngoingDirection.Right)
        {
            snakeController.ongoingDirection = OngoingDirection.Left;
            MoveSnakeInputPressed();
        }
        else if (Input.GetKeyDown(KeyCode.D) && snakeController.ongoingDirection != OngoingDirection.Left)
        {
            snakeController.ongoingDirection = OngoingDirection.Right;
            MoveSnakeInputPressed();
        }
    }

    public void MoveSnakeInputPressed()
    {
        DirectionalPressed?.Invoke();
        snakeController.AutoMove();
    }
}
