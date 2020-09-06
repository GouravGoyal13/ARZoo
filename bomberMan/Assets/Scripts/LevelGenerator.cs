using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

enum TileType
{
    ENEMY,
    BOX,
    PLAYER,
    EXIT
}

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject     wallTile;
    [SerializeField] private GameObject     walkTile;
    [SerializeField] private GameObject     boxTile;
    [SerializeField] private GameObject     exitTile;
    [SerializeField] private GameObject     playerTile;
    [SerializeField] private GameObject     enemyTile;
    [SerializeField] private int            maxRow;
    [SerializeField] private int            maxCol;
    [SerializeField] private GameObject     levelContainer;
    [SerializeField] private int            maxBoxCount =25;
    [SerializeField] private int            maxEnemyCount = 3;

    private Dictionary<Vector2, GameObject> levelMapDict;
    private Vector2 playerPosition = Vector2.zero;
    private Vector2 playerOffset = Vector2.zero;
    private Dictionary<Vector2, bool> emptySlotsDict = new Dictionary<Vector2, bool>();


    public int MaxRow { get { return maxRow; } }
    public int MaxCol { get { return maxCol; } }
    public int MaxBoxCount { get { return maxBoxCount; } }
    public int MaxEnemyCount { get { return maxEnemyCount; } }


    // Start is called before the first frame update
    void Start()
    {
        levelMapDict = new Dictionary<Vector2, GameObject>();
        playerPosition = new Vector2(1, maxCol - 2);
        playerOffset = new Vector2(3, maxCol - 4);
        InitLayout();
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.G))
        //{
        //    if (levelContainer.transform.childCount > 1)
        //    {
        //        levelMapDict.Clear();
        //        emptySlotsDict.Clear();
        //        foreach (Transform tran in levelContainer.transform)
        //        {
        //            Destroy(tran.gameObject);
        //        }
        //    }
        //    InitLayout();
        //    ResetGame();
        //}
    }

    public void ResetGame()
    {
        if (levelContainer.transform.childCount > 1)
        {
            levelMapDict.Clear();
            emptySlotsDict.Clear();
            foreach (Transform tran in levelContainer.transform)
            {
                Destroy(tran.gameObject);
            }
        }
        InitLayout();
        foreach (Vector2 slot in levelMapDict.Keys)
        {
            if(levelMapDict[slot].name.Contains("GreenBlock"))
            {
                emptySlotsDict.Add(slot, true);
            }
        }

        PlaceTiles(boxTile, maxBoxCount);
        PlaceTiles(exitTile, 1);
        PlaceTiles(playerTile, 1,true);
        PlaceTiles(enemyTile, maxEnemyCount);
    }

    void InitLayout()
    {
        GameObject go = null;
        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {
                if (row == 0 || col == 0 || row == maxRow - 1 || col == maxCol - 1)
                {
                    go = CreateTile(wallTile, new Vector2(row, col));
                    levelMapDict.Add(new Vector2(row, col), go);
                }
                else if (col % 2 == 0 && row % 2 == 0)
                {
                    go = CreateTile(wallTile, new Vector2(row, col));
                    levelMapDict.Add(new Vector2(row, col), go);
                }
                else
                {
                    go = CreateTile(walkTile, new Vector2(row, col));
                    levelMapDict.Add(new Vector2(row, col), go);
                }
            }
        }
    }

    void PlaceTiles(GameObject prefab, int maxCount,bool isPlayerTile=false)
    {
        for (int i = 0; i < maxCount; i++)
        {
            Vector2 pos = Vector2.zero;
            List<Vector2> _temp = emptySlotsDict.Keys.ToList(); 
            if (!isPlayerTile)
            {
                int random = GetRandomNumber(emptySlotsDict.Count);
                Vector2 _slot = _temp[random];
                while ((_slot.x < playerOffset.x && _slot.y > playerOffset.y) || !emptySlotsDict[_slot])
                {
                    random = GetRandomNumber(emptySlotsDict.Count);
                    _slot = _temp[random];
                }
                pos = _slot;
                emptySlotsDict[pos] = false;
            }
            else
            {
                pos = playerPosition;
                emptySlotsDict[pos] = false;
            }
            CreateTile(prefab,pos);
        }
    }

    int GetRandomNumber(int max)
    {
         return Random.Range(0, max - 1);
    }

    GameObject CreateTile(GameObject prefab, Vector2 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity, levelContainer.transform);
    }
}
