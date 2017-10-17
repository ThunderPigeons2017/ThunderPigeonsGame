using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSetter : MonoBehaviour
{
    [SerializeField]
    List<MeshRenderer> meshRenderers;

    Material[] primaryMaterials;
    Material[] secondaryMaterials;

    Color primaryColour;
    Color secondaryColour;

        
    void Awake()
    {
        primaryMaterials = new Material[meshRenderers.Count];
        secondaryMaterials = new Material[meshRenderers.Count];

        int i = 0;
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                if (material.name.Contains("Primary"))
                {
                    primaryMaterials[i] = material;
                    continue;
                }
                if (material.name.Contains("Secondary"))
                {
                    secondaryMaterials[i] = material;
                    continue;
                }

            }
            ++i;
        }
    }

    public void UpdateColours()
    {
        foreach (Material material in primaryMaterials)
        {
            if (material != null)
            {
                material.color = primaryColour;
            }
        }
        foreach (Material material in secondaryMaterials)
        {
            if (material != null)
            {
                material.color = secondaryColour;
            }
        }
    }

    public void SetColours(Color primary, Color secondary)
    {
        primaryColour = primary;
        secondaryColour = secondary;

        UpdateColours();
    }
}
