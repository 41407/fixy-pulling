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
    [SerializeField, Range(0.0001f, 5f)] private float steeringAngleCoefficient = 1f;
    [Inject] private IFork fork;

    private float MaximumSteering => Time.deltaTime * sensitivity.x;

    private void FixedUpdate()
    {
        HandleInput();
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
        speed += pedaling;
        if (speed > 20) speed = 20;

        transform.Translate(0, 0, speed * Time.fixedDeltaTime);
        Wheels.ForEach(wheel => wheel.SetSpeed(speed));
    }

    private void Turn()
    {
        var rollAngle = -Vector3.SignedAngle(Vector3.up, transform.up, transform.forward);
        var speedCoefficient = Mathf.Clamp01(speed);
        var angularSpeed = rollAngle * steeringAngleCoefficient * speedCoefficient;

        var turnRate = Mathf.Sign(angularSpeed) * Mathf.Abs(Mathf.Pow(angularSpeed, 2f)) * Time.fixedDeltaTime;
        var rollReduction = Mathf.Clamp(angularSpeed, -MaximumSteering / 2f, MaximumSteering / 2f);

        Turn(turnRate);
        StraightenRoll(rollReduction);

        fork.SetAngle(turnRate * 3f * Mathf.Max(1f, 20f - speed));
    }

    private void StraightenRoll(float rollReduction)
    {
        transform.Rotate(0, 0, rollReduction, Space.Self);
    }

    private void Turn(float turnRate)
    {
        transform.Rotate(0, turnRate, 0, Space.Self);
    }

    void HandleInput()
    {
        steering = Input.GetAxis("Horizontal") * Time.deltaTime * sensitivity.x;
        pedaling = Input.GetAxis("Vertical") * Time.deltaTime * sensitivity.y;
    }
}
