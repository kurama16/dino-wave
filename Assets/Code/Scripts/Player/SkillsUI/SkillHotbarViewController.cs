using System.Collections.Generic;
using UnityEngine;

public class SkillHotbarViewController : MonoBehaviour
{
    [SerializeField] private PlayerAbilityController controller;
    [SerializeField] private SkillSlotViewController slotPrefab;
    [SerializeField] private Transform slotsParent;

    private void OnEnable()
    {
        controller.OnActiveSkillReady += Build;
    }
    private void OnDisable()
    {
        controller.OnActiveSkillReady -= Build;
    }

    private void Build(IReadOnlyList<ActiveSkillSO> skills)
    {
        foreach (Transform c in slotsParent) 
            Destroy(c.gameObject);

        foreach (var skill in skills)
        {
            var slot = Instantiate(slotPrefab, slotsParent);
            slot.Bind(skill, controller);
        }
    }
}
