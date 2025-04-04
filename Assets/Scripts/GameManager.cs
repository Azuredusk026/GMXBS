using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSpawnPoint;
    
    [SerializeField] GameObject[] tetrominoPrefabs;
    [SerializeField] Transform tetrominoSpawnPoint;
    [SerializeField] Sprite[] tetrominoSprites;
    [SerializeField] Image[] nextBlockImages; 
    private Queue<GameObject> _nextBlocksQueue = new Queue<GameObject>();
    private Dictionary<GameObject, Sprite> _blockSpriteMap = new Dictionary<GameObject, Sprite>();
    
    [SerializeField] public Transform endPoint;
    public bool canSpawn = true;
    const int HEIGHT = 22;
    
    void Awake()
    {
        Instance = this;
        InitializeBlockSpriteMap(); // 初始化方块与Sprite的映射
        InitializeNextBlocksQueue(); // 初始化队列
        UpdateNextBlocksUI(); // 更新UI
        SpawnTetromino();
    }
    
    void InitializeBlockSpriteMap()
    {
        for (int i = 0; i < tetrominoPrefabs.Length; i++)
        {
            _blockSpriteMap.Add(tetrominoPrefabs[i], tetrominoSprites[i]);
        }
    }
    
    void InitializeNextBlocksQueue()
    {
        for (int i = 0; i < 3; i++)
        {
            AddRandomBlockToQueue();
        }
    }
    
    void AddRandomBlockToQueue()
    {
        GameObject randomPrefab = tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Length)];
        _nextBlocksQueue.Enqueue(randomPrefab);
    }
    
    void UpdateNextBlocksUI()
    {
        GameObject[] nextBlocks = _nextBlocksQueue.ToArray();
        for (int i = 0; i < 3; i++)
        {
            if (i < nextBlocks.Length && _blockSpriteMap.ContainsKey(nextBlocks[i]))
            {
                nextBlockImages[i].sprite = _blockSpriteMap[nextBlocks[i]];
            }
        }
    }
    
    public void SpawnTetromino()
    {
        if (!canSpawn || tetrominoSpawnPoint.position.z >= HEIGHT || 
            Vector3.Distance(tetrominoSpawnPoint.position, endPoint.position) < 1f)
        {
            canSpawn = false;
            SpawnPlayer();
            return;
        }

        // 从队列头部取出下一个方块
        GameObject nextBlock = _nextBlocksQueue.Dequeue();
        Instantiate(nextBlock, tetrominoSpawnPoint.position, Quaternion.identity);

        // 补充新方块到队列
        AddRandomBlockToQueue();
        UpdateNextBlocksUI(); // 刷新UI
    }
    
    void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
    }
}