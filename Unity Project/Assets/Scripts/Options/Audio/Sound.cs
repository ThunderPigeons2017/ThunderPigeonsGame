using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f,100f)]
    public float volume;

    [Range(0f,3f)]
    public float pitch;

    [Range(256, 0)]
    public int priority;

    [Range(-1f, 1f)]
    public float StereoPan;

    [Range(0f, 1f)]
    public float SpatialBlend;

    [Range(0f, 1.1f)]
    public float ReverbZoneMix;

    public bool Loop;

    [Range(0f, 5f)]
    public float DopplerLevel;

    [Range(0, 360)]
    public int Spread;

    public int minDistance;

    public int maxDistance;

    public AudioMixerGroup output;
    
    [HideInInspector]
    public AudioSource source;
}