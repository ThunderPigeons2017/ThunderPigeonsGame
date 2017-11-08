using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSetter : MonoBehaviour
{
    [SerializeField]
    GameObject meshParent;

    MeshRenderer[] meshRenderers;

    Material[] primaryMaterials;
    Material[] secondaryMaterials;

    Color primaryColour;
    Color secondaryColour;

    //void Awake()
    //{
    //    FindMeshRenderers();
    //    FindMaterials();
    //}

    void FindMaterials()
    {
        primaryMaterials = new Material[meshRenderers.Length];
        secondaryMaterials = new Material[meshRenderers.Length];

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

    void FindMeshRenderers()
    {
        meshRenderers = meshParent.GetComponentsInChildren<MeshRenderer>();
    }

    public void SetMeshParent(GameObject newMeshParent)
    {
        meshParent = newMeshParent;

        FindMeshRenderers();
        FindMaterials();
        UpdateColours();
    }

    public void UpdateColours()
    {
        if (primaryMaterials != null)
            foreach (Material material in primaryMaterials)
            {
                if (material != null)
                {
                    material.color = primaryColour;
                }
            }

        if (secondaryMaterials != null)
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
