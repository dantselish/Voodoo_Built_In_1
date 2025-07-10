using DG.Tweening;
using TMPro;
using UnityEngine;

public class BridgeBarrier : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioSource sound;

    [Space]
    [SerializeField] private ParticleSystem particle1;
    [SerializeField] private ParticleSystem particle2;
    [SerializeField] private ParticleSystem particle3;
    [SerializeField] private ParticleSystem particle4;

    [Space]
    [SerializeField] private Vector3 newScale;
    [SerializeField] private Color color;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Animator bridgeAnimator;

    [Space]
    [SerializeField] private int maxHp;

    private int _hp;

    private bool _built;

    private Sequence sequence;
    private Color startColor;
    private Vector3 startScale;


    private void Awake()
    {
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

        Instantiate(particle, normie.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        normie.Kill();
        --_hp;

        if (_hp <= 0)
        {
            BuildBridge();
        }

        text.SetText(_hp.ToString());
        Instantiate(sound);
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

    private void BuildBridge()
    {
        _built = true;

        FindAnyObjectByType<Bridge>().Stop();
        particle1.Play();
        particle2.Play();
        particle3.Play();
        particle4.Play();
        bridgeAnimator.SetTrigger("glow");
        gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
