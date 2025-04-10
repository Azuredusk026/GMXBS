using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private TetrominoSpawner tetrominoSpawner;
    [SerializeField] private LevelSpawner levelSpawner;

    void Awake()
    {
        Instance = this;
    }

    public void OnTetrominoPhaseComplete()
    {
        levelSpawner.Spawn();
    }
}