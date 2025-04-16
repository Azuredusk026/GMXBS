using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BlockTextureController : MonoBehaviour
{
    [Header("材质配置")]
    public Material[] topMaterials;  // 顶面材质选项
    public Material sideMaterial;    // 侧面材质（四面一致）
    public Material bottomMaterial;  // 底部材质

    void Awake()
    {
        ApplyMaterials();
    }

    public void ApplyMaterials()
    {
        var renderer = GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogError("找不到 MeshRenderer", gameObject);
            return;
        }

        // 每个方块自己随机顶面材质
        Material selectedTopMaterial = GetRandomTopMaterial();

        renderer.sharedMaterials = new Material[]
        {
            selectedTopMaterial, // 顶面
            sideMaterial,        // 侧面
            bottomMaterial       // 底部
        };
    }

    private Material GetRandomTopMaterial()
    {
        if (topMaterials == null || topMaterials.Length == 0)
        {
            Debug.LogWarning("顶面材质为空", gameObject);
            return new Material(Shader.Find("Standard")); // 或 fallback 方案
        }

        return topMaterials[Random.Range(0, topMaterials.Length)];
    }
}