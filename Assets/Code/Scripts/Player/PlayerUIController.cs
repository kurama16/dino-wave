using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Player Controller Component")]
    [SerializeField] PlayerHealth playerHealth;
    [Tooltip("Health image to fill")]
    [SerializeField] Image healthFill;
    [SerializeField] TextMeshProUGUI healthText;

    void OnEnable()
    {
        if (!playerHealth) return;
        playerHealth.OnPlayerHealthChanged += HandleHealthChanged;
    }

    private void Start()
    {
        // Refrescar UI al habilitar
        HandleHealthChanged(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
    }

    void OnDisable()
    {
        if (!playerHealth) 
            return;
        
        playerHealth.OnPlayerHealthChanged -= HandleHealthChanged;
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
