using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _multiplyAudioSources;
    [SerializeField] private List<AudioSource> _barrierAudioSources;
    [SerializeField] private List<AudioSource> _pipeAudioSources;
    [SerializeField] private List<AudioSource> _walkingAudioSources;

    private int _walkingPlays;

    public void PlayMultiply()
    {
        foreach (AudioSource multiplyAudioSource in _multiplyAudioSources)
        {
            if (!multiplyAudioSource.isPlaying)
            {
                multiplyAudioSource.Play();
                return;
            }
        }
    }

    public void PlayBarrier()
    {
        foreach (AudioSource barrierAudioSource in _barrierAudioSources)
        {
            if (!barrierAudioSource.isPlaying)
            {
                barrierAudioSource.Play();
                return;
            }
        }
    }

    public void PlayWalking()
    {
        ++_walkingPlays;
        foreach (AudioSource walkingAudioSource in _walkingAudioSources)
        {
            if (!walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Play();
                break;
            }
        }
    }

    public void StopWalking()
    {
        --_walkingPlays;
        if (_walkingPlays < 3)
        {
            foreach (AudioSource walkingAudioSource in _walkingAudioSources)
            {
                if (walkingAudioSource.isPlaying)
                {
                    walkingAudioSource.Stop();
                }
            }
        }
    }

    public void PlayPipe()
    {
        foreach (AudioSource pipeAudioSource in _pipeAudioSources)
        {
            if (!pipeAudioSource.isPlaying)
            {
                pipeAudioSource.Play();
                return;
            }
        }
    }
}
