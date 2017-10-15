using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    [SerializeField]
    public Color baseColor;
    [SerializeField]
    public Color primaryColor;
    [SerializeField]
    public Color secondaryColor;

    MeshRenderer meshRenderer;

    Material baseMaterial;
    Material primaryMaterial;
    Material secondaryMaterial;

    void Awake ()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        foreach (Material material in meshRenderer.materials)
        {
            Debug.Log(material.name);

            if (material.name.Contains("Base"))
            {
                baseMaterial = material;
                continue;
            }
            if (material.name.Contains("Primary"))
            {
                primaryMaterial = material;
                continue;
            }
            if (material.name.Contains("Secondary"))
            {
                secondaryMaterial = material;
                continue;
            }
        }

        //baseMaterial = meshRenderer.materials[0];
        //primaryMaterial = meshRenderer.materials[1];
        //secondaryMaterial = meshRenderer.materials[2];

        baseMaterial.color = baseColor;
        primaryMaterial.color = primaryColor;
        secondaryMaterial.color = secondaryColor;
    }
	
	void Update ()
    {
        baseMaterial.color = baseColor;
        primaryMaterial.color = primaryColor;
        secondaryMaterial.color = secondaryColor;
    }
}
