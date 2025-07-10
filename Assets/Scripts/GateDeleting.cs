using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GateDeleting : MonoBehaviour
{
    [SerializeField] private int divider;

    private int _dividerCounter;
    private List<Normie> _triggered = new List<Normie>();
    private Sequence sequence;
    private Vector3 startScale;


    private void Awake()
    {
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
        if (++_dividerCounter >= divider)
        {
            normie.Kill();
            HandleTween();
            _dividerCounter = 0;
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
    }
}
