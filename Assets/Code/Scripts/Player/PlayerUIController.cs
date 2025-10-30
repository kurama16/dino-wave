using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerXP playerXP;
    [SerializeField] private TimeGapController timeGap;

    [Header("UI Components")]
    [SerializeField] private Image healthFill;
    [SerializeField] private Image xpFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Panels")]
    [SerializeField] private CanvasGroup defeatPanel;
    [SerializeField] private CanvasGroup victoryPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private float fadeDuration = 5f;

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerHealthChanged += HandleHealthChange;
            playerHealth.OnLivesChanged += HandleLivesChanged;
            playerHealth.OnPlayerDie += ShowDefeatPanel;
        }

        if (timeGap != null)
        {
            timeGap.OnTimeGapDestroy += ShowDefeatPanel;
        }

        if (playerXP != null)
        {
            playerXP.OnXPChanged += HandleXPChanged;
            playerXP.OnLevelChanged += HandleLevelChanged;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerHealthChanged -= HandleHealthChange;
            playerHealth.OnLivesChanged -= HandleLivesChanged;
            playerHealth.OnPlayerDie -= ShowDefeatPanel;
        }

        if (timeGap != null)
        {
            timeGap.OnTimeGapDestroy -= ShowDefeatPanel;
        }

        if (playerXP != null)
        {
            playerXP.OnXPChanged -= HandleXPChanged;
            playerXP.OnLevelChanged -= HandleLevelChanged;
        }
    }

    private void Start()
    {
        if (playerHealth != null)
        {
            HandleHealthChange(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
            HandleLivesChanged(playerHealth.GetCurrentLives());
        }

        if (playerXP != null)
        {
            HandleXPChanged(playerXP.CurrentXP, playerXP.XPToNextLevel);
            HandleLevelChanged(playerXP.CurrentLevel);
        }
    }

    private void HandleHealthChange(float current, float max)
    {
        float fillAmount = (max <= 0 || current <= 0) ? 0 : current / max;
        if (healthFill != null) healthFill.fillAmount = fillAmount;
        if (healthText != null) healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    private void HandleXPChanged(float current, float max)
    {
        float fillAmount = (max <= 0) ? 0 : Mathf.Clamp01(current / max);
        if (xpFill != null) xpFill.fillAmount = fillAmount;
    }

    private void HandleLivesChanged(int currentAmount)
    {
        if (livesText != null) livesText.text = currentAmount.ToString();
    }

    private void HandleLevelChanged(int currentLevel)
    {
        if (levelText != null) levelText.text = currentLevel.ToString();
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
        optionPanel.SetActive(true);
    }

    private System.Collections.IEnumerator FadeIn(CanvasGroup panel)
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