using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Enemy Health Component")]
    [SerializeField] EnemyHealth enemyHealth;
    [Tooltip("Health image to fill")]
    [SerializeField] Image healthFill;
    [SerializeField] TextMeshProUGUI healthText;

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
