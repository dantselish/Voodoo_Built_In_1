using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera start;
    [SerializeField] private CinemachineCamera transition;
    [SerializeField] private CinemachineCamera finish;


    public void Transition()
    {
        start.Priority = 10;
        transition.Priority = 20;
        finish.Priority = 10;
    }

    public void Finish()
    {
        transition.Priority = 10;
        finish.Priority = 20;
    }

    public void TurnOffFollow()
    {
        finish.Follow = null;
    }
}