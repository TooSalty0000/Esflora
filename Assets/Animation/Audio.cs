using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{
    public string name;
    [HideInInspector]
    public AudioSource source;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 0.5f;
}
