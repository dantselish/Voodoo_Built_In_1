using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera intro;
    [SerializeField] private CinemachineCamera start;
    [SerializeField] private CinemachineCamera transition;
    [SerializeField] private CinemachineCamera finish;


    private void Start()
    {
        //intro.Priority = 20;
        start.Priority = 10;
        transition.Priority = 10;
        finish.Priority = 10;
    }

    public void StartCamera()
    {
        start.Priority = 30;
    }

    public void Transition()
    {
        print("Transition");
        transition.Priority = 40;
    }

    public void Finish()
    {
        finish.Priority = 50;
    }

    public void TurnOffFollow()
    {
        finish.Follow = null;
    }
}