using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private SnakeController snakeController;
    public GameObject snakePrefab;
    public GameObject emptyPrefab;
    public GameObject fruitPrefab;

    public Transform gridParent;

    public readonly static int rows = 32;
    public readonly static int cols = 18;
    public float collectedFruits = 0;
    public float maxFruitsPossible;

    public SquareType[,] gridArr = new SquareType[rows, cols];

    public readonly List<Vector2Int> emptySquares = new();

    private readonly GameObject[,] gridObjects = new GameObject[rows, cols];
    private readonly List<GameObject> wallObjects = new();
    public readonly List<GameObject> snakeObjects = new();
    private GameObject fruitObject;

    public void DestroySnakeTailObject()
    {
        if (snakeObjects.Count > 0)
        {
            Destroy(snakeObjects[0]);
            snakeObjects.RemoveAt(0);
        }
    }
    public void InstantiateSingleSnakeObject(int x, int y)
    {
        Vector3 position = new Vector3(x, 0, y);
        GameObject snakeObj = Instantiate(snakePrefab, position, Quaternion.identity, gridParent);
        snakeObjects.Add(snakeObj);
    }

    public void DestroyAllSnakeObjects()
    {
        foreach (GameObject snake in snakeObjects)
            Destroy(snake);

        snakeObjects.Clear();
    }

    public void InstantiateSnakeObjects()
    {
        DestroyAllSnakeObjects();

        foreach (Vector2Int snakePos in snakeController.snakeRef)
        {
            Vector3 position = new Vector3(snakePos.x, 0, snakePos.y);
            GameObject snakeObj = Instantiate(snakePrefab, position, Quaternion.identity, gridParent);
            snakeObjects.Add(snakeObj);
        }
    }

    public void DestroyWalls()
    {
        foreach (GameObject wall in wallObjects)
            Destroy(wall);

        wallObjects.Clear();
    }

    public void InstantiateWalls()
    {
        DestroyWalls();

        for (int y = 0; y < cols; y++)
        {
            Vector3 leftWallPosition = new Vector3(-1, 0, y);
            GameObject wallObj1 = Instantiate(emptyPrefab, leftWallPosition, Quaternion.identity, gridParent);

            Vector3 rightWallPosition = new Vector3(rows, 0, y);
            GameObject wallObj2 = Instantiate(emptyPrefab, rightWallPosition, Quaternion.identity, gridParent);

            wallObjects.Add(wallObj1);
            wallObjects.Add(wallObj2);
        }

        for (int x = -1; x <= rows; x++)
        {
            Vector3 topWallPosition = new Vector3(x, 0, cols);
            GameObject wallObj1 = Instantiate(emptyPrefab, topWallPosition, Quaternion.identity, gridParent);

            Vector3 bottomWallPosition = new Vector3(x, 0, -1);
            GameObject wallObj2 = Instantiate(emptyPrefab, bottomWallPosition, Quaternion.identity, gridParent);

            wallObjects.Add(wallObj1);
            wallObjects.Add(wallObj2);
        }
    }

    private void Awake()
    {
        maxFruitsPossible = (rows * cols) - snakeController.initialSnakeSize;
    }

    public void SpawnRandomFruit()
    {
        int randomInRange = Random.Range(0, emptySquares.Count);
        InstantiateFruit(emptySquares[randomInRange]);
        snakeController.ChangeSquareType(emptySquares[randomInRange], SquareType.Fruit);
    }

    public void InstantiateFruit(Vector2Int fruitPos)
    {
        if (fruitObject != null)
            Destroy(fruitObject);

        Vector3 position = new Vector3(fruitPos.x, 0, fruitPos.y);
        fruitObject = Instantiate(fruitPrefab, position, Quaternion.identity, gridParent);
    }

    public void InitEmptySquareList()
    {
        for (int y = 0; y < cols; y++)
            for (int x = 0; x < rows; x++)
                if (gridArr[x, y] == SquareType.Empty)
                    emptySquares.Add(new Vector2Int(x, y));
    }
}