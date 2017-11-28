using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneParticles : MonoBehaviour
{

    ZoneControl zoneControl;

    ParticleSystem primaryParticle;
    ParticleSystem secondaryParticle;
    ParticleSystem splashParticle;
    Renderer primaryRenderer;
    Renderer secondaryRenderer;
    Renderer splashRenderer;

    [SerializeField]
    Color defaultColour = Color.cyan;

    void Awake ()
    {
        zoneControl = transform.parent.GetComponent<ZoneControl>();
        // Find the Particle systems
        primaryParticle = GetComponent<ParticleSystem>();
        secondaryParticle = primaryParticle.transform.GetChild(0).GetComponent<ParticleSystem>();
        splashParticle = secondaryParticle.transform.GetChild(0).GetComponent<ParticleSystem>();
        // Find the renderers
        primaryRenderer = GetComponent<Renderer>();
        secondaryRenderer = primaryParticle.transform.GetChild(0).GetComponent<Renderer>();
        splashRenderer = secondaryParticle.transform.GetChild(0).GetComponent<Renderer>();
    }
	
	void Update ()
    {
        // No one is in the zone
        if (zoneControl.playersInZone.Count == 0)
        {
            // Play the particles
            primaryParticle.Play();
            secondaryParticle.Play();
            splashParticle.Play();

            // Set default colour
            primaryRenderer.sharedMaterial.SetColor("_TintColor", defaultColour);
            secondaryRenderer.sharedMaterial.SetColor("_TintColor", defaultColour);
            splashRenderer.sharedMaterial.SetColor("_EmissionColor", defaultColour);
        }
        // One person is in the zone
        else if (zoneControl.playersInZone.Count == 1)
        {

            // Play the particles
            primaryParticle.Play();
            secondaryParticle.Play();
            splashParticle.Play();

            // Set colour to the only player in the zones colour
            ColourSetter playerColorSetter = zoneControl.playersInZone[0].transform.parent.GetComponentInChildren<ColourSetter>();
            primaryRenderer.sharedMaterial.SetColor("_TintColor", playerColorSetter.primaryColour);
            secondaryRenderer.sharedMaterial.SetColor("_TintColor", playerColorSetter.secondaryColour);
            splashRenderer.sharedMaterial.SetColor("_EmissionColor", playerColorSetter.primaryColour);
        }
        else
        {
            // Set default colour
            primaryRenderer.sharedMaterial.SetColor("_TintColor", defaultColour);
            secondaryRenderer.sharedMaterial.SetColor("_TintColor", defaultColour);
            splashRenderer.sharedMaterial.SetColor("_EmissionColor", defaultColour);
            // Pause the animation
            primaryParticle.Pause();
            secondaryParticle.Pause();
            splashParticle.Pause();
        }

	}
}
