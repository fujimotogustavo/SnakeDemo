using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterController : MonoBehaviour
{
    [SerializeField] private TMP_Text textElement;
    [SerializeField] private TMP_Text scoreTMP;

    private readonly static int rows = 16;
    private readonly static int cols = 9;

    private bool isGameOver = false;
    private int collectedFruits = 0;

    private OngoingDirection ongoingDirection = OngoingDirection.Right;

    private SquareType[,] gridArr = new SquareType[rows, cols];
    private readonly List<Vector2Int> snake = new();

    private float autoMoveTime = 1.0f; // TODO: scale according to collectedFruits
    private float timer = 0.0f;

    private void Awake()
    {
        InitSnake();
        SpawnRandomFruit();
        DisplayGridInString();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            HandleInput();
            HandleAutoMove();
        }
        else
        {
            HandleGameOver();
        }
    }

    private void HandleAutoMove()
    {
        timer += Time.deltaTime;
        if (timer > autoMoveTime)
        {
            autoMoveTime = timer;
            timer = timer - autoMoveTime;
            AutoMove();
        }
    }

    private void HandleGameOver()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            SceneManager.LoadScene("MainScreen");
        }
    }

    private void AutoMove()
    {
        switch (ongoingDirection)
        {
            case OngoingDirection.Up:
                TryMove(0, -1);
                break;
            case OngoingDirection.Down:
                TryMove(0, 1);
                break;
            case OngoingDirection.Left:
                TryMove(-1, 0);
                break;
            case OngoingDirection.Right:
                TryMove(1, 0);
                break;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && ongoingDirection != OngoingDirection.Down)
        {
            ongoingDirection = OngoingDirection.Up;
            TryMove(0, -1);
            timer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.S) && ongoingDirection != OngoingDirection.Up)
        {
            ongoingDirection = OngoingDirection.Down;
            TryMove(0, 1);
            timer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.A) && ongoingDirection != OngoingDirection.Right)
        {
            ongoingDirection = OngoingDirection.Left;
            TryMove(-1, 0);
            timer = 0;
        }
        else if (Input.GetKeyDown(KeyCode.D) && ongoingDirection != OngoingDirection.Left)
        {
            ongoingDirection = OngoingDirection.Right;
            TryMove(1, 0);
            timer = 0;
        }
    }

    private void TryMove(int x, int y)
    {
        Vector2Int snakeHead = snake[snake.Count - 1];
        Vector2Int targetPos = new Vector2Int(snakeHead.x + x, snakeHead.y + y);

        if (IsGameOver(targetPos))
        {
            isGameOver = true;
            textElement.text = "Game Over!\nPress ENTER.";
            return;
        }

        if (gridArr[targetPos.x, targetPos.y] == SquareType.Fruit)
        {
            collectedFruits += 1;
            scoreTMP.text = $"{collectedFruits}";
            SpawnRandomFruit();
        }
        else
        {
            RemoveSnakeTail();
        }
        AddSnakeToPos(targetPos.x, targetPos.y);

        DisplayGridInString();
    }

    private bool IsGameOver(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0)
        {
            return true;
        }
        if (pos.x >= rows || pos.y >= cols)
        {
            return true;
        }
        if (gridArr[pos.x, pos.y] == SquareType.Snake)
        {
            return true;
        }

        return false;
    }

    private void InitSnake()
    {
        int halfX = rows / 2;
        int halfY = cols / 2;

        for (int x = -3; x < 0; x++)
        {
            AddSnakeToPos(halfX + x, halfY);
        }
    }

    private void AddSnakeToPos(int x, int y)
    {
        gridArr[x, y] = SquareType.Snake;
        Vector2Int vector2Int = new Vector2Int(x, y);
        if (!snake.Contains(vector2Int))
        {
            snake.Add(vector2Int);
        }
    }

    private void RemoveSnakeTail()
    {
        Vector2Int tailSquare = snake[0];
        gridArr[tailSquare.x, tailSquare.y] = SquareType.Empty;
        snake.RemoveAt(0);
    }

    private void SpawnRandomFruit()
    {
        while (true)
        {
            int randomX = Random.Range(0, rows);
            int randomY = Random.Range(0, cols);
            if (gridArr[randomX, randomY] == SquareType.Empty)
            {
                gridArr[randomX, randomY] = SquareType.Fruit;
                return;
            }
        }
    }

    private void DisplayGridInString()
    {
        string fullGridString = "";

        for (int y = 0; y < cols; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                switch (gridArr[x, y])
                {
                    case SquareType.Empty:
                        fullGridString += "e ";
                        break;

                    case SquareType.Snake:
                        fullGridString += "S ";
                        break;

                    case SquareType.Fruit:
                        fullGridString += "F ";
                        break;
                }
            }
            fullGridString += "\n";
        }


        textElement.text = fullGridString;
    }
}

public enum SquareType
{
    Empty,
    Snake,
    Fruit
}

public enum OngoingDirection
{
    Up,
    Down,
    Left,
    Right
}
