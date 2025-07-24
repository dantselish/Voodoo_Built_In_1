using System;
using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Normie playerNormiePrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private ParticleSystem shotParticle;
    [SerializeField] private Transform shotParticleTransformPoint;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private Circle circle;
    [SerializeField] private Transform levelZPosTransform;

    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float travelSpeed;
    [SerializeField] private float coordinateBorder;
    [SerializeField] private float shootInterval;

    [Space]
    [SerializeField] private bool startWithBridge;

    private float _horizontalInput;

    private float _shootCooldownTimer;
    private bool _killed;

    private IEnumerator _travelRoutine;


    private void Start()
    {
        if (!startWithBridge)
        {
            FindAnyObjectByType<EnemyBase>().WokeUp = true;
            transform.position = levelZPosTransform.position;
            FindAnyObjectByType<CameraController>().StartCamera();
            StartCoroutine(ChangeToFinishCameraAfter(4));
        }
        animator.Play("Start");
        Invoke(nameof(EnableCircle), 1.267f);
    }

    private void Update()
    {
        _shootCooldownTimer -= Time.deltaTime;
        ManageInput();
        Move();
    }

    public void Kill()
    {
        if (_killed)
        {
            return;
        }

        _killed = true;
        StartCoroutine(KillRoutine());
    }

    public void TravelStart()
    {
        animator.SetTrigger("TravelBegin");
        _travelRoutine = TravelRoutine();
        StartCoroutine(_travelRoutine);
    }

    public void EnableCircle()
    {
        circle.gameObject.SetActive(true);
        circle.transform.localScale = Vector3.zero;
        circle.transform.DOScale(Vector3.one * 0.89f, 0.2f).SetEase(Ease.OutBack);
    }

    private IEnumerator TravelRoutine()
    {
        FindAnyObjectByType<CameraController>().Transition();
        yield return new WaitForSeconds(0.733f);
        while (transform.position.z < -10.65f)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * travelSpeed, Space.World);
            yield return null;
        }
        animator.SetTrigger("TravelEnd");
        FindAnyObjectByType<CameraController>().Finish();
        FindAnyObjectByType<EnemyBase>().WokeUp = true;
        _travelRoutine = null;
    }

    private IEnumerator KillRoutine()
    {
        Instantiate(deathParticle, transform.position, quaternion.identity);
        Instantiate(deathSound);
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    private void Move()
    {
        Vector3 translationVector = Vector3.right * _horizontalInput * speed * Time.deltaTime;

        if (transform.position.x + translationVector.x < -coordinateBorder)
        {
            translationVector.x = -coordinateBorder - transform.position.x;
        }

        if (transform.position.x + translationVector.x > coordinateBorder)
        {
            translationVector.x = coordinateBorder - transform.position.x;
        }

        transform.Translate(translationVector);
        int intInput = 0;
        if (_horizontalInput < -0.1f)
        {
            intInput = -1;
        }
        else if (_horizontalInput > 0.1f)
        {
            intInput = 1;
        }
        //animator.SetInteger("Horizontal", intInput);
    }

    private void ManageInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        if (_travelRoutine != null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space) && _shootCooldownTimer <= 0)
        {
            ShootNormie();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Shooting", true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("Shooting", false);
        }
    }

    private void ShootNormie()
    {
        Vector3 spawnPos = transform.position + transform.forward.normalized;
        Normie normie = Instantiate(playerNormiePrefab, spawnPos + Vector3.left * 0.3f, Quaternion.identity);
        normie.PushStart();
        normie =Instantiate(playerNormiePrefab, spawnPos + Vector3.right * 0.3f, Quaternion.identity);
        normie.PushStart();
        animator.SetTrigger("Shoot");
        animator.SetBool("Shooting", true);
        Instantiate(shootSound);
        Instantiate(shotParticle, shotParticleTransformPoint.position, quaternion.identity);
        _shootCooldownTimer = shootInterval;
    }

    private void SpawnNormie(Transform originTransform, Vector3 positionModifier)
    {
        Debug.Log("Spawn Normie");
        Vector3 spawnPosition = originTransform.position + positionModifier;
        spawnPosition += originTransform.forward * 0.1f;
        Normie normie = Instantiate(playerNormiePrefab, spawnPosition, originTransform.rotation);
        normie.PushStart();
        animator.SetTrigger("Shoot");
        animator.SetBool("Shooting", true);
        Instantiate(shootSound);
        _shootCooldownTimer = shootInterval;
    }

    private IEnumerator ChangeToFinishCameraAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        FindAnyObjectByType<CameraController>().Finish();
    }
}
