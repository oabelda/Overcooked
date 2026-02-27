using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class PlayerColor : MonoBehaviour
{
    [SerializeField] string materialName = "body";
    string colorProperty = "_Color";
    [SerializeField] Color playerColor = Color.white;

    struct TargetSlot
    {
        public Renderer renderer;
        public int materialIndex;
    }

    readonly List<TargetSlot> targets = new List<TargetSlot>();

    MaterialPropertyBlock propertyBlock;
    bool initialized;

    void OnEnable()
    {
        Initialize();
        ApplyColor();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        initialized = false; // fuerza recolección
        Initialize();
        ApplyColor();
    }
#endif

    void Initialize()
    {
        if (initialized)
            return;

        targets.Clear();

        var renderers = GetComponentsInChildren<Renderer>(true);

        foreach (var r in renderers)
        {
            var mats = r.sharedMaterials;

            for (int i = 0; i < mats.Length; i++)
            {
                var mat = mats[i];

                if (mat != null && mat.name.StartsWith(materialName))
                {
                    targets.Add(new TargetSlot
                    {
                        renderer = r,
                        materialIndex = i
                    });
                }
            }
        }

        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();

        initialized = true;
    }

    void ApplyColor()
    {
        foreach (var t in targets)
        {
            if (!t.renderer) continue;

            t.renderer.GetPropertyBlock(propertyBlock, t.materialIndex);

            propertyBlock.SetColor(colorProperty, playerColor);

            t.renderer.SetPropertyBlock(propertyBlock, t.materialIndex);
        }
    }

    public void SetColor(Color newColor)
    {
        playerColor = newColor;
        ApplyColor();
    }
}