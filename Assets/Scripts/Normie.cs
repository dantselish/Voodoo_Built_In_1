using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Normie : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float startSpeedSeconds;
    [SerializeField] private float startSpeedMultiplier;

    [Space]
    [SerializeField] private bool canDie = true;

    [Space]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    private float basicSpeed;
    private Vector3 translateVector = Vector3.zero;

    public NormieType NormieType;
    public bool Launched { get; private set; }
    public bool Killed { get; private set; }

    public bool bridgeMode = true;


    private void Awake()
    {
        basicSpeed = speed;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector3.zero;
        translateVector = Vector3.forward * speed * Time.fixedDeltaTime;
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (bridgeMode)
        {
            Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
            if (!Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Bridge")))
            {
                transform.Translate(Vector3.down * 9.8f * Time.deltaTime);
            }
            else
            {
                Bridge bridge = hit.transform.gameObject.GetComponentInParent<Bridge>();
                if (bridge)
                {
                    transform.Translate(bridge.movementVector, Space.World);
                }
            }
        }
    }

    private void OnDisable()
    {
        speed = basicSpeed;
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
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

        if (otherNormie.NormieType == NormieType.Enemy && !Killed && !otherNormie.Killed)
        {
            Debug.Log("here");
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
        if (!canDie)
        {
            return;
        }

        Killed = true;
        if (gameObject)
        {
            StartCoroutine(KillRoutine());
        }
    }

    private IEnumerator KillRoutine()
    {
        animator.enabled = false;
        GetComponentInChildren<SkinnedMeshRenderer>().material.DOColor(Color.gray, 0.1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(0.00001f, 0.1f);
        yield return new WaitForSeconds(0.1f);
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