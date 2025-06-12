using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Normie : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float startSpeedSeconds;
    [SerializeField] private float startSpeedMultiplier;

    [Space]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

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
        StartCoroutine(KillRoutine());
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