using UnityEngine;
using System.Collections;



public class Water : MonoBehaviour
{

    Vector3 waveSource1 = new Vector3(2.0f, 0.0f, 2.0f);
    public float waveSpeed = 0.5f;
    public float waveHeight = 0.48f;
    public float triangulationHeight = 0.1f;
    //public float waveHeight3 = 0.1f;

    Mesh mesh;
    Vector3[] verts;

    float offsetX;
    float offsetY;

    void Awake()
    {
        //Camera.main.depthTextureMode |= DepthTextureMode.Depth;
        MeshFilter mf = GetComponent<MeshFilter>();
        makeMeshLowPoly(mf);
    }

    void Start()
    {
        string seed = Time.time.ToString();

        offsetX = seed.GetHashCode() % 10000;
        offsetY = offsetX;
    }

    MeshFilter makeMeshLowPoly(MeshFilter mf)
    {
        mesh = mf.sharedMesh;//Change to sharedmesh? 
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

    // Update is called once per frame
    void Update()
    {
        CalcWave();
        offsetX += Time.deltaTime * waveSpeed;

    }

    void CalcWave()
    {
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 v = verts[i];
            v.y = 0;

            // Perlin noise wave 1
            float xCoord = offsetX + v.x / 100 * 1;// scale;
            float zCoord = offsetY + v.z / 100 * 1;// scale;

            v.y += Remap(Mathf.PerlinNoise(xCoord, zCoord), 0, 1, 0, waveHeight) - waveHeight / 2;

            xCoord = offsetX + v.x / 2 * 1;// scale;
            zCoord = offsetY + v.z / 2 * 1;// scale;
            v.y += Remap(Mathf.PerlinNoise(xCoord, zCoord), 0, 1, 0, triangulationHeight) - triangulationHeight / 2;

            //v.y += waveHeight3 / 5 * Mathf.Sin(Time.time * Mathf.PI * 2.0f * 0.1f);

            verts[i] = v;
        }
        mesh.vertices = verts;
        mesh.RecalculateNormals();
        mesh.MarkDynamic();

        GetComponent<MeshFilter>().mesh = mesh;
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}