using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private float coordinateBorder;
    [SerializeField] private float speed;

    private float _movementMultiplyer = 1;

    public Vector3 movementVector;


    private void Update()
    {
        movementVector = Vector3.right * speed * _movementMultiplyer * Time.deltaTime;
        transform.Translate(movementVector, Space.World);

        if (transform.position.x > coordinateBorder)
        {
            Vector3 position = transform.position;
            position.x = coordinateBorder;
            transform.position = position;
            _movementMultiplyer *= -1;
        }
        else if (transform.position.x < -coordinateBorder)
        {
            Vector3 position = transform.position;
            position.x = -coordinateBorder;
            transform.position = position;
            _movementMultiplyer *= -1;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Stop();
        }
    }

    public void Stop()
    {
        _movementMultiplyer = 0;
        FindAnyObjectByType<Cannon>().TravelStart();
        foreach (Normie normie in FindObjectsByType<Normie>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            normie.Kill();
        }
    }
}