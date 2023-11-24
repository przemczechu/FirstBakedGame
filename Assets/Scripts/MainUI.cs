using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : Singleton<MainUI>
{
    public ProgressBar progressBar;
    public GameObject buttonsPanel;
    public GameObject gameOverPanel;
    public Text scoreText;

    void Start()
    {
        ShowGameView();
    }

    public void ShowGameView() {
        buttonsPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOverView(float points)
    {
        buttonsPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        scoreText.text = "Score: " + points;
    }

    public void RestartButtonAction()
    {
        Debug.Log("RESTART");
        ShowGameView();
        MainGame.FindInstance.StartNewGame();
    }

    public void ExitButtonAction()
    {
        Debug.Log("EXIT");
    }
}
