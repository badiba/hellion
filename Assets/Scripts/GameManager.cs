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

    public Action OnPlayerScored;

    private const float _cameraSpeed = 20f;
    private float _cameraOffset;

    public void StartGame()
    {
        State = GameState.Running;
    }

    public void IncreaseScore()
    {
        Score++;
        OnPlayerScored?.Invoke();
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
}
