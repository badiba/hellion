using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : MonoBehaviour
{
    public Player Player;
    public Text ScoreTextWidget;
    public Text PausePanelScoreText;
    public Text PausePanelHighestScoreText;
    public GameObject PausePanel;

    private ScoreText _scoreText;

    public void ExecuteRestart()
    {
        _scoreText.UpdateText(0.ToString());
        PausePanel.SetActive(false);
        GameManager.Instance.RestartGame();
    }

    private void Start()
    {
        GameManager.Instance.OnPlayerScored += OnPlayerScored;
        GameManager.Instance.OnGameEnded += OnGameEnded;
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

    private void OnGameEnded()
    {
        PausePanelScoreText.text = $"Score: {GameManager.Instance.Score}";
        PausePanelHighestScoreText.text = $"Highest Score: {GameManager.Instance.HighestScore}";
        PausePanel.SetActive(true);
    }
}
