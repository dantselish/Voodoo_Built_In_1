using System;
using System.Collections;
using UnityEngine;

public class Normie : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float startSpeedSeconds;
    [SerializeField] private float startSpeedMultiplier;

    [Space]
    [SerializeField] private Rigidbody rb;

    private float basicSpeed;
    private Vector3 translateVector = Vector3.zero;

    public NormieType NormieType;
    public bool Launched { get; private set; }


    private void Awake()
    {
        basicSpeed = speed;
    }

    private void Update()
    {
        rb.linearVelocity = Vector3.zero;
        translateVector = Vector3.forward * speed * Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnDisable()
    {
        speed = basicSpeed;
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (NormieType == NormieType.Enemy)
        {
            return;
        }

        Normie otherNormie = other.gameObject.GetComponent<Normie>();
        if (!otherNormie)
        {
            return;
        }

        if (otherNormie.NormieType == NormieType.Enemy)
        {
            Kill();
            otherNormie.Kill();
        }
    }

    public void PushStart()
    {
        StartCoroutine(StartSpeedCoroutine());
        Launched = true;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private IEnumerator StartSpeedCoroutine()
    {
        if (NormieType == NormieType.Player)
        {
            speed *= startSpeedMultiplier;
        }

        yield return new WaitForSeconds(startSpeedSeconds);
        speed = basicSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, translateVector);
    }
}


public enum NormieType
{
    Player,
    Enemy
}