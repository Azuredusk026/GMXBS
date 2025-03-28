using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    public GameObject[] blocks;
    public Transform tilemapGrid; // 引用TilemapGrid的Transform
    private static GameObject[] _blocks;
    private static Transform _transform;
    private static Transform _tilemapGrid; // 静态引用

    void Start()
    {
        _blocks = blocks;
        _transform = transform;
        _tilemapGrid = tilemapGrid; // 初始化静态引用
        Spawn();
    }

    public static void Spawn()
    {
        if (_blocks == null || _blocks.Length == 0 || _tilemapGrid == null)
        {
            Debug.LogError("Blocks array or TilemapGrid reference not set!");
            return;
        }

        int blockIndex = Random.Range(0, _blocks.Length);
        GameObject newBlock = Instantiate(_blocks[blockIndex], _transform.position, quaternion.identity);
        
        // 将新生成的block设置为TilemapGrid的子对象
        newBlock.transform.SetParent(_tilemapGrid);
    }
}