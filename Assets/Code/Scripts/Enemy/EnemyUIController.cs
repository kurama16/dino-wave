using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Enemy Health Component")]
    [SerializeField] private EnemyHealth enemyHealth;
    [Tooltip("Health image to fill")]
    [SerializeField] private Image healthFill;
    [Tooltip("Health text (Optional)")]
    [SerializeField] private TextMeshProUGUI healthText;
    [Tooltip("Camera to look at (main camera is not assigned)")]
    [SerializeField] private Camera targetCamera;

    void OnEnable()
    {
        if (!enemyHealth) return;
        enemyHealth.OnEnemyHealthChanged += HandleHealthChanged;
    }

    private void Start()
    {
        // Refrescar UI al habilitar
        HandleHealthChanged(enemyHealth.GetCurrentHealth(), enemyHealth.GetMaxHealth());
    }

    private void LateUpdate()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward, targetCamera.transform.rotation * Vector3.up);
    }

    void OnDisable()
    {
        if (!enemyHealth)
            return;

        enemyHealth.OnEnemyHealthChanged -= HandleHealthChanged;
    }

    void HandleHealthChanged(float current, float max)
    {
        float fillAmount = (max <= 0 || current <= 0) ? 0 : current / max;
        if (healthFill != null)
            healthFill.fillAmount = fillAmount;
        if (healthText != null)
            healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }
}
