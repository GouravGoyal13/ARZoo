using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;


public class GameBehaviour : MonoBehaviour
{
    //Inspector Variables
    [SerializeField] private Text turnText;
    [SerializeField] private GameObject Player1BG;
    [SerializeField] private GameObject Player2BG;
    [SerializeField] private Text Player1ScoreText;
    [SerializeField] private Text Player2ScoreText;

    //private variables
    private int playerId = 0;
    private int Player1Score = 0;
    private int Player2Score = 0;
    private int playerMoveCount = 0;

    //cache all marks in cell according to cellid
    private Dictionary<string, int> arrayGrid = new Dictionary<string, int>();
    //cache all cells to check result
    private List<CellBehaviour> cellList = new List<CellBehaviour>();

    void OnEnable()
    {
        if (GameManager.Instance.turnChangeEvent != null)
            GameManager.Instance.turnChangeEvent.AddListener(OnTurnChangeEvent);
        if (GameManager.Instance.onPlayerMoveEvent != null)
            GameManager.Instance.onPlayerMoveEvent.AddListener(OnPlayerMoveEvent);
        if (GameManager.Instance.OnGameRestartEvent != null)
            GameManager.Instance.OnGameRestartEvent.AddListener(OnGameRestart);
        cellList = FindObjectsOfType<CellBehaviour>().ToList();
        ResetGame();
    }

    private void OnGameRestart()
    {
        ResetGame();
    }

    //Init Grid dictionary with -1 as default values so that we can assign player 1 as 0 and player 2 as 1
    private void InitGrid()
    {
        arrayGrid.Clear();
        arrayGrid.Add("00", -1);
        arrayGrid.Add("01", -1);
        arrayGrid.Add("02", -1);
        arrayGrid.Add("10", -1);
        arrayGrid.Add("11", -1);
        arrayGrid.Add("12", -1);
        arrayGrid.Add("20", -1);
        arrayGrid.Add("21", -1);
        arrayGrid.Add("22", -1);
    }

    private void OnPlayerMoveEvent(string cellId)
    {
        arrayGrid[cellId] = playerId;
        playerMoveCount++;

        bool result = CheckForResult(cellId);
        if (playerMoveCount == 9 && !result)
        {
            if (GameManager.Instance.OnGameCompleteEvent != null)
                GameManager.Instance.OnGameCompleteEvent.Invoke(new TicTacResult("", 0, TicTacResult.ResultState.DRAW));
        }
        if (!result)
        {
            if (playerId == 0)
                Player1Score += 10;
            else
                Player2Score += 10;

            if (GameManager.Instance.turnChangeEvent != null)
            {
                playerId = playerId == 0 ? 1 : 0;
                GameManager.Instance.turnChangeEvent.Invoke(playerId);
            }
            UpdateScore();
        }
        else
        {
            if (GameManager.Instance.OnGameCompleteEvent != null)
            {
                string winMessage = GetPlayerName(playerId);
                int score = getPlayerScore(playerId);
                GameManager.Instance.OnGameCompleteEvent.Invoke(new TicTacResult(winMessage,score,TicTacResult.ResultState.WIN));
            }
        }
    }

    private void OnTurnChangeEvent(int arg0)
    {
        playerId = arg0;
        Debug.Log(playerId);
        ActivatePlayerBG();
        string playerName = GetPlayerName(playerId);
        turnText.text = string.Format("{0} Turn", playerName);
    }

