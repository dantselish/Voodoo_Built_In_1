using System;
using DG.Tweening;
using UnityEngine;

public class PipeStart : MonoBehaviour
{
    [SerializeField] private PipeEnd pipeEnd;
    [SerializeField] private AudioSource sound;

    private Sequence sequence;
    private Vector3 startScale;


    private void Awake()
    {
        startScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        Normie normie = other.GetComponent<Normie>();

        if (!normie)
        {
            return;
        }

        if (normie.NormieType == NormieType.Enemy)
        {
            return;
        }

        normie.gameObject.SetActive(false);
        pipeEnd.AddToEnd(normie);
        Instantiate(sound);
        HandleTween();
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
        sequence.Append(transform.DOScale(Vector3.one * 165f, 0.2f));
        sequence.Play();
    }
}
