using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class BlockTextureController : MonoBehaviour
{
    [Header("材质配置")]
    [SerializeField] private Material sideMaterial;  // 侧面材质
    [SerializeField] private Material bottomMaterial; // 底面材质
    [SerializeField] private Material[] topMaterials; // 顶面材质选项（至少1个）

    // 面名称关键词（根据模型导出时的材质命名修改！）
    private const string BOTTOM_KEY = "Bottom";
    private const string TOP_KEY = "Top";
    private const string SIDE_KEY = "Side"; // 或其他侧面标识如 Front/Back/Left/Right

    private void Awake()
    {
        ApplyMaterials();
    }

    /// <summary>
    /// 强制按面名称分配材质（不依赖材质数组顺序）
    /// </summary>
    public void ApplyMaterials()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer == null || renderer.sharedMaterials.Length == 0)
        {
            Debug.LogError("MeshRenderer或材质未设置!", gameObject);
            return;
        }

        // 创建新材质数组
        Material[] newMaterials = new Material[renderer.sharedMaterials.Length];

        // 遍历所有子材质，按名称分配
        for (int i = 0; i < renderer.sharedMaterials.Length; i++)
        {
            string matName = renderer.sharedMaterials[i].name.ToLower();

            if (matName.Contains(BOTTOM_KEY.ToLower()))
            {
                newMaterials[i] = bottomMaterial;
            }
            else if (matName.Contains(TOP_KEY.ToLower()))
            {
                newMaterials[i] = GetRandomTopMaterial();
            }
            else if (matName.Contains(SIDE_KEY.ToLower())) 
            {
                newMaterials[i] = sideMaterial;
            }
            else
            {
                // 未识别的面保持原材质
                newMaterials[i] = renderer.sharedMaterials[i];
                Debug.LogWarning($"未识别的材质面: {renderer.sharedMaterials[i].name}", gameObject);
            }
        }

        renderer.sharedMaterials = newMaterials;
    }

    private Material GetRandomTopMaterial()
    {
        if (topMaterials == null || topMaterials.Length == 0)
        {
            Debug.LogWarning("未设置顶面材质，使用默认材质", gameObject);
            return new Material(Shader.Find("Standard"));
        }
        return topMaterials[Random.Range(0, topMaterials.Length)];
    }

#if UNITY_EDITOR
    [ContextMenu("Debug Log Material Names")]
    private void DebugLogMaterialNames()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Debug.Log("当前材质顺序：" + string.Join(", ", 
            System.Array.ConvertAll(renderer.sharedMaterials, m => m?.name)));
    }
#endif
}