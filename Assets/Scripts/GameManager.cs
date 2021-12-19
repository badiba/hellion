using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera MainCamera;
    public Player Player;

    private const float _cameraSpeed = 20f;
    private float _cameraOffset;

    private void Start()
    {
        _cameraOffset = MainCamera.transform.position.y - Player.transform.position.y;
    }

    private void Update()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        var targetVector = new Vector3(Player.transform.position.x, Player.transform.position.y + _cameraOffset, -10);

        if (targetVector.y < MainCamera.transform.position.y)
        {
            var cameraSpeed = (MainCamera.transform.position.y - targetVector.y) * _cameraSpeed * Time.deltaTime;
            MainCamera.transform.position = Vector3.MoveTowards(MainCamera.transform.position, targetVector, cameraSpeed);
        }
    }
}
