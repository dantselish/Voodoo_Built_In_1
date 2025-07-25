﻿using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Normie playerPrefab;
    [SerializeField] private Normie enemyPrefab;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem plusParticle;
    [SerializeField] private AudioSource sound;

    public float multiplier;

    private List<Normie> _triggered = new List<Normie>();
    private Sequence sequence;
    private Vector3 startScale;
    private SoundManager _soundManager;


    private void Awake()
    {
        _soundManager = FindAnyObjectByType<SoundManager>();
        startScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        Normie normie = other.gameObject.GetComponent<Normie>();

        if (!normie)
        {
            return;
        }

        if (normie.NormieType == NormieType.Enemy)
        {
            return;
        }

        if (_triggered.Contains(normie))
        {
            return;
        }

        _triggered.Add(normie);

        for (int i = 0; i < multiplier - 1; i++)
        {
            Vector3 positionModifier = Vector3.right;
            positionModifier *= i / 2 * 0.5f + 0.5f;
            if (i % 2 == 1)
            {
                positionModifier *= -1;
            }
            SpawnNormie(normie.NormieType, normie.transform, positionModifier);
        }
    }

    private void SpawnNormie(NormieType type, Transform originTransform, Vector3 positionModifier)
    {
        Vector3 spawnPosition = originTransform.position + positionModifier;
        spawnPosition += originTransform.forward * 0.1f;
        Normie newNormie = Instantiate(type == NormieType.Player ? playerPrefab : enemyPrefab, spawnPosition, originTransform.rotation);
        _triggered.Add(newNormie);
        HandleTween();

        if (particle != null)
        {
            Instantiate(particle, spawnPosition + Vector3.up * 0.5f + Vector3.forward * 1f, Quaternion.identity);
        }

        if (plusParticle)
        {
            Instantiate(plusParticle, spawnPosition + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

    private void HandleTween()
    {
        if (sequence.IsActive())
        {
            sequence.Kill();
            transform.localScale = startScale;
        }

        sequence = DOTween.Sequence();
        sequence.SetLoops(2, LoopType.Yoyo);
        sequence.Append(transform.DOScale(Vector3.one * 3.1f, 0.2f));
        sequence.Play();
        _soundManager.PlayMultiply();
    }
}

[System.Serializable]
public class Wave
{
    public int enemies;
}