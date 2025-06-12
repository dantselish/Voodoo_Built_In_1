using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Normie normie = other.GetComponent<Normie>();

        if (!normie)
        {
            return;
        }

        if (normie.NormieType == NormieType.Enemy)
        {
            Cannon cannon = FindAnyObjectByType<Cannon>();
            if (cannon)
            {
                cannon.Kill();
            }
        }
    }
}
