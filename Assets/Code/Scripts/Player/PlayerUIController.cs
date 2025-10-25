using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Player Controller Component")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PlayerXP playerXP;
    [Tooltip("TimeGap Controller Component")]
    [SerializeField] TimeGapController timeGap;

    [Header("UI Components")]
    [Tooltip("Health image to fill")]
    [SerializeField] Image healthFill;
    [Tooltip("Experience image to fill")]
    [SerializeField] Image xpFill;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI levelText;

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
        if (!playerStats || !timeGap || !playerXP)
            return;

        playerStats.OnPlayerHealthChanged += HandleHealthChange;
        playerStats.OnLivesChanged += HandleLivesChanged;
        playerStats.OnPlayerDie += ShowDefeatPanel;
        timeGap.OnTimeGapDestroy += ShowDefeatPanel;
        playerXP.OnXPChanged += HandleXPChanged;
        playerXP.OnLevelChanged += HandleLevelChanged;
    }

    void OnDisable()
    {
        if (!playerStats || !timeGap || !playerXP)
            return;

        playerStats.OnPlayerHealthChanged -= HandleHealthChange;
        playerStats.OnLivesChanged -= HandleLivesChanged;
        playerStats.OnPlayerDie -= ShowDefeatPanel;
        timeGap.OnTimeGapDestroy -= ShowDefeatPanel;
        playerXP.OnXPChanged -= HandleXPChanged;
        playerXP.OnLevelChanged -= HandleLevelChanged;
    }

    private void Start()
    {
        // Refrescar UI al habilitar
        HandleHealthChange(playerStats.GetCurrentHealth(), playerStats.GetMaxHealth());
        HandleLivesChanged(playerStats.GetCurrentLives());
        HandleXPChanged(playerXP.GetCurrentXP(), playerXP.GetXPToNextLevel());
        HandleLevelChanged(playerXP.GetCurrentLevel());

        Debug.Log("playerStats.GetCurrentHealth()" + playerStats.GetCurrentHealth());
        Debug.Log("playerStats.GetMaxHealth()" + playerStats.GetMaxHealth());
    }

    void HandleHealthChange(float current, float max)
    {
        float fillAmount = (max <= 0 || current <= 0) ? 0 : current / max;
        if (healthFill != null)
            healthFill.fillAmount = fillAmount;
        if (healthText != null)
            healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    public void HandleXPChanged(float current, float max)
    {
        float fillAmount = (max <= 0 || current <= 0 || current >= max) ? 0 : current / max;
        if (xpFill != null)
            xpFill.fillAmount = fillAmount;
    }

    void HandleLivesChanged(int currentAmount)
    {
        if(livesText != null)
            livesText.text = currentAmount.ToString();
    }

    void HandleLevelChanged(int currentLevel)
    {
        if (levelText != null)
            levelText.text = currentLevel.ToString();
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
