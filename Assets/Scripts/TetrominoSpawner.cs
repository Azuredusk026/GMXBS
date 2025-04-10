using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TetrominoSpawner : MonoBehaviour
{
    [Header("方块生成设置")]
    [SerializeField] GameObject[] tetrominoPrefabs;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Sprite[] tetrominoSprites;
    [SerializeField] Image[] nextBlockImages;
    
    [Header("游戏边界设置")]
    [SerializeField] public Transform endPoint;
    [SerializeField] int heightLimit = 13;
    
    private Queue<GameObject> _nextBlocksQueue = new Queue<GameObject>();
    private Dictionary<GameObject, Sprite> _blockSpriteMap = new Dictionary<GameObject, Sprite>();
    public bool CanSpawn { get; set; } = true;

    public static TetrominoSpawner Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InitializeBlockSpriteMap();
        InitializeNextBlocksQueue();
        UpdateNextBlocksUI();
        SpawnNext();
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
        for (int i = 0; i < nextBlockImages.Length; i++)
        {
            if (i < nextBlocks.Length && _blockSpriteMap.ContainsKey(nextBlocks[i]))
            {
                nextBlockImages[i].sprite = _blockSpriteMap[nextBlocks[i]];
            }
        }
    }

    public void SpawnNext()
    {
        if (!CanSpawn || spawnPoint.position.z >= heightLimit || 
            Vector3.Distance(spawnPoint.position, endPoint.position) < 1f)
        {
            CanSpawn = false;
            GameManager.Instance.OnTetrominoPhaseComplete();
            return;
        }

        GameObject nextBlock = _nextBlocksQueue.Dequeue();
        Instantiate(nextBlock, spawnPoint.position, Quaternion.identity);
        AddRandomBlockToQueue();
        UpdateNextBlocksUI();
    }
}