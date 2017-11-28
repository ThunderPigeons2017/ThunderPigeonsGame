/***********************************************
 * Rummy Robots
 * by Thunder Pidgeons
 * 
 * Water Environment Script
 ***********************************************/
//Codes to include basic Unity Functions

using UnityEngine;
using System.Collections;

/// <summary>
/// Water Class Begins
/// </summary>
public class Water : MonoBehaviour                                                                                                      
{                                                                                                                                       
    [SerializeField]                                                                                                                    //public variables that can be set from Inspector for producers 
    [Tooltip("The speed that the waves move")]                                                                                          //Tooltip for waveSpeed
    float waveSpeed = 0.5f;                                                                                                             //waveSpeed variable
    [SerializeField]                                                                                                                    
    [Tooltip("The +/- y height of the main waves")]                                                                                     //Tooltip for waveHeight
    float waveHeight = 0.48f;                                                                                                           //Varaible for waveHeight
    [SerializeField]                                                                                                                    
    [Tooltip("The +/- y height of the triangulation (kinda like choppyness at sea)")]                                                   //Tooltip for triangulationHeight
    float triangulationHeight = 0.1f;                                                                                                   //variable for triangulationHeight
                                                                                                                                        
    Mesh mesh;                                                                                                                          //Mesh Varaible called mesh
    Vector3[] verts;                                                                                                                    //Mesh Vector varaible called verts
                                                                                                                                        
    float offsetX;                                                                                                                      //offsetX
    float offsetY;                                                                                                                      //offsetY
    
    /// <summary>
    /// On Boot of Game
    /// </summary>
    void Awake()                                                                                                                        
    {                                                                                                                                   
        MeshFilter mf = GetComponent<MeshFilter>();                                                                                     //variable mf gets data from MeshFilter
        MakeMeshLowPoly(mf);                                                                                                            //calls function MakemeshLowPoly passing in mf
    }                                                                                                                                   
    
    /// <summary>
    /// On Start of Program
    /// </summary>
    void Start()                                                                                                                        
    {                                                                                                                                   
        string seed = Time.time.ToString();                                                                                             //Time is set as variable seed
                                                                                                                                        
        offsetX = seed.GetHashCode() % 10000;                                                                                           //offsetX is set as seed percentage
        offsetY = offsetX;                                                                                                              //offsetY set as offsetX
    }                                                                                                                                   

    /// <summary>
    /// Makes Mesh passed in parameter as low poly
    /// </summary>
    /// <param name="mf">Meshfilter passed in as parameter</param>
    /// <returns></returns>
    MeshFilter MakeMeshLowPoly(MeshFilter mf)                                                                                           
    {                                                                                                                                   
                                                                                                                                        
        mesh = mf.sharedMesh;                                                                                                           //We want each tri to have its own unshared verts
        Vector3[] oldVerts = mesh.vertices;                                                                                             //triangle Vectors from mesh is stored in oldVerts
        int[] triangles = mesh.triangles;                                                                                               //mesh.triangle values is stored in triangles int variable
        Vector3[] vertices = new Vector3[triangles.Length];                                                                             //triangles length vector is placed in vertices array

        for (int i = 0; i < triangles.Length; i++)                                                                                      //loops through all triangles
        {                                                                                                                               
            vertices[i] = oldVerts[triangles[i]];                                                                                       //sets vertices as oldVerts Triangles
            triangles[i] = i;                                                                                                           //triangles is set as counter
        }                                                                                                                               

        mesh.vertices = vertices;                                                                                                       //mesh vertices is set as new vertices
        mesh.triangles = triangles;                                                                                                     //mesh triangles is set as new triangles
        mesh.RecalculateBounds();                                                                                                       //mesh Recalculate Bounds
        mesh.RecalculateNormals();                                                                                                      //mesh recalculates normals
        verts = mesh.vertices;                                                                                                          //verts is set as new mesh vertices
        return mf;                                                                                                                      //returns the calculated mesh
    }                                                                                                                                   

    /// <summary>
    /// Every time Game updates
    /// </summary>
    void Update()                                                                                                                       
    {                                                                                                                                   
        CalcWave();                                                                                                                     //Calls CalcWave function
        offsetX += Time.deltaTime * waveSpeed;                                                                                          //offsetX is computed according to deltatime and wavespeed
    }                                                                                                                                   

    /// <summary>
    /// Calcwave function that calculates the wave
    /// </summary>
    void CalcWave()                                                                                                                     
    {                                                                                                                                   
                                                                                                                                        
        for (int i = 0; i < verts.Length; i++)                                                                                          // Loop through every vert and change its y
        {                                                                                                                               
            Vector3 v = verts[i];                                                                                                       
            v.y = 0;                                                                                                                    // Reset the y so we can add the new values
                                                                                                                                        
            Vector3 worldPos = v + transform.position;                                                                                  // Add position to make it relatice to world space
            
            float xCoord = offsetX + worldPos.x / 100;                                                                                  // Perlin noise wave 1 (bigger underlying wave)
            float zCoord = offsetY + worldPos.z / 100;                                                                                  //
            v.y += Helper.Remap(Mathf.PerlinNoise(xCoord, zCoord), 0, 1, 0, waveHeight) - waveHeight / 2;                               
                                                         
            xCoord = offsetX + worldPos.x / 2;                                                                                          // Perlin noise wave 2 (small more frequent waves to add choppyness to the top)
            zCoord = offsetY + worldPos.z / 2;                                                                                          
            v.y += Helper.Remap(Mathf.PerlinNoise(xCoord, zCoord), 0, 1, 0, triangulationHeight) - triangulationHeight / 2;             
                                                                                                                                        
            verts[i] = v;                                                                                                               
        }                                                                                                                               
        mesh.vertices = verts;                                                                                                          
        mesh.RecalculateNormals();                                                                                                      
        mesh.MarkDynamic();                                                                                                             
                                                                                                                                        
        GetComponent<MeshFilter>().mesh = mesh;                                                                                         
    }                                                                                                                                   
}                                                                                                                                       