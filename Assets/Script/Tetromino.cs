using UnityEngine;
using UnityEngine.Tilemaps;

public class Tetromino : MonoBehaviour
{
    public Vector3Int position;
    public Tilemap mainTilemap;  // 主体 Tilemap
    public Tilemap cliffTilemap; // 底边 Tilemap
    public Tile mainTile;   // 主要方块 Tile
    public Tile cliffTile;  // 底边 Tile

    // 俄罗斯方块结构
    public Vector3Int[] mainCells;  // 主要块坐标
    public Vector3Int[] cliffCells; // 底边块坐标

    public void SetTiles(Vector3Int offset)
    {
        mainTilemap.ClearAllTiles();
        cliffTilemap.ClearAllTiles();

        foreach (Vector3Int cell in mainCells)
            mainTilemap.SetTile(position + cell + offset, mainTile);
        
        foreach (Vector3Int cell in cliffCells)
            cliffTilemap.SetTile(position + cell + offset, cliffTile);
    }

    public void Move(Vector3Int direction)
    {
        position += direction;
        SetTiles(Vector3Int.zero);
    }

    public void Rotate()
    {
        for (int i = 0; i < mainCells.Length; i++)
            mainCells[i] = new Vector3Int(-mainCells[i].y, mainCells[i].x, 0);
        for (int i = 0; i < cliffCells.Length; i++)
            cliffCells[i] = new Vector3Int(-cliffCells[i].y, cliffCells[i].x, 0);

        SetTiles(Vector3Int.zero);
    }
}