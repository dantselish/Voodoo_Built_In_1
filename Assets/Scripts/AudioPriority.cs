using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPriority : MonoBehaviour
{
    [SerializeField] private int priority;

    private AudioSource _audioSource;

    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.priority = priority;
    }
}
