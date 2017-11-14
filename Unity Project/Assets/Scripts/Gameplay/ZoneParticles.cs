using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneParticles : MonoBehaviour
{

    ZoneControl zoneControl;

    ParticleSystem zoneParticle;
    Renderer particleRenderer;

    [SerializeField]
    Color defaultColour = Color.cyan;

    void Awake ()
    {
        zoneControl = transform.parent.GetComponent<ZoneControl>();
        zoneParticle = GetComponent<ParticleSystem>();
        particleRenderer = GetComponent<Renderer>();
    }
	
	void Update ()
    {
        // No one is in the zone
        if (zoneControl.playersInZone.Count == 0)
        {
            zoneParticle.Play();

            // Set default colour
            particleRenderer.material.SetColor("_TintColor", defaultColour);
        }
        // One person is in the zone
        else if (zoneControl.playersInZone.Count == 1)
        {
            zoneParticle.Play();

            // Set colour to the only player in the zones colour
            Color playerColor = zoneControl.playersInZone[0].transform.parent.GetComponentInChildren<ColourSetter>().primaryColour;
            particleRenderer.material.SetColor("_TintColor", playerColor);
        }
        else
        {
            // Set to default colour
            particleRenderer.material.SetColor("_TintColor", defaultColour);
            // Pause the animation
            zoneParticle.Pause();
        }

	}
}
