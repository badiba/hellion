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
    public int HighestScore { get; private set; }
    public int PassedHouseCount { get; private set; }

    public Action OnPlayerScored;
    public Action OnGameStarted;
    public Action OnGameEnded;
    public Action OnGameRestarted;

    private const float _speedIncreasePerDifficulty = 0.1f;
    private const int _difficultyIncreaseInterval = 5;
    private int _difficulty = 0;

    public void RestartGame()
    {
        _difficulty = 0;
        PassedHouseCount = 0;
        Score = 0;
        State = GameState.WaitingToStart;
        OnGameRestarted?.Invoke();
    }

    public void StartGame()
    {
        var saveData = SaveManager.LoadGame();
        HighestScore = saveData.HighestScore;

        State = GameState.Playing;
        OnGameStarted?.Invoke();
    }

    public void EndGame()
    {
        if (Score > HighestScore)
        {
            HighestScore = Score;
            SaveManager.SaveGame();
        }

        State = GameState.Ended;
        OnGameEnded?.Invoke();
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

        Player.OnEnteredHouse += OnPlayerEnteredHouse;
    }

    private void Update()
    {
        
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
