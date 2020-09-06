using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    GameOver,
    Running
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject RestartPopup;
    [SerializeField] LevelGenerator levelGenerator;
    [SerializeField] Text scoreText;
    [SerializeField] Text gameOverMessaageText;

    int score;
    GameState gameState;

    public GameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }

    private void Awake()
    {
        BombBehaviour.OnBombHit += OnObjectHit;
        if(levelGenerator==null)
        {
            Debug.Log("LevelGenerator Not Found");
        }
        scoreText.text = "Score : 0";
    }

    private void Start()
    {
        PlayerMovement.OnGameOver+= PlayerMovement_OnGameOver;
    }

    void PlayerMovement_OnGameOver(object sender, GameOverEvent e)
    {
        GameState = GameState.GameOver;
        if(e.IsWon)
        {
            OnGameOver("You Won! Play Again.");
        }
        else
        {
            OnGameOver("You Lost! Try Again.");
        }
    }

    void UpdateScore()
    {
        scoreText.text = string.Format("Score : {0}", score );
    }

    private void OnDestroy()
    {
        BombBehaviour.OnBombHit -= OnObjectHit;
        PlayerMovement.OnGameOver -= PlayerMovement_OnGameOver;
    }

    void OnObjectHit(object sender, BombHit e)
    {
        if(e.HitObjectName.Equals("Player"))
        {
            PlayerMovement_OnGameOver(this, new GameOverEvent(false));
        }
        if(e.HitObjectName.Equals("Enemy"))
        {
            Debug.Log("Enemy ScoreUpdate");
            score += e.Score;
            UpdateScore();
        }
    }

    public void RestartGame()
    {
        levelGenerator.ResetGame();
        ResetScore();
        RestartPopup.SetActive(false);
    }

    void ResetScore()
    {
        score = 0;
        scoreText.text = "Score : 0";
    }

    public void OnGameOver(string msg)
    {
        gameOverMessaageText.text = msg;
        RestartPopup.SetActive(true);
        gameState = GameState.Running;
    }
}
