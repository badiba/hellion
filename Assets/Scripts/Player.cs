using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HouseManager HouseManager;
    public Action OnEnteredHouse;
    public Action OnLeftHouse;

    private Rigidbody2D _rigidBody2D;
    private float _onHouseNormalizedHorizontalPosition;
    private float _onHouseSpeed = 2;
    private int _onHouseDirection = 1;
    private bool _isOnGround = true;

    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _rigidBody2D.gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == HouseManager.NextHouse?.gameObject)
        {
            _rigidBody2D.gravityScale = 0;
            _isOnGround = true;
            Destroy(collision.gameObject.GetComponent<EdgeCollider2D>());
            HouseManager.MoveToNextTarget();
            OnEnteredHouse?.Invoke();
        }
    }

    private void Update()
    {
        var isMovementOrderGiven = Input.GetKeyDown(KeyCode.X);

        switch (GameManager.Instance.State)
        {
            case GameManager.GameState.WaitingToStart:
                if (isMovementOrderGiven)
                {
                    StartMoving();
                }

                break;
            case GameManager.GameState.Running:
                if (isMovementOrderGiven)
                {
                    SwitchDirection();
                }

                UpdatePosition();
                break;
            case GameManager.GameState.Paused:
                break;
            default:
                break;
        }
    }

    private void UpdatePosition()
    {
        if (_isOnGround)
        {
            var targetHouse = HouseManager.TargetHouse;
            _onHouseNormalizedHorizontalPosition += _onHouseDirection * _onHouseSpeed * Time.deltaTime;
            _onHouseNormalizedHorizontalPosition = Mathf.Clamp(_onHouseNormalizedHorizontalPosition, -1, 1);

            var lerpStart = targetHouse.transform.position + new Vector3(0, 1, -1);
            var lerpEnd = targetHouse.transform.position;
            lerpEnd += _onHouseNormalizedHorizontalPosition > 0 ? new Vector3(1, 0, -1) : new Vector3(-1, 0, -1);
            transform.position = Vector3.Lerp(lerpStart, lerpEnd, Mathf.Abs(_onHouseNormalizedHorizontalPosition));

            if (_onHouseNormalizedHorizontalPosition <= -1 || _onHouseNormalizedHorizontalPosition >= 1)
            {
                _rigidBody2D.gravityScale = 1;
                _isOnGround = false;
                _onHouseNormalizedHorizontalPosition = 0;
                OnLeftHouse?.Invoke();
            }
        }
    }

    private void StartMoving()
    {
        var targetHousePosition = HouseManager.TargetHouse.transform.position;
        var nextHousePosition = HouseManager.NextHouse.transform.position;
        _onHouseDirection = nextHousePosition.x > targetHousePosition.x ? 1 : -1;
        GameManager.Instance.StartGame();
    }

    private void SwitchDirection()
    {
        _onHouseDirection *= -1;
    }
}
