using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [Header("Weapon Info")]
    
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private TextMeshProUGUI _statusTypeText;

    [Header("Attacco Base")]
    [SerializeField] private TextMeshProUGUI _baseDamageText;
    [SerializeField] private TextMeshProUGUI _baseAPCostText;
    [SerializeField] private Button _baseAttackButton;

    [Header("Attacco Speciale")]
    [SerializeField] private TextMeshProUGUI _specialDamageText;
    [SerializeField] private TextMeshProUGUI _specialAPCostText;
    [SerializeField] private TextMeshProUGUI _specialNameText;
    [SerializeField] private TextMeshProUGUI _specialTypeText;
    [SerializeField] private Button _specialAttackButton;

    [Header("Abilità")]
    
    [SerializeField] private TextMeshProUGUI _abilityName;
    [SerializeField] private TextMeshProUGUI _abilityDescriptionText;
    [SerializeField] private TextMeshProUGUI _abilityAPCostText;
    [SerializeField] private Button _abilityButton;



    private void OnEnable()
    {
        Refresh();
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnAPChanged += UpdateButtons;
    }

    private void OnDisable()
    {
        if (CombatManager.Instance != null)
            CombatManager.Instance.OnCombatStarted -= Refresh;

        if (TurnManager.Instance != null)
            TurnManager.Instance.OnAPChanged -= UpdateButtons;
    }

    private void Refresh()
    {
        RefreshWeapon();
        RefreshAbility();
        if (TurnManager.Instance != null)
            UpdateButtons(TurnManager.Instance.CurrentAP);
    }

    private void RefreshWeapon()
    {
        SO_Weapon weapon = InventoryManager.Instance.CurrentWeapon;
        if (weapon == null) return;

        _weaponName.SetText(weapon.Name);
        _statusTypeText.SetText(GetStatusDescription(weapon.StatusType));
        _baseDamageText.SetText($"Danno: {weapon.BaseDamage}");
        _baseAPCostText.SetText($"AP: {weapon.BaseAttackAPCost}");
        _specialDamageText.SetText($"Danno: {weapon.BaseDamage * weapon.SpecialDamageMultiplier:F0}");
        _specialAPCostText.SetText($"AP: {weapon.SpecialAttackAPCost}");
        _specialNameText.SetText(GetSpecialName(weapon.SpecialType));     
        _specialTypeText.SetText(GetSpecialDescription(weapon.SpecialType)); 
    }

    private void RefreshAbility()
    {
        SO_Ability ability = InventoryManager.Instance.CurrentAbility;
        if (ability == null) return;

       
        _abilityName.SetText(ability.Name);
        _abilityDescriptionText.SetText(ability.Description);
        _abilityAPCostText.SetText($"AP: {ability.ApCost}");
    }

    private void UpdateButtons(int currentAP)
    {
        SO_Weapon weapon = InventoryManager.Instance.CurrentWeapon;
        SO_Ability ability = InventoryManager.Instance.CurrentAbility;

        _baseAttackButton.interactable = weapon != null && currentAP >= weapon.BaseAttackAPCost;
        _specialAttackButton.interactable = weapon != null && currentAP >= weapon.SpecialAttackAPCost;
        _abilityButton.interactable = ability != null && currentAP >= ability.ApCost;
    }

    private string GetStatusDescription(StatusType type)
    {
        switch (type)
        {
            case StatusType.Stun : return "Stuns enemy , skips turn";
            case StatusType.Bleeding : return "Enemy loses HP each turn";
            case StatusType.Weakness : return "Enemy takes more damage";
            case StatusType.Fracture : return "Ignores enemy defence";
            case StatusType.Panic : return "50% chance to hit ally";
            default: return "";

        }
    }

    private string GetSpecialName(SpecialAttackType type)
    {
        switch (type)
        {
            case SpecialAttackType.AOE: return "Area Attack";
            case SpecialAttackType.IgnoreDefense: return "Piercing Strike";
            case SpecialAttackType.HeavyHit: return "Heavy Strike";
            case SpecialAttackType.ApplyPanic: return "Panic Strike";
            case SpecialAttackType.Execute: return "Execute";
            default: return "";
        }
    }

    private string GetSpecialDescription(SpecialAttackType type)
    {
        switch (type)
        {
            case SpecialAttackType.HeavyHit: return "High damage single hit";
            case SpecialAttackType.IgnoreDefense: return "Bypasses enemy defence";
            case SpecialAttackType.AOE: return "Hits all enemies";
            case SpecialAttackType.ApplyPanic: return "Applies Panic status";
            case SpecialAttackType.Execute: return "Instant kill below 30% HP";
            default: return "";
        }
    }

    public void OnBaseAttack()
    {
        CombatManager.Instance.StartTargeting(target =>
            CombatManager.Instance.ExecuteBaseAttack(target));
    }

    public void OnSpecialAttack()
    {
        CombatManager.Instance.StartTargeting(target =>
            CombatManager.Instance.ExecuteSpecialAttack(target));
    }

    public void OnAbility()
    {
        CombatManager.Instance.ExecuteAbility();
    }

    public void OnEndTurn()
    {
        TurnManager.Instance.EndPlayerTurn();
    }
}
