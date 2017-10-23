using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSetter : MonoBehaviour
{
    [SerializeField]
    GameObject meshParent;

    [SerializeField]
    GameObject meshPrefab;

    ColourSetter colourSetter;

    // The index of the mesh prefabs array 
    public int meshNumber;

    void Awake()
    {
        colourSetter = GetComponent<ColourSetter>();
        meshNumber = 0;
    }

    void Start()
    {
        if (meshParent == null)
        {
            CreateNewMesh();
        }
	}

    void CreateNewMesh()
    {
        GameObject newMesh = Instantiate(meshPrefab, gameObject.transform); // Create the new gameObject with the right parent

        newMesh.transform.position += new Vector3(0.0f, -0.5f, 0.0f); // Put the pivot at the centre of the ball

        meshParent = newMesh; // Set the meshParent to the correct

        colourSetter.SetMeshParent(meshParent);
    }

    void UpdateMesh()
    {
        if (meshParent != null)
        {
            Destroy(meshParent);
            meshParent = null;
        }

        CreateNewMesh();
    }

    public void SetMeshPrefab(GameObject newMeshPrefab)
    {
        meshPrefab = newMeshPrefab;
        UpdateMesh();
    }
}
