using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TetrominoBlock : MonoBehaviour {
    // 静态数据库引用（避免每个实例都拖拽）
    private static TetrominoDatabase _database;
    public static void SetDatabase(TetrominoDatabase db) => _database = db;

    // 初始化方法（只需类型）
    public void Initialize(TetrominoType type) {
        TetrominoData data = GetData(type);
        GenerateMesh(data);
        ApplyColor(data.color);
    }

    private TetrominoData GetData(TetrominoType type) {
        foreach (var t in _database.tetrominos) {
            if (t.type == type) return t;
        }
        throw new System.Exception($"No data for {type}");
    }

    private void GenerateMesh(TetrominoData data) {
        CombineInstance[] combines = new CombineInstance[data.cells.Length];
        
        for (int i = 0; i < data.cells.Length; i++) {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localPosition = (Vector3Int)data.cells[i];
            
            combines[i].mesh = cube.GetComponent<MeshFilter>().sharedMesh;
            combines[i].transform = cube.transform.localToWorldMatrix;
            DestroyImmediate(cube);
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combines);
        
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void ApplyColor(Color color) {
        GetComponent<Renderer>().material.color = color;
    }
}