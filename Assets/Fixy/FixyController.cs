using System.Collections.Generic;
using Fixy;
using UnityEngine;
using Zenject;

public class FixyController : MonoBehaviour
{
    [Inject] private List<IWheel> Wheels { get; }

    [SerializeField] private Vector2 sensitivity = Vector2.one;

    private float steering;
    private float pedaling;
    private float speed;
    [SerializeField, Range(0f, 1f)] private float timeScale = 0.3f;
    [SerializeField, Range(1f, 5f)] private float steeringAngleCoefficient = 1f;
    [SerializeField] private Transform fork;

    private float MaximumSteering => Time.deltaTime * sensitivity.x;

    private void FixedUpdate()
    {
        Time.timeScale = timeScale;
        HandleInput();
        speed += pedaling;
        if (speed > 20) speed = 20;
        Steer();
        Turn();

        Push();
    }

    private void Steer()
    {
        transform.Rotate(0, 0, -steering * Mathf.Clamp01(speed), Space.Self);
    }

    private void Push()
    {
        transform.Translate(0, 0, speed * Time.fixedDeltaTime);
        Wheels.ForEach(wheel => wheel.SetSpeed(speed));
    }

    private void Turn()
    {
        var angularSpeed = -Vector3.SignedAngle(Vector3.up, transform.up, transform.forward) * steeringAngleCoefficient * Mathf.Clamp01(speed) * Time.fixedDeltaTime;

        transform.Rotate(0, Mathf.Sign(angularSpeed) * Mathf.Abs(Mathf.Pow(angularSpeed, 2f)), 0, Space.Self);
        transform.Rotate(0, 0, Mathf.Clamp(angularSpeed, -MaximumSteering / 2f, MaximumSteering / 2f), Space.Self);
        fork.localRotation = Quaternion.Euler(fork.localRotation.eulerAngles.x, angularSpeed * 10f, fork.localRotation.eulerAngles.z);
    }

    void HandleInput()
    {
        steering = Input.GetAxis("Horizontal") * Time.deltaTime * sensitivity.x;
        pedaling = Input.GetAxis("Vertical") * Time.deltaTime * sensitivity.y;
    }
}
