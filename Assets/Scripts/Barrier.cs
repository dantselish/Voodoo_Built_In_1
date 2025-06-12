using System;
using TMPro;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private ParticleSystem particle;

    [Space]
    [SerializeField] private int maxHp;

    private int _hp;


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
        text.SetText(_hp.ToString());
    }
}
