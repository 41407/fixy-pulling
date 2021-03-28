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
    }

    public interface IRearWheel
    {
        float CurrentAngle { get; }
        float GetAngularSpeed();
    }
}
