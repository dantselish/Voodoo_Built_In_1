using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Normie playerNormiePrefab;
    [SerializeField] private Animator animator;

    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float coordinateBorder;
    [SerializeField] private float shootInterval;

    private float _horizontalInput;

    private float _shootCooldownTimer;


    private void Update()
    {
        _shootCooldownTimer -= Time.deltaTime;
        ManageInput();
        Move();
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
        _shootCooldownTimer = shootInterval;
    }
}
