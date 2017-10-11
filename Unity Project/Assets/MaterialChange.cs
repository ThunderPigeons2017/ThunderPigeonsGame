using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour {

    public MeshRenderer meshRenderer;

    public Color p1Color;
    public Color p2Color;

    public bool isP1 = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //in switch color function
        meshRenderer.material.color = isP1 ? p1Color : p2Color;
	}
}
