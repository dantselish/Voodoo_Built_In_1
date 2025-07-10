using DG.Tweening;
using UnityEngine;

public class DummyMoverRootMotion : MonoBehaviour
{
    [SerializeField] private Transform endPos;

    void Start()
    {
        transform.DOMoveZ(endPos.position.z, 0.6f, true).SetEase(Ease.OutSine);
        Invoke(nameof(OnComplete), 0.15f);
    }

    private void OnComplete()
    {
        FindAnyObjectByType<CameraController>().StartCamera();
    }
}
