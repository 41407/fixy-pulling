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

        private float steering;
        private float pedalingInput;
        private float speed;
        [SerializeField, Range(0.0001f, 5f)] private float steeringAngleCoefficient = 1f;
        [Inject] private IFork fork;
        [Inject] private IRearWheel rearWheel;
        [Inject] private IDrivetrain drivetrain;
        [SerializeField] private AnimationCurve pedalingStrengthOverCadence = AnimationCurve.Constant(0, 90, 1f);

        [SerializeField] private float pedalingStrength = 0.1f;
        private float mashingCoefficient;
        [SerializeField] private float mashingStrength = 10f;

        private float MaximumSteering => Time.deltaTime * sensitivity.x;

        private float PedalingForce => pedalingInput * (1f + mashingCoefficient * mashingStrength) * CurrentPedalingStrength * Mathf.Clamp(drivetrain.GetCrankAnglePedalingStrengthModifier(), 0.1f, 1f);

        private float CurrentPedalingStrength => pedalingStrength * pedalingStrengthOverCadence.Evaluate(drivetrain.GetCurrentCadence(rearWheel));

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
                speed += PedalingForce;
            }
            else if (IsPedalingBackwards)
            {
                speed += PedalingForce / 2f;
            }

            speed -= rollingResistanceAcceleration.Evaluate(speed) * Time.fixedDeltaTime;

            transform.Translate(0, 0, speed * Time.fixedDeltaTime);
            Wheels.ForEach(wheel => wheel.SetSpeed(speed));

            drivetrain.SetCrankAngle(rearWheel);
            mashingCoefficient = Mathf.Min(mashingCoefficient, drivetrain.GetCrankAnglePedalingStrengthModifier());
            drivetrain.SetMashingCoefficient(mashingCoefficient);
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
            var pedalingInput = Input.GetAxisRaw("Vertical") * Time.deltaTime * sensitivity.y;
            if (this.pedalingInput <= 0.01f && pedalingInput > 0)
            {
                mashingCoefficient = drivetrain.GetCrankAnglePedalingStrengthModifier();
                Debug.Log($"We mashing at {mashingCoefficient * 100f} % effieciency!");
            }

            this.pedalingInput = pedalingInput;
        }
    }

    public interface IFixyController
    {
        float GetSpeed();
    }
}
