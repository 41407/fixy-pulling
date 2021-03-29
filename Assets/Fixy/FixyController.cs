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
        private bool isSkidding;
        [SerializeField] private float skiddingDeceleration;

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
            var steeringEffect = -1f * steering;
            var spinningEffect = pedalingInput * (180 - drivetrain.GetCrankAngle()) / 90f;
            var mashingEffect = mashingCoefficient * Mathf.Sign(spinningEffect);
            transform.Rotate(0, 0, steeringEffect + spinningEffect + mashingEffect * Mathf.Clamp01(speed), Space.Self);
        }

        private void Push()
        {
            if (isSkidding)
            {
                speed = Mathf.MoveTowards(speed, 0f, skiddingDeceleration * Time.deltaTime);
                Wheels.ForEach(wheel => wheel.SetSpeed(speed));
                rearWheel.SetSpeed(0f);
            }
            else if (IsPedalingForward)
            {
                speed += PedalingForce;
                Wheels.ForEach(wheel => wheel.SetSpeed(speed));
            }
            else if (IsPedalingBackwards)
            {
                speed += PedalingForce / 2f;
                Wheels.ForEach(wheel => wheel.SetSpeed(speed));
            }

            speed -= rollingResistanceAcceleration.Evaluate(speed) * Time.fixedDeltaTime;

            transform.Translate(0, 0, speed * Time.fixedDeltaTime);

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

            fork.SetAngle(turnRate * 3f * Mathf.Max(1f, 10f - speed));
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
            if (mashingCoefficient <= 0.01f && this.pedalingInput <= 0.01f && pedalingInput > 0)
            {
                mashingCoefficient = drivetrain.GetCrankAnglePedalingStrengthModifier();
            }

            if (mashingCoefficient > 0 && pedalingInput < 0)
            {
                isSkidding = true;
                mashingCoefficient = 0f;
            }

            if (isSkidding && pedalingInput >= 0f)
            {
                isSkidding = false;
            }

            this.pedalingInput = pedalingInput;
        }
    }

    public interface IFixyController
    {
        float GetSpeed();
    }
}
