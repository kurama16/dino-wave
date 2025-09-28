using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Player Controller Component")]
    [SerializeField] PlayerHealth playerHealth;
    [Header("UI Components")]
    [Tooltip("Health image to fill")]
    [SerializeField] Image healthFill;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI livesText;
    [Header("Panels")]
    [Tooltip("Defeat panel to show")]
    [SerializeField] CanvasGroup defeatPanel;
    [Tooltip("Victory panel to show")]
    [SerializeField] CanvasGroup victoryPanel;
    [Tooltip("Option panel to show")]
    [SerializeField] GameObject optionPanel;
    [SerializeField] private float fadeDuration = 5f;
    void OnEnable()
    {
        if (!playerHealth) 
            return;

        playerHealth.OnPlayerHealthChanged += HandleHealthChange;
        playerHealth.OnLivesChanged += HandleLivesChange;
        playerHealth.OnPlayerDie += ShowDefeatPanel;
    }

    void OnDisable()
    {
        if (!playerHealth)
            return;

        playerHealth.OnPlayerHealthChanged -= HandleHealthChange;
        playerHealth.OnLivesChanged -= HandleLivesChange;
        playerHealth.OnPlayerDie -= ShowDefeatPanel;
    }

    private void Start()
    {
        // Refrescar UI al habilitar
        HandleHealthChange(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
    }

    void HandleHealthChange(float current, float max)
    {
        float fillAmount = (max <= 0 || current <= 0) ? 0 : current / max;
        if (healthFill != null) 
            healthFill.fillAmount = fillAmount;
        if (healthText != null) 
            healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    void HandleLivesChange(int currentAmount)
    {
        if(livesText != null)
            livesText.text = currentAmount.ToString();
    }

    public void ShowDefeatPanel()
    {
        MusicManager.Instance.PlayLose();
        defeatPanel.gameObject.SetActive(true);
        StartCoroutine(FadeIn(defeatPanel));
    }

    public void ShowVictoryPanel()
    {
        MusicManager.Instance.PlayWin();
        victoryPanel.gameObject.SetActive(true);
        StartCoroutine(FadeIn(victoryPanel));
    }

    public void ShowOptionPanel()
    {
        optionPanel.gameObject.SetActive(true);
    }

    System.Collections.IEnumerator FadeIn(CanvasGroup panel)
    {
        panel.interactable = false;
        panel.blocksRaycasts = false;
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            panel.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        panel.interactable = true;
        panel.blocksRaycasts = true;
        panel.alpha = 1;
    }
}
