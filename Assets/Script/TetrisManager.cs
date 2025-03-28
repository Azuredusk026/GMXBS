using UnityEngine;
using UnityEngine.Tilemaps;

public class TetrisManager : MonoBehaviour
{
    [Header("Tilemap References")]
    public Tilemap activeTilemap;    // 当前活动方块 Tilemap
    public Tilemap staticTilemap;   // 已固定方块 Tilemap
    public Tilemap cliffTilemap;    // 底边方块 Tilemap
    
    [Header("Tile Assets")]
    public Tile blockTile;          // 主要方块 Tile
    public Tile cliffBlockTile;     // 底边方块 Tile
    
    [Header("Spawn Settings")]
    public Transform spawnPoint;

    private Tetromino currentTetromino; // 当前活动的俄罗斯方块

    void Start()
    {
        SpawnTetromino();
    }

    void Update()
    {
        // 处理输入
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentTetromino.Move(Vector3Int.left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            currentTetromino.Move(Vector3Int.right);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            currentTetromino.Move(Vector3Int.down);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            currentTetromino.Rotate();
        
        // 检查碰撞
        CheckCollision();
    }

    // 生成新的俄罗斯方块
    void SpawnTetromino()
    {
        // 创建新的 Tetromino 对象
        currentTetromino = new GameObject("Tetromino").AddComponent<Tetromino>();
        currentTetromino.transform.SetParent(transform);
        
        // 设置 Tilemap 引用
        currentTetromino.mainTilemap = activeTilemap;
        currentTetromino.cliffTilemap = cliffTilemap;
        currentTetromino.mainTile = blockTile;
        currentTetromino.cliffTile = cliffBlockTile;

        // 随机选择方块类型
        switch (Random.Range(0, 4))
        {
            case 0: // I 形
                currentTetromino.mainCells = new Vector3Int[]
                {
                    new Vector3Int(0, 0, 0),
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, 2, 0),
                    new Vector3Int(0, 3, 0)
                };
                currentTetromino.cliffCells = new Vector3Int[]
                {
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(0, -2, 0)
                };
                break;
            case 1: // O 形
                currentTetromino.mainCells = new Vector3Int[]
                {
                    new Vector3Int(0, 0, 0),
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(1, 1, 0)
                };
                currentTetromino.cliffCells = new Vector3Int[]
                {
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(1, -1, 0)
                };
                break;
            case 2: // L 形
                currentTetromino.mainCells = new Vector3Int[]
                {
                    new Vector3Int(0, 0, 0),
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, 2, 0),
                    new Vector3Int(1, 0, 0)
                };
                currentTetromino.cliffCells = new Vector3Int[]
                {
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(1, -1, 0)
                };
                break;
            case 3: // T 形
                currentTetromino.mainCells = new Vector3Int[]
                {
                    new Vector3Int(0, 0, 0),
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(0, 1, 0)
                };
                currentTetromino.cliffCells = new Vector3Int[]
                {
                    new Vector3Int(-1, -1, 0),
                    new Vector3Int(0, -1, 0),
                    new Vector3Int(1, -1, 0)
                };
                break;
        }

        // 设置初始位置并渲染
        currentTetromino.position = new Vector3Int(
            Mathf.RoundToInt(spawnPoint.position.x),
            Mathf.RoundToInt(spawnPoint.position.y),
            0
        );
        currentTetromino.SetTiles(Vector3Int.zero);
    }

    // 检查碰撞
    void CheckCollision()
    {
        // 检查底部碰撞
        foreach (Vector3Int cell in currentTetromino.mainCells)
        {
            Vector3Int tilePos = currentTetromino.position + cell + Vector3Int.down;
            
            // 如果碰到底部边界或已固定的方块
            if (tilePos.y < 0 || staticTilemap.HasTile(tilePos))
            {
                MergeToStatic();
                SpawnTetromino();
                return;
            }
        }
    }

    // 将当前方块合并到静态 Tilemap
    void MergeToStatic()
    {
        // 合并主要方块
        foreach (Vector3Int cell in currentTetromino.mainCells)
        {
            staticTilemap.SetTile(currentTetromino.position + cell, currentTetromino.mainTile);
        }
        
        // 合并底边方块
        foreach (Vector3Int cell in currentTetromino.cliffCells)
        {
            cliffTilemap.SetTile(currentTetromino.position + cell, currentTetromino.cliffTile);
        }
        
        // 清除活动 Tilemap
        activeTilemap.ClearAllTiles();
        
        // 销毁当前 Tetromino 对象
        Destroy(currentTetromino.gameObject);
    }
}