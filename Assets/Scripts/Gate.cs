using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Normie playerPrefab;
    [SerializeField] private Normie enemyPrefab;

    public int multiplier;

    private List<Normie> _triggered = new List<Normie>();


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

        for (int i = 0; i < multiplier - 1; i++)
        {
            Vector3 positionModifier = Vector3.right;
            positionModifier *= i / 2 * 0.5f + 0.5f;
            if (i % 2 == 1)
            {
                positionModifier *= -1;
            }
            SpawnNormie(normie.NormieType, normie.transform, positionModifier);
        }
    }

    private void SpawnNormie(NormieType type, Transform originTransform, Vector3 positionModifier)
    {
        Debug.Log("Spawn Normie");
        Vector3 spawnPosition = originTransform.position + positionModifier;
        spawnPosition += originTransform.forward * 0.1f;
        Normie newNormie = Instantiate(type == NormieType.Player ? playerPrefab : enemyPrefab, spawnPosition, originTransform.rotation);
        _triggered.Add(newNormie);
    }
}
