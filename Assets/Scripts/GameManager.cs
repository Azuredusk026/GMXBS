using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float AutoDropTime => autoDropTime;

    public static GameManager Instance;
    public bool IsBuildingPhase { get; private set; } = true;
    
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSpawnPoint;
    
    [SerializeField] GameObject[] tetrominoPrefabs;
    [SerializeField] Transform tetrominoSpawnerTransform;
    
    [SerializeField] float autoDropTime = 0f;

    [SerializeField] public Transform endPoint;
    public bool canSpawn = true;
    const int HEIGHT = 22;
    
    void Awake()
    {
        Instance = this;
        SpawnTetromino();
    }
    
    public void SpawnTetromino()
    {
        // 检查是否超过最大高度
        if(!canSpawn || tetrominoSpawnerTransform.position.z >= HEIGHT || 
           Vector3.Distance(tetrominoSpawnerTransform.position, endPoint.position) < 1f)
        {
            canSpawn = false;
            SpawnPlayer();
            return;
        }
        
        Instantiate(tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Length)], 
            tetrominoSpawnerTransform.position, 
            Quaternion.identity);
    }
    
    void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
    }
}