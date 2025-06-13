using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Normie playerNormiePrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource deathSound;

    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float coordinateBorder;
    [SerializeField] private float shootInterval;

    private float _horizontalInput;

    private float _shootCooldownTimer;
    private bool _killed;


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

        _horizontalInput = Input.GetAxis("Horizontal");
    }

    private void ShootNormie()
    {
        Vector3 spawnPos = transform.position + transform.forward.normalized;
        Normie normie = Instantiate(playerNormiePrefab, spawnPos, Quaternion.identity);
        normie.PushStart();
        animator.SetTrigger("Shoot");
        animator.SetBool("Shooting", true);
        Instantiate(shootSound);
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
}
