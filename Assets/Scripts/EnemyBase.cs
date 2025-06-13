using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Normie enemyPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private ParticleSystem particleDeath;
    [SerializeField] private AudioSource death;

    [Space]
    [SerializeField] private int maxHp;
    [SerializeField] private float waveInterval;
    [SerializeField] private Wave[] waves;

    private int currentWaveIndex;
    private Wave currentWave => waves[currentWaveIndex];

    private int _remainingHp;
    private float _waveCooldown;
    private bool _dead;


    private void Awake()
    {
        _remainingHp = maxHp;
        text.SetText(maxHp.ToString());
        _waveCooldown = waveInterval;
    }

    private void Update()
    {
        _waveCooldown -= Time.deltaTime;
        if (_waveCooldown <= 0)
        {
            SpawnWave();
            _waveCooldown = waveInterval;
        }
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

        TakeDamage();
        normie.Kill();
    }

    public void TakeDamage()
    {
        if (_dead)
        {
            return;
        }

        --_remainingHp;
        if (_remainingHp <= 0)
        {
            Death();
            return;
        }

        text.SetText(_remainingHp.ToString());
        animator.SetTrigger("Hit");
        Instantiate(particle, transform.position + Vector3.up * 2 + Vector3.back * 1.5f, quaternion.identity);
    }

    private void Death()
    {
        text.gameObject.SetActive(false);
        animator.SetTrigger("Death");
        _dead = true;
        Instantiate(particleDeath, transform.position + Vector3.up * 2 + Vector3.back * 1.5f, quaternion.identity);
        Instantiate(death);
        StartCoroutine(DisableTower());
    }

    private IEnumerator DisableTower()
    {
        yield return new WaitForSeconds(0.533f);
        gameObject.SetActive(false);
    }

    private void SpawnWave()
    {
        for (int i = 0; i < currentWave.enemies; i++)
        {
            SpawnNormie();
        }
        ++currentWaveIndex;
    }

    private void SpawnNormie()
    {
        Vector3 basePosition = transform.position;
        Vector3 spawnPosition = basePosition;
        spawnPosition.z = Random.Range(basePosition.z - 3.5f, basePosition.z - 2);
        spawnPosition.x = Random.Range(basePosition.x - 1f, basePosition.x + 1f);
        Quaternion rotation = transform.rotation;
        var eulerRotation = rotation.eulerAngles;
        //eulerRotation.y += 180;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.Euler(eulerRotation));
    }
}
