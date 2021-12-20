using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : MonoBehaviour
{
    public Player Player;
    public Text ScoreTextWidget;

    private ScoreText _scoreText;

    private void Start()
    {
        GameManager.Instance.OnPlayerScored += OnPlayerScored;
        _scoreText = new ScoreText(ScoreTextWidget);
    }

    private void Update()
    {
        _scoreText.Update();
    }

    private void OnPlayerScored()
    {
        _scoreText.StartAnimation();
        _scoreText.UpdateText(GameManager.Instance.Score.ToString());
    }
}
