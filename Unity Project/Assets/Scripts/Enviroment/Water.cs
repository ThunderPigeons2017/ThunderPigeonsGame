using UnityEngine;
using System.Collections;



public class Water : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed that the waves move")]
    float waveSpeed = 0.5f;
    [SerializeField]
    [Tooltip("The +/- y height of the main waves")]
    float waveHeight = 0.48f;
    [SerializeField]
    [Tooltip("The +/- y height of the triangulation (kinda like choppyness at sea)")]
    float triangulationHeight = 0.1f;

    Mesh mesh;
    Vector3[] verts;

    float offsetX;
    float offsetY;

    void Awake()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MakeMeshLowPoly(mf);
    }

    void Start()
    {
        string seed = Time.time.ToString();

        offsetX = seed.GetHashCode() % 10000;
        offsetY = offsetX;
    }

    MeshFilter MakeMeshLowPoly(MeshFilter mf)
    {
        // We want each tri to have its own unshared verts

        mesh = mf.sharedMesh;
        Vector3[] oldVerts = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            vertices[i] = oldVerts[triangles[i]];
            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        verts = mesh.vertices;
        return mf;
    }

    void Update()
    {
        CalcWave();
        offsetX += Time.deltaTime * waveSpeed;
    }

    void CalcWave()
    {
        // Loop through every vert and change its y
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 v = verts[i];
            v.y = 0; // Reset the y so we can add the new values

            // Add position to make it relatice to world space
            Vector3 worldPos = v + transform.position;

            // Perlin noise wave 1 (bigger underlying wave)
            float xCoord = offsetX + worldPos.x / 100;
            float zCoord = offsetY + worldPos.z / 100;
            v.y += Helper.Remap(Mathf.PerlinNoise(xCoord, zCoord), 0, 1, 0, waveHeight) - waveHeight / 2;

            // Perlin noise wave 2 (small more frequent waves to add choppyness to the top)
            xCoord = offsetX + worldPos.x / 2;
            zCoord = offsetY + worldPos.z / 2;
            v.y += Helper.Remap(Mathf.PerlinNoise(xCoord, zCoord), 0, 1, 0, triangulationHeight) - triangulationHeight / 2;

            //v.y += waveHeight3 / 5 * Mathf.Sin(Time.time * Mathf.PI * 2.0f * 0.1f);

            verts[i] = v;
        }
        mesh.vertices = verts;
        mesh.RecalculateNormals();
        mesh.MarkDynamic();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}