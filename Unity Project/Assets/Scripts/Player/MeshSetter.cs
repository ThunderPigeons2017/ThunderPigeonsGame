using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSetter : MonoBehaviour
{
    GameObject meshParent;

    [SerializeField]
    GameObject meshPrefab;

    ColourSetter colourSetter;

    CSManager csManager;

    public enum Character
    {
        RANDOM,
        BARRY,
        LARRY,
        SALLY,
        GARY
    }

    public Character currentCharacter = Character.RANDOM;

    void Awake()
    {
        colourSetter = GetComponent<ColourSetter>();
        csManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<CSManager>();
    }

    void Start()
    {
        if (meshParent == null)
        {
            CreateNewMesh();
        }

        if (currentCharacter == Character.RANDOM)
        {
            // Set the mesh to a random one
            SetMeshPrefab(csManager.meshPrefabs[Random.Range((int)Character.BARRY, (int)Character.GARY)]);
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
