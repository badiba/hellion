using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        WaitingToStart,
        Running,
        Paused
    }

    public static GameManager Instance;

    public Camera MainCamera;
    public Player Player;

    public GameState State { get; private set; }
    public int Score { get; private set; }
    public int PassedHouseCount { get; private set; }

    public Action OnPlayerScored;

    private const float _speedIncreasePerDifficulty = 0.1f;
    private const int _difficultyIncreaseInterval = 5;
    private const float _cameraSpeed = 20f;
    private float _cameraOffset;
    private int _difficulty = 0;

    public void StartGame()
    {
        State = GameState.Running;
        Player.OnEnteredHouse += OnPlayerEnteredHouse;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        State = GameState.WaitingToStart;
        _cameraOffset = MainCamera.transform.position.y - Player.transform.position.y;
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.WaitingToStart:
                break;
            case GameState.Running:
                UpdateCameraPosition();
                break;
            default:
                break;
        }
    }

    private void UpdateCameraPosition()
    {
        var playerPosition = Player.transform.position;
        var targetCameraPosition = new Vector3(playerPosition.x, playerPosition.y + _cameraOffset, -10);
        var mainCameraTransform = MainCamera.transform;

        if (targetCameraPosition.y < mainCameraTransform.position.y)
        {
            var cameraSpeed = (mainCameraTransform.position.y - targetCameraPosition.y) * _cameraSpeed * Time.deltaTime;
            mainCameraTransform.position = Vector3.MoveTowards(mainCameraTransform.position, targetCameraPosition, cameraSpeed);
        }
    }

    private void OnPlayerEnteredHouse()
    {
        Score++;
        PassedHouseCount++;

        if (PassedHouseCount % _difficultyIncreaseInterval == 0)
        {
            IncreaseDifficulty();
        }

        OnPlayerScored?.Invoke();
    }

    private void IncreaseDifficulty()
    {
        _difficulty++;
        Player.SetOnHouseSpeed(Player.OnHouseSpeed + _speedIncreasePerDifficulty);
    }
}
