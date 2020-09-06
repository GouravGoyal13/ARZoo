using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Direction
{
    None = 0,
    Right = 1,
    Left = -1,
    Up = 2,
    Down = -2
}
public class GameOverEvent
{
    public bool IsWon { get; set; }
    public GameOverEvent(bool isWon)
    {
        IsWon = isWon;
    }
}
public class PlayerMovement : Movement
{
    public static event EventHandler<GameOverEvent> OnGameOver;

    [SerializeField] private int firePower;
    [SerializeField] private GameObject bomb;

    private bool isBombActive;

    public override void Update()
    {
        UserControl();
        base.Update();
    }

    public void UserControl()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            direction = Direction.Right;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction = Direction.Up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction = Direction.Down;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            PlantBomb();
        }
    }

    void PlantBomb()
    {
        if (!isBombActive)
        {
            GameObject bombGo = Instantiate(bomb, this.transform.position, Quaternion.identity, this.transform.parent);
            BombBehaviour bombObj = bombGo.GetComponent<BombBehaviour>();
            if (bombObj != null)
            {
                isBombActive = true;
                BombBehaviour.OnBombPlanted += BombObj_OnBombExploded;
            }
        }
    }

    void BombObj_OnBombExploded(object sender, EventArgs e)
    {
        isBombActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if (OnGameOver != null)
                OnGameOver(this, new GameOverEvent(isWon: false));
            Destroy(gameObject);
        }

        if (other.CompareTag("Exit"))
        {
            ExitGateBehaviour exitGateBehaviour = other.GetComponent<ExitGateBehaviour>();
            if (exitGateBehaviour != null)
            {
                if (exitGateBehaviour.IsGateOpen)
                {
                    if (OnGameOver != null)
                        OnGameOver(this,new GameOverEvent(isWon: true));
                }
            }
        }
    }
}
