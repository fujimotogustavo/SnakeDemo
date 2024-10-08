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
    public readonly List<Vector2Int> snakeRef = new();

    public void AutoMove()
    {
        TryMove(GetCoordinates(ongoingDirection));
    }

    private Vector2Int GetCoordinates(OngoingDirection ongoingDirection)
    {
        switch (ongoingDirection)
        {
            case OngoingDirection.Up:
                return new Vector2Int(0, 1);
            case OngoingDirection.Down:
                return new Vector2Int(0, -1);
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
        Vector2Int snakeHead = snakeRef[snakeRef.Count - 1];
        Vector2Int targetPos = new(snakeHead.x + receivedVector.x, snakeHead.y + receivedVector.y);

        if (IsGameOver(targetPos))
        {
            TriggerGameOver?.Invoke();
            return;
        }

        if (gridController.gridArr[targetPos.x, targetPos.y] == SquareType.Fruit)
            FruitEaten?.Invoke();
        else
        {
            RemoveSnakeTail();
            gridController.DestroySnakeTailObject();
        }

        AddSnakeToPos(targetPos.x, targetPos.y);
        gridController.InstantiateSingleSnakeObject(targetPos.x, targetPos.y);        
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
        ChangeSquareType(new Vector2Int(x, y), SquareType.Snake);
    }

    private void RemoveSnakeTail()
    {
        ChangeSquareType(snakeRef[0], SquareType.Empty);
    }

    public void ChangeSquareType(Vector2Int squareVector, SquareType newType)
    {
        switch (newType)
        {
            case SquareType.Empty:
                gridController.gridArr[squareVector.x, squareVector.y] = SquareType.Empty;

                if (!gridController.emptySquares.Contains(squareVector))
                    gridController.emptySquares.Add(squareVector);

                if (snakeRef.Contains(squareVector))
                    snakeRef.Remove(squareVector);
                break;

            case SquareType.Snake:
                gridController.gridArr[squareVector.x, squareVector.y] = SquareType.Snake;

                if (!snakeRef.Contains(squareVector))
                    snakeRef.Add(squareVector);

                if (gridController.emptySquares.Contains(squareVector))
                    gridController.emptySquares.Remove(squareVector);
                break;

            case SquareType.Fruit:
                gridController.gridArr[squareVector.x, squareVector.y] = SquareType.Fruit;

                if (gridController.emptySquares.Contains(squareVector))
                    gridController.emptySquares.Remove(squareVector);

                if (snakeRef.Contains(squareVector))
                    snakeRef.Remove(squareVector);
                break;
        }
    }
}