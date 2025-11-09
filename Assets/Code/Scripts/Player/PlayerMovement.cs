using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Animations")]
    [SerializeField] private Animator animator;

    private Rigidbody _rb;
    private Vector3 _moveDirection;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f;
            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void FixedUpdate()
    {

        Vector3 velocity = _moveDirection * moveSpeed;
        float forwardValue = Vector3.Dot(velocity, _moveDirection);
        animator.SetFloat("forward", forwardValue);
        _rb.MovePosition(_rb.position + velocity * Time.fixedDeltaTime);
    }
}