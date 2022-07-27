using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private SerializableDictionary<string, AudioClip> _clips = new();

    private void Awake()
    {
        Instance = this;
    }

    public void PlayOneShot(string name, float volumeScale = 1)
        => source.PlayOneShot(_clips[name], volumeScale);

    public void PlayOneShot(AudioClip clip, float volumeScale = 1)
        => source.PlayOneShot(clip, volumeScale);
}
