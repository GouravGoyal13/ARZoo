using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour 
{
    //Inspector Variables
    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject cross;
    [SerializeField] private bool isMarked;

	private string cellId;
    private int playerId;
    private bool show;

	// Use this for initialization
	void Awake () 
    {
        if (GameManager.Instance.turnChangeEvent != null)
            GameManager.Instance.turnChangeEvent.AddListener(OnTurnChangeEvent);
        cellId = gameObject.name;
	}

    private void OnTurnChangeEvent(int arg0)
    {
        playerId = arg0;
        Debug.Log(arg0);
    }

	public void onClick () 
    {
        if (!isMarked)
        {
            if (playerId == 0)
                circle.SetActive(!show);
            else
                cross.SetActive(!show);
            if (GameManager.Instance.onPlayerMoveEvent != null)
            {
                GameManager.Instance.onPlayerMoveEvent.Invoke(cellId);
            }
        }
        isMarked = true;
	}

	private void OnDestroy()
	{
        if (GameManager.Instance.turnChangeEvent != null)
            GameManager.Instance.turnChangeEvent.RemoveListener(OnTurnChangeEvent);
	}

    public void Reset()
    {
        circle.SetActive(false);
        cross.SetActive(false);
        //playerId = 0;
        isMarked = false;
        show = false;
    }
}
