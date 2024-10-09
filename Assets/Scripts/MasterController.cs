using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour
{
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private InputController inputController;
    [SerializeField] private GridController gridController;
    [SerializeField] private UIController uiController;
    [SerializeField] private CameraController cameraController;

    private bool isGameOver = false;
    private float timer = 0.0f;
    private float autoMoveTime;

    private void Awake()
    {
        inputController.DirectionalPressed += OnDirectionalPressedEvent;
        snakeController.FruitEaten += OnFruitEatenEvent;
        snakeController.TriggerGameOver += OnTriggerGameOver;
        snakeController.InitSnake();
        gridController.InitEmptySquareList();
        gridController.SpawnRandomFruit();
        gridController.InstantiateSnakeObjects();
        gridController.InstantiateWalls();
        CalculateAutoMove();
        cameraController.AdjustOrthographicCameraForGrid();
    }

    private void OnTriggerGameOver()
    {
        isGameOver = true;
        uiController.ChangeTextElementString("Game Over!\nPress ENTER.");
        uiController.ActivateGameOverObj();
        return;
    }

    private void OnFruitEatenEvent()
    {
        CalculateAutoMove();
        gridController.collectedFruits++;
        uiController.ChangeScoreText($"{gridController.collectedFruits}");
        gridController.SpawnRandomFruit();
    }

    private void OnDirectionalPressedEvent()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            inputController.HandleInput();
            if (!inputController.isHoldingKey)
                HandleAutoMove();
        }
        else
            HandleGameOver();
    }

    public void ResetTimer()
    {
        timer = 0.0f;
    }

    private void HandleAutoMove()
    {
        timer += Time.deltaTime;
        if (timer > autoMoveTime)
        {
            timer = 0;
            snakeController.AutoMove();
        }
    }

    private void HandleGameOver()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            SceneManager.LoadScene("MainScreen");
        }
    }

    private void CalculateAutoMove()
    {
        autoMoveTime = (gridController.collectedFruits / gridController.maxFruitsPossible * -1)
            + snakeController.initialSpeed + snakeController.maxSpeed;
    }
}