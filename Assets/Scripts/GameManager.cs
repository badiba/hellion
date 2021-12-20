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
        Playing,
        Paused,
        Ended
    }

    public static GameManager Instance;

    public Player Player;

    public GameState State { get; private set; }
    public int Score { get; private set; }
    public int PassedHouseCount { get; private set; }

    public Action OnPlayerScored;
    public Action OnGameStarted;
    public Action OnGameEnded;

    private const float _speedIncreasePerDifficulty = 0.1f;
    private const int _difficultyIncreaseInterval = 5;
    private int _difficulty = 0;

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

        Player.OnStartedMoving += OnPlayerStartedMoving;
        Player.OnEnteredHouse += OnPlayerEnteredHouse;
        Player.OnDied += OnPlayerDied;
    }

    private void Update()
    {
        
    }

    private void StartGame()
    {
        State = GameState.Playing;
        OnGameStarted?.Invoke();
    }

    private void EndGame()
    {
        State = GameState.Ended;
        Time.timeScale = 0f;
        OnGameEnded?.Invoke();
    }

    private void OnPlayerStartedMoving()
    {
        StartGame();
    }

    private void OnPlayerDied()
    {
        EndGame();
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
