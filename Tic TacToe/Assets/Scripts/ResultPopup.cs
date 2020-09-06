using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TicTacResult
{
	public enum ResultState { WIN, DRAW };
    public string WinMessage { get; set; }
    public int Score { get; set; }
    public ResultState State { get; set; }
    public TicTacResult(string winMessage, int score,ResultState resultState)
    {
        WinMessage = winMessage;
        Score = score;
        State = resultState;
    }
}

public class ResultPopup : MonoBehaviour 
{
    public static object context;
	[SerializeField] private Text WinMessageText;
    [SerializeField] private Text ScoreText;
    [SerializeField] private Animation popupInAnim;
    private TicTacResult result;

	private void OnEnable()
	{
        popupInAnim.Play();
        if (context is TicTacResult)
        {
            result = ((TicTacResult)context);
            if (result.State == TicTacResult.ResultState.DRAW)
            {
                WinMessageText.text = "Draw";
                ScoreText.text = "";
            }
            else
            {
                WinMessageText.text = string.Format("{0} Won!",result.WinMessage);
                ScoreText.text = string.Format("Score : {0}", result.Score);
            }
        }
	}

    public void OnExitClick()
    {
        if (GameManager.Instance.OnGameExitEvent != null)
            GameManager.Instance.OnGameExitEvent.Invoke();
        gameObject.SetActive(false);
    }

    public void OnRestartClick()
    {
        if (GameManager.Instance.OnGameRestartEvent != null)
            GameManager.Instance.OnGameRestartEvent.Invoke();
        gameObject.SetActive(false);
    }
}
