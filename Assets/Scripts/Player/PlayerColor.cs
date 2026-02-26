using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class PlayerColor : MonoBehaviour
{
    [SerializeField] Material bodyMaterial; // material original
    [SerializeField] Color playerColor = Color.white;

    Material instanceMaterial;

    List<Renderer> renderers = new List<Renderer>();

    void OnEnable()
    {
        CollectRenderers();
        ApplyColor();
    }

    void OnValidate()
    {
        CollectRenderers();
        ApplyColor();
    }

    void CollectRenderers()
    {
        renderers.Clear();

        foreach (var r in GetComponentsInChildren<Renderer>(true))
        {
            foreach (var mat in r.sharedMaterials)
            {
                if (mat == bodyMaterial)
                {
                    renderers.Add(r);
                    break;
                }
            }
        }
    }

    void ApplyColor()
    {
        if (!bodyMaterial)
            return;

        if (instanceMaterial == null)
        {
            instanceMaterial = new Material(bodyMaterial);
            instanceMaterial.name = bodyMaterial.name + "_Instance";
        }

        instanceMaterial.color = playerColor;

        foreach (var r in renderers)
        {
            var mats = r.sharedMaterials;

            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i] == bodyMaterial)
                    mats[i] = instanceMaterial;
            }

            r.sharedMaterials = mats;
        }
    }
}