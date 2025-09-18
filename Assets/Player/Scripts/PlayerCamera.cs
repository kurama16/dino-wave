using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 12, -8);
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private float tiltAngle = 60f;

    private Vector3 _smoothedPosition;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset;
        _smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = _smoothedPosition;

        transform.rotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }
}