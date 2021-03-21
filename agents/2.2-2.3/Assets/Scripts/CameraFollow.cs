using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float followSpeed;
    [SerializeField] Vector3 offset;

    private Transform target;

    private void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        offset = target.transform.position - transform.position;
    }

    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.transform.position - offset;
    }
}
