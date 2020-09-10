using UnityEngine;

namespace Fixy
{
    public interface IWheel
    {
        void SetSpeed(float unitsPerSecond);
    }

    public class Wheel : MonoBehaviour, IWheel
    {
        private float CurrentAngularSpeed { get; set; }
        private float WheelDiameter => transform.localScale.y;

        void FixedUpdate()
        {
            transform.Rotate(Vector3.right, CurrentAngularSpeed * Time.fixedDeltaTime);
        }

        public void SetSpeed(float unitsPerSecond)
        {
            CurrentAngularSpeed = unitsPerSecond * WheelDiameter * 100f * Mathf.PI;
        }
    }
}
