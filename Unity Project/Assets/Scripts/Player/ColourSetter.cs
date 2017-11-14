using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSetter : MonoBehaviour
{
    [SerializeField]
    GameObject meshParent;

    Renderer[] meshRenderers;

    List<Material> materials = new List<Material>();

    [HideInInspector]
    public Color primaryColour;
    [HideInInspector]
    public Color secondaryColour;

    //void Awake()
    //{
    //    FindMeshRenderers();
    //    FindMaterials();
    //}

    void FindMaterials()
    {
        materials.Clear();

        foreach (Renderer meshRenderer in meshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                materials.Add(material);
            }
        }
    }

    void FindMeshRenderers()
    {
        meshRenderers = meshParent.GetComponentsInChildren<Renderer>();
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
        foreach (Material material in materials)
        {
            material.SetColor("_Primary", primaryColour);
            material.SetColor("_Secondary", secondaryColour);
        }
    }

    public void SetColours(Color primary, Color secondary)
    {
        primaryColour = primary;
        secondaryColour = secondary;

        UpdateColours();
    }
}
