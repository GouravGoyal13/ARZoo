using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    
    public UnityEvent OnGameRestartEvent = new UnityEvent();
    public UnityEvent OnGameExitEvent = new UnityEvent();
    public OnTurnChange turnChangeEvent = new OnTurnChange();
    public OnPlayerMove onPlayerMoveEvent = new OnPlayerMove();
    public OnGameComplete OnGameCompleteEvent = new OnGameComplete();

    [SerializeField] private GameObject StartScreen;
    [SerializeField] private GameObject GameScreen;
    [SerializeField] private ResultPopup ResultPopup;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        if (OnGameCompleteEvent != null)
            OnGameCompleteEvent.AddListener(OnGameComplete);
        
        if (OnGameExitEvent != null)
            OnGameExitEvent.AddListener(OnGameExit);
    }

    private void OnGameExit()
    {
        GameScreen.SetActive(false);
        StartScreen.SetActive(true);
    }

    private void OnGameComplete(TicTacResult gameResult)
    {
        ResultPopup.context = gameResult;
        ResultPopup.gameObject.SetActive(true);
    }

	void OnDisable () {
        if (OnGameCompleteEvent != null)
            OnGameCompleteEvent.RemoveListener(OnGameComplete);
       
        if (OnGameExitEvent != null)
            OnGameExitEvent.RemoveListener(OnGameExit);
	}

    public void StartGame()
    {
        StartScreen.SetActive(false);
        GameScreen.SetActive(true);
    }
}

public class OnTurnChange : UnityEvent<int>
{
}
public class OnPlayerMove : UnityEvent<string>
{
}
public class OnGameComplete : UnityEvent<TicTacResult>
{
}
