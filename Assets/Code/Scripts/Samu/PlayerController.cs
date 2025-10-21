using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Movimiento")]
    [Tooltip("Velocidad de movimiento del jugador")]
    [SerializeField] float moveSpeed = 7f;
    [Tooltip("Velocidad de giro")]
    [SerializeField] float rotationSpeed = 12f;
    [Tooltip("Transform de la camara para seguir al jugador")]
    [SerializeField] Transform cameraTransform;

    [Header("Salto")]
    [Tooltip("Fuerza del salto")]
    [SerializeField] float jumpForce = 7.5f;
    [Tooltip("Verificador del suelo")]
    [SerializeField] Transform groundCheck;
    [Tooltip("Tamano del verificador del suelo")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [Tooltip("Layer que tiene asignado el suelo")]
    [SerializeField] LayerMask groundMask;

    [Header("Salud")]
    [SerializeField] private float MaxHealth = 100f;

    private float _currentHealth;
    private Rigidbody rb;
    private bool isGrounded;
    private bool isDead;

    public event Action<float, float> OnHealthChanged;
    public event Action OnPlayerDie;
    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => MaxHealth;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _currentHealth = MaxHealth;
    }

    void Update()
    {
        PlayerMovement();
        if (Input.GetKeyDown(KeyCode.M))
            this.TakeDamage(50);
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void PlayerMovement()
    {
        // --- Movimiento en plano XZ relativo a la camara ---
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Direccion base: adelante/derecha de la camara proyectado al plano horizontal
        Vector3 forward = cameraTransform ? Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized : Vector3.forward;
        Vector3 right = cameraTransform ? Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized : Vector3.right;

        Vector3 moveDir = (forward * v + right * h).normalized;
        Vector3 targetVel = moveDir * moveSpeed;
        rb.linearVelocity = new Vector3(targetVel.x, rb.linearVelocity.y, targetVel.z);

        // Rotar el player hacia donde avanza
        if (moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // --- Salto ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // reinicio componente vertical para saltos consistentes
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    private void GroundCheck()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        }
        else
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out _, 1.1f, groundMask);
        }
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) 
            return;

        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        OnHealthChanged?.Invoke(_currentHealth, MaxHealth);
        if(_currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) 
            return;
        
        isDead = true;
        OnPlayerDie?.Invoke();
        gameObject.SetActive(false);
    }
}
