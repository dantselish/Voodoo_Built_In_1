using System.Collections;
using UnityEngine;

public class PipeEnd : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;


    public void AddToEnd(Normie normie)
    {
        StartCoroutine(StartRoutine(normie));
    }

    private IEnumerator StartRoutine(Normie normie)
    {
        yield return new WaitForSeconds(3f);
        normie.transform.position = spawnPosition.position;
        normie.transform.forward = spawnPosition.forward;
        normie.gameObject.SetActive(true);
    }
}
