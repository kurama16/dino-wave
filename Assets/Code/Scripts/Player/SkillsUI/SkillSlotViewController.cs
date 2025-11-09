using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotViewController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Image icon;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TextMeshProUGUI slotText;

    private string _skillId;
    private float _cdStart;
    private float _cdEnd;
    private bool _tracking;
    private PlayerAbilityController _playerAbilityController;

    private void OnDisable()
    {
        if (_playerAbilityController != null)
            _playerAbilityController.OnSkillCooldownStarted -= HandleCooldownStarted;
    }

    private void Update()
    {
        if (_tracking) 
            UpdateFill();
    }

    public void Bind(ActiveSkillSO skill, PlayerAbilityController owner)
    {
        if (_playerAbilityController != null)
            _playerAbilityController.OnSkillCooldownStarted -= HandleCooldownStarted;

        _playerAbilityController = owner;
        _skillId = skill.skillId;
        icon.sprite = skill.icon;
        slotText.text = skill.displayName;
        SetCooldownFill(0f);

        _playerAbilityController.OnSkillCooldownStarted += HandleCooldownStarted;
    }

    private void HandleCooldownStarted(string skillId, float start, float end)
    {
        if (skillId != _skillId) 
            return;

        _cdStart = start;
        _cdEnd = end;
        _tracking = true;
        UpdateFill();
    }

    private void UpdateFill()
    {
        float duration = Mathf.Max(0.0001f, _cdEnd - _cdStart);
        float remaining = Mathf.Clamp(_cdEnd - Time.time, 0f, duration);
        float time = remaining / duration;

        SetCooldownFill(time);

        if (remaining <= 0f) 
            _tracking = false;
    }

    private void SetCooldownFill(float normalized)
    {
        if (cooldownOverlay != null)
        {
            if (cooldownOverlay.sprite == null)
            {
                cooldownOverlay.sprite = icon.sprite;
                cooldownOverlay.type = Image.Type.Filled;
                cooldownOverlay.fillMethod = Image.FillMethod.Radial360;
                cooldownOverlay.fillClockwise = false;
            }

            cooldownOverlay.enabled = normalized > 0f;
            cooldownOverlay.fillAmount = normalized;
        }
    }
}
