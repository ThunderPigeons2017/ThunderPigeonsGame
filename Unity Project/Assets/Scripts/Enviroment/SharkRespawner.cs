using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkRespawner : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shark")
        {
            other.GetComponent<Wander>().Respawn();
        }
    }
}
