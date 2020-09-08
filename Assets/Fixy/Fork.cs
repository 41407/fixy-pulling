using UnityEngine;

namespace Fixy
{
    interface IFork
    {
        void SetAngle(float angle);
    }

    public class Fork : MonoBehaviour, IFork
    {
        public void SetAngle(float angle)
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, angle, transform.localRotation.eulerAngles.z);

        }
    }
}
