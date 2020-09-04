using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 offset;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        transform.LookAt(target);
        transform.position = Vector3.Lerp(transform.position, target.position + target.rotation * offset, 0.05f);
    }
}
