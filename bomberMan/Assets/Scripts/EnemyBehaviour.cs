using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Movement
{
    [SerializeField] int enemyKillScore = 100;

    public int EnemyKillScore { get { return enemyKillScore; } }

    Direction previousdir = 0;

    int[] moveDir = new int[] {
        (int)Direction.Right,
        (int)Direction.Left,
        (int)Direction.Up,
        (int)Direction.Down
    };

    public override void Update()
    {
        direction = GetRandomDirection();
        base.Update();
    }

    //Get random direction for enemy movement
    Direction GetRandomDirection()
    {
        int random = Random.Range(0, 4);
        Direction dir = (Direction)moveDir[random];
        //If previous direction is same as new direction get a new direction, so that it wont go in same direction repeatedly.
        while (previousdir == dir)
        {
            random = Random.Range(0, 4);
            dir = (Direction)moveDir[random];
            previousdir = dir;
        }
        return dir;
    }
}
