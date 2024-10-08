using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private SnakeController snakeController;

    public readonly static int rows = 16;
    public readonly static int cols = 9;
    public float collectedFruits = 0;
    public SquareType[,] gridArr = new SquareType[rows, cols];
    public float maxFruitsPossible;
    
    private void Awake()
    {
        maxFruitsPossible = (rows * cols) - snakeController.initialSnakeSize;
    }

    public void SpawnRandomFruit()
    {
        while (true)
        {
            int randomX = UnityEngine.Random.Range(0, rows);
            int randomY = UnityEngine.Random.Range(0, cols);

            if (gridArr[randomX, randomY] == SquareType.Empty)
            {
                gridArr[randomX, randomY] = SquareType.Fruit;
                return;
            }
        }
    }
}