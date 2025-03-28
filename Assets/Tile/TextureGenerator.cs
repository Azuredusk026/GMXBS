using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextureGenerator : MonoBehaviour
{
    [MenuItem("Assets/Create/2D/Custom Block Tile")]
    public static void CreateBlockTile()
    {
        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = CreateSprite(Color.white);
        AssetDatabase.CreateAsset(tile, "Assets/Tile/BlockTile.asset");
    }

    [MenuItem("Assets/Create/2D/Custom Cliff Tile")]
    public static void CreateCliffTile()
    {
        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = CreateSprite(Color.gray);
        AssetDatabase.CreateAsset(tile, "Assets/Tile/CliffTile.asset");
    }

    private static Sprite CreateSprite(Color color)
    {
        var texture = new Texture2D(16, 16);
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, 16, 16), Vector2.one * 0.5f);
    }
}