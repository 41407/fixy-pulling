using UnityEngine;

namespace Camera
{
    public class FirstPersonCamera : MonoBehaviour
    {
        private Vector3 inputPosition;
        [SerializeField] private float sensitivity = 0.001f;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            inputPosition = Input.mousePosition;
        }

        void Update()
        {
            transform.Rotate((Input.mousePosition.y - inputPosition.y) * -sensitivity, (Input.mousePosition.x - inputPosition.x) * sensitivity, 0);
            var rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            transform.rotation = rotation;
            inputPosition = Input.mousePosition;
        }
    }
}
