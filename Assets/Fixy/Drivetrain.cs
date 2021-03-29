using UnityEngine;

namespace Fixy
{
    internal interface IDrivetrain
    {
        void SetCrankAngle(IRearWheel rearWheel);
        float GetCrankAngle();
        float GetCurrentCadence(IRearWheel rearWheel);
        float GetCrankAnglePedalingStrengthModifier();
        void SetMashingCoefficient(float mashingCoefficient);
        float MashingCoefficient { get; }
        bool IsBraking { get; set; }
    }

    public class Drivetrain : MonoBehaviour, IDrivetrain
    {
        [SerializeField] private int chainring = 48;
        [SerializeField] private int cog = 16;

        private float GearRatio => (float) chainring / (float) cog;

        private float CrankAngle { get; set; }

        public void SetCrankAngle(IRearWheel rearWheel)
        {
            CrankAngle = rearWheel.CurrentAngle / GearRatio;
            while (CrankAngle > 360) CrankAngle -= 360;
        }

        public float GetCrankAngle() => CrankAngle;

        public float MashingCoefficient { get; private set; }
        public bool IsBraking { get; set; }

        public float GetCurrentCadence(IRearWheel rearWheel)
        {
            var currentCadence = GetCadenceRpm(rearWheel);
            return currentCadence;
        }

        public float GetCrankAnglePedalingStrengthModifier()
        {
            var pedalingStrengthModifier = (1 + Mathf.Cos((GetCrankAngle() + 90f) / 90f * Mathf.PI)) / 2f;
            return pedalingStrengthModifier;
        }

        public void SetMashingCoefficient(float mashingCoefficient)
        {
            MashingCoefficient = mashingCoefficient;
        }

        private float GetCadenceRpm(IRearWheel rearWheel)
        {
            return rearWheel.GetAngularSpeed() / 360f / GearRatio * 60;
        }
    }
}
