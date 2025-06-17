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

    [Space]
    [SerializeField] private int maxHp;

    private int _hp;

    private bool _built;


    private void Awake()
    {
        _hp = maxHp;
        text.SetText(maxHp.ToString());
    }

    private void OnCollisionEnter(Collision other)
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

        Instantiate(particle, normie.transform.position + Vector3.up * 0.5f, Quaternion.identity);
        normie.Kill();
        --_hp;

        if (_hp <= 0)
        {
            BuildBridge();
        }

        text.SetText(_hp.ToString());
        Instantiate(sound);
    }

    private void BuildBridge()
    {
        _built = true;

        FindAnyObjectByType<Bridge>().Stop();
        particle1.Play();
        particle2.Play();
        particle3.Play();
        gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
