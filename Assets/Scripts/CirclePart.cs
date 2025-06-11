using System.Collections.Generic;
using UnityEngine;

public class CirclePart : MonoBehaviour
{
    [SerializeField] private GameObject active;
    [SerializeField] private GameObject passive;
    [SerializeField] private Normie playerPrefab;

    [Space]
    [SerializeField] private int addValue;
    [SerializeField] private float multValue;
    [SerializeField] private bool isMult;

    public Transform pos;

    private List<Normie> _triggered = new List<Normie>();


    public void Active()
    {
        active.SetActive(true);
        passive.SetActive(false);
    }

    public void Passive()
    {
        active.SetActive(false);
        passive.SetActive(true);
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

        if (!normie.Launched)
        {
            return;
        }

        if (_triggered.Contains(normie))
        {
            return;
        }

        _triggered.Add(normie);

        if (isMult)
        {
            for (int i = 0; i < multValue - 1; i++)
            {
                Vector3 positionModifier = Vector3.right;
                positionModifier *= i / 2 * 0.5f + 0.5f;
                if (i % 2 == 1)
                {
                    positionModifier *= -1;
                }
                SpawnNormie(normie.transform, i);
            }
        }
        else
        {
            for (int i = 0; i < addValue; i++)
            {
                Vector3 positionModifier = Vector3.right;
                positionModifier *= i / 2 * 0.5f + 0.5f;
                if (i % 2 == 1)
                {
                    positionModifier *= -1;
                }
                SpawnNormie(normie.transform, i);
            }
        }
    }

    private void SpawnNormie(Transform originTransform, int index)
    {
        Vector3 modifier = Vector3.zero;
        modifier.x = Random.Range(-0.5f * index, 0.5f * index);
        modifier.z = Random.Range(0, index * 0.5f);

        // Vector3 spawnPosition = originTransform.position + positionModifier;
        // spawnPosition += originTransform.forward * 0.1f;

        Vector3 spawnPosition = originTransform.position + modifier;

        Normie newNormie = Instantiate(playerPrefab, spawnPosition, originTransform.rotation);
        _triggered.Add(newNormie);
    }
}
