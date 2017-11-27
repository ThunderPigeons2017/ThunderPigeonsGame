using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticleOnFinish : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }            
}
