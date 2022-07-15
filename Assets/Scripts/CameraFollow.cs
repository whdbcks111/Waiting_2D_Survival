using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float _followSpeed = 3F;
    [SerializeField]
    private GameObject target;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, _followSpeed * Time.deltaTime);
    }
}
