using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioSource sound;

    [Space]
    [SerializeField] private Vector3 newScale;
    [SerializeField] private Color color;
    [SerializeField] private MeshRenderer meshRenderer;

    [Space]
    [SerializeField] private int maxHp;

    private Sequence sequence;
    private Color startColor;
    private Vector3 startScale;
    private SoundManager _soundManager;

    private int _hp;


    private void Awake()
    {
        _soundManager = FindAnyObjectByType<SoundManager>();
        _hp = maxHp;
        text.SetText(maxHp.ToString());

        startColor = meshRenderer.material.color;
        startScale = transform.localScale;
    }

    private void OnCollisionEnter(Collision other)
    {
        Normie normie = other.gameObject.GetComponent<Normie>();

        if (!normie)
        {
            return;
        }

        if (normie.NormieType == NormieType.Enemy || normie.Killed)
        {
            return;
        }

        Instantiate(particle, normie.transform.position + Vector3.forward * 1 + Vector3.up * 0.5f, Quaternion.identity);
        normie.Kill();
        --_hp;
        text.SetText(_hp.ToString());
        _soundManager.PlayBarrier();
        HandleSequence();
    }

    private void HandleSequence()
    {
        if (sequence.IsActive())
        {
            transform.localScale = startScale;
            meshRenderer.material.color = startColor;
            sequence.Kill();
        }

        sequence = DOTween.Sequence();
        sequence.Append(meshRenderer.material.DOColor(color, 0.1f));
        sequence.Join(transform.DOScale(newScale, 0.1f));
        sequence.SetLoops(2, LoopType.Yoyo);
    }
}