    private bool CheckForResult(string cellId)
    {
        switch (cellId)
        {
            case "00":
                if ((arrayGrid["01"] == playerId && arrayGrid["02"] == playerId) || (arrayGrid["10"] == playerId && arrayGrid["20"] == playerId) || (arrayGrid["11"] == playerId && arrayGrid["22"] == playerId))
                {
                    Debug.Log(GetPlayerName(playerId) + "won !");
                    return true;
                }
                break;
            case "01":
                if ((arrayGrid["00"] == playerId && arrayGrid["02"] == playerId) || (arrayGrid["11"] == playerId && arrayGrid["21"] == playerId))
                {
                    Debug.Log(GetPlayerName(playerId) + "won !");
                    return true;
                }
                break;
            case "02":
                if ((arrayGrid["00"] == playerId && arrayGrid["01"] == playerId) || (arrayGrid["12"] == playerId && arrayGrid["22"] == playerId) || (arrayGrid["11"] == playerId && arrayGrid["20"] == playerId))
                {
                    Debug.Log(GetPlayerName(playerId) + "won !");
                    return true;
                }
                break;
            case "10":
                if ((arrayGrid["00"] == playerId && arrayGrid["20"] == playerId) || (arrayGrid["11"] == playerId && arrayGrid["12"] == playerId))
                {
                    Debug.Log(GetPlayerName(playerId) + "won !");
                    return true;
                }
                break;
            case "11":
                if ((arrayGrid["00"] == playerId && arrayGrid["22"] == playerId) || (arrayGrid["02"] == playerId && arrayGrid["20"] == playerId) || (arrayGrid["01"] == playerId && arrayGrid["21"] == playerId) || (arrayGrid["10"] == playerId && arrayGrid["12"] == playerId))
                {
                    Debug.Log(GetPlayerName(playerId) + "won !");
                    return true;
                }
                break;
            case "12":
                if ((arrayGrid["10"] == playerId && arrayGrid["11"] == playerId) || (arrayGrid["02"] == playerId && arrayGrid["22"] == playerId))
                {
                    Debug.Log(GetPlayerName(playerId) + "won !");
                    return true;
                }
                break;
            case "20":
                if ((arrayGrid["00"] == playerId && arrayGrid["10"] == playerId) || (arrayGrid["21"] == playerId && arrayGrid["22"] == playerId) || (arrayGrid["11"] == playerId && arrayGrid["02"] == playerId))
                {
                    Debug.Log(GetPlayerName(playerId) + "won !");
                    return true;
                }
                break;
            case "21":
                if ((arrayGrid["20"] == playerId && arrayGrid["22"] == playerId) || (arrayGrid["11"] == playerId && arrayGrid["01"] == playerId))
                {
                    Debug.Log(GetPlayerName(playerId) + "won !");
                    return true;
                }
                break;
            case "22":
                if ((arrayGrid["20"] == playerId && arrayGrid["21"] == playerId) || (arrayGrid["12"] == playerId && arrayGrid["02"] == playerId) || (arrayGrid["11"] == playerId && arrayGrid["00"] == playerId))
                {
                    Debug.Log(GetPlayerName(playerId) + "won !");
                    return true;
                }
                break;
        }
        return false;
    }

    private void OnDisable()
    {
        if (GameManager.Instance.turnChangeEvent != null)
            GameManager.Instance.turnChangeEvent.RemoveListener(OnTurnChangeEvent);
        if (GameManager.Instance.onPlayerMoveEvent != null)
            GameManager.Instance.onPlayerMoveEvent.RemoveListener(OnPlayerMoveEvent);
        if (GameManager.Instance.OnGameRestartEvent != null)
            GameManager.Instance.OnGameRestartEvent.RemoveListener(OnGameRestart);
    }

    private void UpdateScore()
    {
            Player1ScoreText.text = Player1Score.ToString();
            Player2ScoreText.text = Player2Score.ToString();
    }

    void ResetGame()
    {
        foreach (CellBehaviour cell in cellList)
        {
            cell.Reset();
        }
        InitGrid();
        //start with random player
        playerId = UnityEngine.Random.Range(0, 2);
        playerMoveCount = 0;
        Player1Score = 0;
        Player2Score = 0;
        UpdateScore();
        GameManager.Instance.turnChangeEvent.Invoke(playerId);
    }

    void ActivatePlayerBG()
    {
        Player1BG.SetActive(playerId == 0);
        Player2BG.SetActive(playerId == 1);
    }

    private int getPlayerScore(int playerId)
    {
        return playerId == 0 ? Player1Score : Player2Score;
    }

    string GetPlayerName(int id)
    {
        return id == 0 ? "Player 1" : "Player 2";
    }

}
