using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] private List<CirclePart> circleParts;

    [Space]
    [SerializeField] private float rotationSpeed;


    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        CirclePart currentCirclePart = circleParts[0];

        for (int i = 1; i < circleParts.Count; i++)
        {
            if (circleParts[i].pos.position.z > currentCirclePart.pos.position.z)
            {
                currentCirclePart = circleParts[i];
            }
        }

        foreach (CirclePart circlePart in circleParts)
        {
            if (circlePart == currentCirclePart)
            {
                circlePart.Active();
            }
            else
            {
                circlePart.Passive();
            }
        }
    }
}
