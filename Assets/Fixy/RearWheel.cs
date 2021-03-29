using UnityEngine;
using Zenject;

namespace Fixy
{
    public class RearWheel : MonoBehaviour, IRearWheel
    {
        [Inject] private IWheel Wheel { get; }

        public float CurrentAngle => Wheel.RotationAngle;

        public float GetAngularSpeed()
        {
            return Wheel.GetSpeed();
        }

        public void SetSpeed(float groundSpeed) => Wheel.SetSpeed(groundSpeed);
    }

    public interface IRearWheel
    {
        float CurrentAngle { get; }
        float GetAngularSpeed();
        void SetSpeed(float groundSpeed);
    }
}
