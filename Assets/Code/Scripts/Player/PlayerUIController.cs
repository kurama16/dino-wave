using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Player Reference")]
    [SerializeField] GameObject player;
    [Tooltip("TimeGap Reference")]
    [SerializeField] private TimeGapController timeGap;

    [Header("UI Components")]
    [Tooltip("Health image to fill")]
    [SerializeField] private Image healthFill;
    [Tooltip("Skill image to show")]
    [SerializeField] private GameObject skillIcon;
    [Tooltip("Experience image to fill")]
    [SerializeField] private Image xpFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Panels")]
    [Tooltip("Defeat panel to show")]
    [SerializeField] private CanvasGroup defeatPanel;
    [Tooltip("Victory panel to show")]
    [SerializeField] private CanvasGroup victoryPanel;
    [Tooltip("Option panel to show")]
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private float fadeDuration = 5f;

    private PlayerStats _playerStats;
    private PlayerXP _playerXP;

    private void Awake()
    {
        if (player == null) 
            throw new InvalidOperationException("Player no asignado");
        _playerStats = player.GetComponent<PlayerStats>();
        if (_playerStats == null)
            throw new InvalidOperationException("El Player no tiene PlayerStats asignado");
        _playerXP = player.GetComponent<PlayerXP>();
        if (_playerXP == null)
            throw new InvalidOperationException("El Player no tiene PlayerXP asignado");
        if(timeGap == null)
            throw new InvalidOperationException("TimeGapController no asignado");
    }

    private void OnEnable()
    {
        _playerStats.OnPlayerHealthChanged += HandleHealthChange;
        _playerStats.OnLivesChanged += HandleLivesChanged;
        _playerStats.OnPlayerDie += ShowDefeatPanel;
        _playerStats.OnPlayerSkillActived += HandleSkillActived;
        timeGap.OnTimeGapDestroy += ShowDefeatPanel;
        _playerXP.OnXPChanged += HandleXPChanged;
        _playerXP.OnLevelChanged += HandleLevelChanged;
    }

    private void OnDisable()
    {
        _playerStats.OnPlayerHealthChanged -= HandleHealthChange;
        _playerStats.OnLivesChanged -= HandleLivesChanged;
        _playerStats.OnPlayerDie -= ShowDefeatPanel;
        _playerStats.OnPlayerSkillActived -= HandleSkillActived;
        timeGap.OnTimeGapDestroy -= ShowDefeatPanel;
        _playerXP.OnXPChanged -= HandleXPChanged;
        _playerXP.OnLevelChanged -= HandleLevelChanged;
    }

    private void Start()
    {
        // Refrescar UI al habilitar
        HandleHealthChange(_playerStats.GetCurrentHealth(), _playerStats.GetMaxHealth());
        HandleLivesChanged(_playerStats.GetCurrentLives());
        HandleXPChanged(_playerXP.GetCurrentXP(), _playerXP.GetXPToNextLevel());
        HandleLevelChanged(_playerXP.GetCurrentLevel());
    }

    private void HandleHealthChange(float current, float max)
    {
        float fillAmount = (max <= 0 || current <= 0) ? 0 : Mathf.Clamp01(current / max);
        if (healthFill != null)
            healthFill.fillAmount = fillAmount;
        if (healthText != null)
            healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    private void HandleXPChanged(float current, float max)
    {
        float fillAmount = (max <= 0 || current <= 0 || current >= max) ? 0 : Mathf.Clamp01(current / max);
        if (xpFill != null)
            xpFill.fillAmount = fillAmount;
    }

    private void HandleLivesChanged(int currentAmount)
    {
        if (livesText != null)
            livesText.text = currentAmount.ToString();
    }

    private void HandleLevelChanged(int currentLevel)
    {
        if (levelText != null)
            levelText.text = currentLevel.ToString();
    }

    private void HandleSkillActived(bool isActive)
    {
        if (skillIcon != null)
            skillIcon.SetActive(isActive);
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