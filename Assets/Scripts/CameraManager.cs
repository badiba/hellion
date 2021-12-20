using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Player Player;

    private const float _cameraSpeed = 20f;
    private float _cameraOffset;
    private Vector3 _initialPosition;

    private void Start()
    {
        GameManager.Instance.OnGameRestarted += OnGameRestarted;
        _cameraOffset = transform.position.y - Player.transform.position.y;
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.Playing)
        {
            UpdateCameraPosition();
        }
    }

    private void UpdateCameraPosition()
    {
        var playerPosition = Player.transform.position;
        var targetCameraPosition = new Vector3(playerPosition.x, playerPosition.y + _cameraOffset, -10);

        if (targetCameraPosition.y < transform.position.y)
        {
            var cameraSpeed = (transform.position.y - targetCameraPosition.y) * _cameraSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetCameraPosition, cameraSpeed);
        }
    }

    private void OnGameRestarted()
    {
        transform.position = _initialPosition;
    }
}
