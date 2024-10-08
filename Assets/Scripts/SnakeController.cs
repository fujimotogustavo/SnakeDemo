using System;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] private GridController gridController;

    public event Action FruitEaten;
    public event Action TriggerGameOver;

    public readonly int initialSnakeSize = 3;
    public readonly float maxSpeed = 0.1f;
    public readonly float initialSpeed = 1f;
    public OngoingDirection ongoingDirection = OngoingDirection.Right;
    private readonly List<Vector2Int> snake = new();

    public void AutoMove()
    {
        TryMove(GetCoordinates(ongoingDirection));
    }

    private Vector2Int GetCoordinates(OngoingDirection ongoingDirection)
    {
        switch (ongoingDirection)
        {
            case OngoingDirection.Up:
                return new Vector2Int(0, -1);
            case OngoingDirection.Down:
                return new Vector2Int(0, 1);
            case OngoingDirection.Left:
                return new Vector2Int(-1, 0);
            case OngoingDirection.Right:
                return new Vector2Int(1, 0);
            default:
                return new Vector2Int();
        }
    }
    private void TryMove(Vector2Int receivedVector)
    {
        Vector2Int snakeHead = snake[snake.Count - 1];
        Vector2Int targetPos = new(snakeHead.x + receivedVector.x, snakeHead.y + receivedVector.y);

        if (IsGameOver(targetPos))
        {
            TriggerGameOver?.Invoke();
            return;
        }

        if (gridController.gridArr[targetPos.x, targetPos.y] == SquareType.Fruit)
            FruitEaten?.Invoke();
        else
            RemoveSnakeTail();

        AddSnakeToPos(targetPos.x, targetPos.y);

        uiController.DisplayGridInString();
    }
    private bool IsGameOver(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0)
        {
            return true;
        }
        if (pos.x >= GridController.rows || pos.y >= GridController.cols)
        {
            return true;
        }
        if (gridController.gridArr[pos.x, pos.y] == SquareType.Snake)
        {
            return true;
        }

        return false;
    }
    public void InitSnake()
    {
        int halfX = GridController.rows / 2;
        int halfY = GridController.cols / 2;

        for (int x = -(initialSnakeSize); x < 0; x++)
        {
            AddSnakeToPos(halfX + x, halfY);
        }
    }

    private void AddSnakeToPos(int x, int y)
    {
        gridController.gridArr[x, y] = SquareType.Snake;
        Vector2Int vector2Int = new Vector2Int(x, y);
        if (!snake.Contains(vector2Int))
        {
            snake.Add(vector2Int);
        }
    }

    private void RemoveSnakeTail()
    {
        Vector2Int tailSquare = snake[0];
        gridController.gridArr[tailSquare.x, tailSquare.y] = SquareType.Empty;
        snake.RemoveAt(0);
    }
}
