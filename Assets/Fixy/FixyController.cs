using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Fixy
{
    public class FixyController : MonoBehaviour, IFixyController
    {
        [Inject] private List<IWheel> Wheels { get; }

        [SerializeField] private Vector2 sensitivity = Vector2.one;
        [SerializeField] private AnimationCurve rollingResistanceAcceleration;
        [SerializeField] private AnimationCurve mashingAccelerationOverTime;
        [SerializeField] private float mashingForce = 10f;

        private float mashingTimer;
        private float steering;
        private float pedalingInput;
        private float speed;
        [SerializeField, Range(0.0001f, 5f)] private float steeringAngleCoefficient = 1f;
        [Inject] private IFork fork;
        [SerializeField] private AnimationCurve skiddingAcceleration;

        private float MaximumSteering => Time.deltaTime * sensitivity.x;


        private bool IsSkidding { get; set; }
        private bool IsMashing => mashingAccelerationOverTime.Evaluate(mashingTimer) > 0.5f;
        private bool IsPedalingBackwards => pedalingInput < 0;
        private bool IsPedalingForward => pedalingInput > 0;

        private void FixedUpdate()
        {
            HandleInput();
            Steer();
            Turn();
            Push();
        }

        public float GetSpeed() => speed;

        private void Steer()
        {
            transform.Rotate(0, 0, -steering * Mathf.Clamp01(speed), Space.Self);
        }

        private void Push()
        {
            if (IsPedalingForward)
            {
                IsSkidding = false;
                mashingTimer += Time.fixedDeltaTime;
                speed += pedalingInput;
                speed += mashingAccelerationOverTime.Evaluate(mashingTimer) * mashingForce * Time.fixedDeltaTime;
            }
            else if (IsPedalingBackwards)
            {
                if (IsMashing)
                {
                    IsSkidding = true;
                }

                if (IsSkidding)
                {
                    speed -= skiddingAcceleration.Evaluate(speed) * Time.fixedDeltaTime;
                }
            }

            if (!IsPedalingForward && !IsPedalingBackwards)
            {
                IsSkidding = false;
            }

            speed -= rollingResistanceAcceleration.Evaluate(speed) * Time.fixedDeltaTime;

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

        private void HandleInput()
        {
            steering = Input.GetAxis("Horizontal") * Time.deltaTime * sensitivity.x;
            pedalingInput = Input.GetAxisRaw("Vertical") * Time.deltaTime * sensitivity.y;
        }
    }

    public interface IFixyController
    {
        float GetSpeed();
    }
}
