using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GateDeleting : MonoBehaviour
{
    [SerializeField] private int divider;
    [SerializeField] private CanvasGroup redCanvasGroup;
    [SerializeField] private ParticleSystem particleSystem;

    private int _dividerCounter;
    private List<Normie> _triggered = new List<Normie>();
    private Sequence sequence;
    private Sequence canvasSequence;
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
            Instantiate(particleSystem, normie.transform.position + Vector3.forward, Quaternion.identity);
        }
    }

    private void HandleTween()
    {
        // if (sequence.IsActive())
        // {
        //     sequence.Kill();
        //     transform.localScale = startScale;
        // }
        //
        // sequence = DOTween.Sequence();
        // sequence.SetLoops(2, LoopType.Yoyo);
        // sequence.Append(transform.DOScale(Vector3.one * 3.1f, 0.2f));
        //sequence.Play();

        if (canvasSequence.IsActive())
        {
            canvasSequence.Kill();
            redCanvasGroup.alpha = 0;
        }

        canvasSequence = DOTween.Sequence();
        canvasSequence.Append(redCanvasGroup.DOFade(0.7f, 0.2f));
        canvasSequence.SetLoops(2, LoopType.Yoyo);
        canvasSequence.Play();
    }
}
