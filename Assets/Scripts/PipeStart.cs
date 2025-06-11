using UnityEngine;

public class PipeStart : MonoBehaviour
{
    [SerializeField] private PipeEnd pipeEnd;


    private void OnTriggerEnter(Collider other)
    {
        Normie normie = other.GetComponent<Normie>();

        if (!normie)
        {
            return;
        }

        if (normie.NormieType == NormieType.Enemy)
        {
            return;
        }

        normie.gameObject.SetActive(false);
        pipeEnd.AddToEnd(normie);
    }
}
