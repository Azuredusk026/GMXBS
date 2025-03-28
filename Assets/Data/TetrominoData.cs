using UnityEngine;

[System.Serializable]
public struct TetrominoData {
    public TetrominoType type;
    public Vector2Int[] cells; // 相对坐标
    public Color color;
}

public enum TetrominoType { I, O, T, L } // 基础4种