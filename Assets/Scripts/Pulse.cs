using DG.Tweening;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    [SerializeField] private Vector3 newValue;

    private void Awake()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(newValue, 1.2f)).SetEase(Ease.InOutSine);
        sequence.SetLoops(-1, LoopType.Yoyo);
        sequence.Play();
    }
}