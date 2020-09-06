using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGateBehaviour : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    private LevelGenerator levelGenerator;
    private int destroyedBoxCount = 0;
    private int destroyedEnemyCount = 0;

    private bool isGateOpen;

    public bool IsGateOpen
    {
        get { return isGateOpen; }
        private set { isGateOpen = value; }
    }

    bool CheckGateStatus()
    {
        if(destroyedBoxCount==levelGenerator.MaxBoxCount && destroyedEnemyCount == levelGenerator.MaxEnemyCount)
            {
                sprite.color = new Color(121 / 255, 154 / 255, 47 / 255);
                return true;
            }
        return false;
    }

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        if (levelGenerator == null)
            Debug.LogError("LevelGenerator not Found");
        BombBehaviour.OnBombHit+= BombBehaviour_OnBoxDestroy;
    }

    private void OnDestroy()
    {
        BombBehaviour.OnBombHit -= BombBehaviour_OnBoxDestroy;
    }

    void BombBehaviour_OnBoxDestroy(object sender, BombHit e)
    {
        if(e.HitObjectName.Equals("block"))
            destroyedBoxCount++;
        if (e.HitObjectName.Equals("Enemy"))
            destroyedEnemyCount++;
        IsGateOpen = CheckGateStatus();
    }
}
