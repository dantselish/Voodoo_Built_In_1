using System;
using DG.Tweening;
using UnityEngine;

public class PipeStart : MonoBehaviour
{
    [SerializeField] private PipeEnd pipeEnd;
    [SerializeField] private AudioSource sound;
    [SerializeField] private Transform pivot;

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

        if (normie.Killed)
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
            pivot.localScale = Vector3.one;
        }

        sequence = DOTween.Sequence();
        sequence.SetLoops(2, LoopType.Yoyo);
        sequence.Append(pivot.DOScale(1.25f, 0.1f));
        sequence.Play();
    }
}
