using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;

public class FaceBehaviour : MonoBehaviour
{
    [SerializeField] int gridSize = 6; // 6x6

    [SerializeField] int actualExpresion;

    [SerializeField] Renderer rend;
    [SerializeField] int materialIndex = 0; // material de la cara
    string textureProperty = "_MainTex";

    MaterialPropertyBlock block;

    void Awake()
    {
        block = new MaterialPropertyBlock();
    }

    private void Update()
    {
        SetFace(actualExpresion);
    }

    public void SetFace(int index)
    {
        int max = gridSize * gridSize;
        index = ((index % max) + max) % max;

        int x = index % gridSize;
        int y = index / gridSize;

        float size = 1f / gridSize;

        float offsetX = x * size;
        float offsetY = 1f - size - y * size;

        rend.GetPropertyBlock(block, materialIndex);

        // Solo modificamos OFFSET (zw)
        Vector4 st = block.GetVector(textureProperty + "_ST");

        // preservamos tiling actual
        if (st == Vector4.zero)
            st = new Vector4(1, 1, 0, 0);

        st.z = offsetX;
        st.w = offsetY;

        block.SetVector(textureProperty + "_ST", st);

        rend.SetPropertyBlock(block, materialIndex);
    }

    public int GetActualFace()
    {
        return actualExpresion;
    }
}
