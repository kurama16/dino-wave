using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeGapUIController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("TimeGap Health Component")]
    [SerializeField] private TimeGapController timegapController;
    [Tooltip("Health image to fill")]
    [SerializeField] private Image healthFill;
    [Tooltip("Health text (Optional)")]
    [SerializeField] private TextMeshProUGUI healthText;
    [Tooltip("Camera to look at (main camera is not assigned)")]
    [SerializeField] private Camera targetCamera;

    void OnEnable()
    {
        if (!timegapController) 
            return;
        timegapController.OnTimeGapHealthChanged += HandleHealthChanged;
    }

    void OnDisable()
    {
        if (!timegapController)
            return;

        timegapController.OnTimeGapHealthChanged -= HandleHealthChanged;
    }

    private void Start()
    {
        // Refrescar UI al habilitar
        HandleHealthChanged(timegapController.GetCurrentHealth(), timegapController.GetMaxHealth());
    }

    private void LateUpdate()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward, targetCamera.transform.rotation * Vector3.up);
    }

    private void HandleHealthChanged(float current, float max)
    {
        float fillAmount = (max <= 0 || current <= 0) ? 0 : current / max;
        if (healthFill != null)
            healthFill.fillAmount = fillAmount;
        if (healthText != null)
            healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }
}
