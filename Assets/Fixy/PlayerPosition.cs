using UnityEngine;

namespace Fixy
{
    public interface IPlayerPosition
    {
        Vector3 Position { get; }
    }

    public class PlayerPosition : MonoBehaviour, IPlayerPosition
    {
        public Vector3 Position => transform.position;
    }
}
