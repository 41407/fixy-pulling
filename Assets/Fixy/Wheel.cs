using System;
using UnityEngine;

namespace Fixy
{
    public interface IWheel
    {
        void SetSpeed(float kmh);
    }

    public class Wheel : MonoBehaviour, IWheel
    {
        private float CurrentAngularSpeed { get; set; }

        void FixedUpdate()
        {
            transform.Rotate(Vector3.right, CurrentAngularSpeed * Time.fixedDeltaTime);
        }

        public void SetSpeed(float kmh)
        {
            CurrentAngularSpeed = kmh * 200f;
        }
    }
}
