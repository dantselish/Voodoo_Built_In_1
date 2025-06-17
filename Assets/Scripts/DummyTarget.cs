using System;
using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    [SerializeField] private Transform cannon;

    private void LateUpdate()
    {
        Vector3 cannonPosition = cannon.position;
        cannonPosition.x = 0f;
        transform.position = cannonPosition;
    }
}
