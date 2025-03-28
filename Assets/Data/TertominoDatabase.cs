using UnityEngine;

[CreateAssetMenu(fileName = "TetrominoDB", menuName = "Tetris/Database")]
public class TetrominoDatabase : ScriptableObject {
    public TetrominoData[] tetrominos;
}