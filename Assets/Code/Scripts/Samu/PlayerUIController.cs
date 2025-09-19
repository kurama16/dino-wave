using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Player Controller Component")]
    [SerializeField] PlayerController playerController;
    [Tooltip("Health image to fill")]
    [SerializeField] Image healthFill;
    [SerializeField] TextMeshProUGUI healthText;

    void OnEnable()
    {
        if (!playerController) return;
        playerController.OnHealthChanged += HandleHealthChanged;
    }

    private void Start()
    {
        // Refrescar UI al habilitar
        HandleHealthChanged(playerController.GetCurrentHealth(), playerController.GetMaxHealth());
    }

    void OnDisable()
    {
        if (!playerController) 
            return;
        
        playerController.OnHealthChanged -= HandleHealthChanged;
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
