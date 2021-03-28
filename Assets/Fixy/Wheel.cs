using UnityEngine;

namespace Fixy
{
    public interface IWheel
    {
        void SetSpeed(float unitsPerSecond);
        float RotationAngle { get; }
        float GetSpeed();
    }

    public class Wheel : MonoBehaviour, IWheel
    {
        private float CurrentAngularSpeed { get; set; }
        private float WheelDiameter => transform.localScale.y;

        public float RotationAngle { get; private set; } = 180f;

        public float GetSpeed()
        {
            return CurrentAngularSpeed;
        }

        void FixedUpdate()
        {
            RotationAngle += CurrentAngularSpeed * Time.fixedDeltaTime;
            transform.localRotation = Quaternion.Euler(RotationAngle, 0, 0);
        }

        public void SetSpeed(float unitsPerSecond)
        {
            CurrentAngularSpeed = unitsPerSecond * WheelDiameter * 100f * Mathf.PI;
        }
    }
}
