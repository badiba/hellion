using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float OnHouseSpeed { get; private set; } = 2f;

    public HouseManager HouseManager;
    public Action OnStartedMoving;
    public Action OnEnteredHouse;
    public Action OnLeftHouse;
    public Action OnDyingStarted;
    public Action OnDied;

    private const float _activeGravityScale = 5f;
    private Rigidbody2D _rigidBody2D;
    private Vector3 _startingPosition;
    private float _onHouseNormalizedHorizontalPosition;
    private int _onHouseDirection = 1;
    private bool _isOnGround = true;
    private bool _isAlive = true;

    public void SetOnHouseSpeed(float speed)
    {
        OnHouseSpeed = speed;
    }

    private void Start()
    {
        GameManager.Instance.OnGameRestarted += OnGameRestarted;

        _startingPosition = transform.position;
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _rigidBody2D.gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == HouseManager.NextHouse?.gameObject)
        {
            EnterHouse(collision.gameObject);
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
            case GameManager.GameState.Playing:
                if (isMovementOrderGiven)
                {
                    SwitchDirection();
                }

                UpdatePosition();

                if (_isAlive && transform.position.y < HouseManager.NextHouse.transform.position.y)
                {
                    Die();
                }
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
            _onHouseNormalizedHorizontalPosition += _onHouseDirection * OnHouseSpeed * Time.deltaTime;
            _onHouseNormalizedHorizontalPosition = Mathf.Clamp(_onHouseNormalizedHorizontalPosition, -1, 1);

            var lerpStart = targetHouse.transform.position + new Vector3(0, 1, -1);
            var lerpEnd = targetHouse.transform.position;
            lerpEnd += _onHouseNormalizedHorizontalPosition > 0 ? new Vector3(1, 0, -1) : new Vector3(-1, 0, -1);
            transform.position = Vector3.Lerp(lerpStart, lerpEnd, Mathf.Abs(_onHouseNormalizedHorizontalPosition));

            if (_onHouseNormalizedHorizontalPosition <= -1 || _onHouseNormalizedHorizontalPosition >= 1)
            {
                LeaveHouse();
            }
        }
    }

    private void StartMoving()
    {
        var targetHousePosition = HouseManager.TargetHouse.transform.position;
        var nextHousePosition = HouseManager.NextHouse.transform.position;
        _onHouseDirection = nextHousePosition.x > targetHousePosition.x ? 1 : -1;
        OnStartedMoving?.Invoke();
        GameManager.Instance.StartGame();
    }

    private void SwitchDirection()
    {
        _onHouseDirection *= -1;
    }

    private void EnterHouse(GameObject house)
    {
        _rigidBody2D.gravityScale = 0;
        _isOnGround = true;
        Destroy(house.GetComponent<EdgeCollider2D>());
        HouseManager.MoveToNextTarget();
        OnEnteredHouse?.Invoke();
    }

    private void LeaveHouse()
    {
        _rigidBody2D.gravityScale = _activeGravityScale;
        _isOnGround = false;
        _onHouseNormalizedHorizontalPosition = 0;
        OnLeftHouse?.Invoke();
    }

    private void Die()
    {
        _isAlive = false;
        _rigidBody2D.gravityScale = 0;
        _rigidBody2D.velocity = Vector2.zero;
        OnDyingStarted?.Invoke();

        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(1);

        _rigidBody2D.gravityScale = _activeGravityScale;

        OnDied?.Invoke();
        GameManager.Instance.EndGame();
    }

    private void OnGameRestarted()
    {
        _rigidBody2D.gravityScale = 0;
        _rigidBody2D.velocity = Vector2.zero;
        transform.position = _startingPosition;
        OnHouseSpeed = 2f;
        _onHouseNormalizedHorizontalPosition = 0f;
        _onHouseDirection = 1;
        _isOnGround = true;
        _isAlive = true;
    }
}
