using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour
{
    public TMP_Text textElement;
    static int rows = 16;
    static int cols = 9;
    bool isGameOver = false;

    SquareType[,] gridArr = new SquareType[rows, cols];
    List<Vector2Int> snake = new List<Vector2Int>();

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
        }
        else {
            HandleGameOver();
        }
    }

    void HandleGameOver()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            SceneManager.LoadScene("MainScreen");
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TryMove(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(1, 0);
        }
    }

    void TryMove(int x, int y)
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
            SpawnRandomFruit();
        }
        else
        {
            RemoveSnakeTail();
        }
        AddSnakeToPos(targetPos.x, targetPos.y);

        DisplayGridInString();
    }

    bool IsGameOver(Vector2Int pos)
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

    void InitSnake()
    {
        int halfX = rows / 2;
        int halfY = cols / 2;

        for (int x = -3; x < 0; x++)
        {
            AddSnakeToPos(halfX + x, halfY);
        }
    }

    void AddSnakeToPos(int x, int y)
    {
        gridArr[x, y] = SquareType.Snake;
        Vector2Int vector2Int = new Vector2Int(x, y);
        if (!snake.Contains(vector2Int))
        {
            snake.Add(vector2Int);
        }
    }

    void RemoveSnakeTail()
    {
        Vector2Int tailSquare = snake[0];
        gridArr[tailSquare.x, tailSquare.y] = SquareType.Empty;
        snake.RemoveAt(0);
    }

    void SpawnRandomFruit()
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

    void DisplayGridInString()
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
