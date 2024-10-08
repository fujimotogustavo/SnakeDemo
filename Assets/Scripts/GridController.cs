using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private SnakeController snakeController;

    public readonly static int rows = 16;
    public readonly static int cols = 9;
    public float collectedFruits = 0;
    public SquareType[,] gridArr = new SquareType[rows, cols];
    public float maxFruitsPossible;
    public readonly List<Vector2Int> emptySquares = new();

    private void Awake()
    {
        maxFruitsPossible = (rows * cols) - snakeController.initialSnakeSize;
    }

    public void SpawnRandomFruit()
    {
        int randomInRange = Random.Range(0, emptySquares.Count);
        snakeController.ChangeSquareType(emptySquares[randomInRange], SquareType.Fruit);
    }

    public void InitEmptySquareList()
    {
        for (int y = 0; y < cols; y++)
            for (int x = 0; x < rows; x++)
                if (gridArr[x, y] == SquareType.Empty)
                    emptySquares.Add(new Vector2Int(x, y));

    }
}