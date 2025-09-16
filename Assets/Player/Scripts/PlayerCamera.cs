using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    
    public Transform Player; 

    
    public Vector3 Offset = new Vector3(0, 12, -8);
    public float SmoothSpeed = 10f;
    public float TiltAngle = 60f;

    
    private Vector3 _smoothedPosition;

    void LateUpdate()
    {
        if (Player == null) return;

       
        Vector3 desiredPosition = Player.position + Offset;

       
        _smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);
        transform.position = _smoothedPosition;

        
        transform.rotation = Quaternion.Euler(TiltAngle, 0f, 0f);
    }
}