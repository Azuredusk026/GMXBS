using UnityEngine;

public class TetrominoTester : MonoBehaviour {
    [SerializeField] private TetrominoDatabase database;
    [SerializeField] private TetrominoType spawnType = TetrominoType.I;
    [SerializeField] private Vector3 spawnPosition = new Vector3(0, 10, 0);

    private void Start() {
        TetrominoBlock.SetDatabase(database); // 全局设置一次数据库
        Spawn();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) Spawn();
    }

    private void Spawn() {
        var go = new GameObject($"Tetromino_{spawnType}");
        go.transform.position = spawnPosition;
        
        var piece = go.AddComponent<TetrominoBlock>();
        piece.Initialize(spawnType);
    }
}