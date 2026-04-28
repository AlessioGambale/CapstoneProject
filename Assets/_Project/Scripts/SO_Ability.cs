using UnityEngine;

[CreateAssetMenu(menuName = "Item/Ability")]

public class SO_Ability : SO_GenericItem
{
    [SerializeField] private AbilityType _abilityType;
    [SerializeField] private float _value;
    [SerializeField] private int _apCost;
    [SerializeField] private float _duration;


    public AbilityType AbilityType => _abilityType;
    public float Value => _value;
    public int ApCost => _apCost;
    public float Duration => _duration;

    public override void Use(GameObject user)
    {
        PlayerCreature player = user.GetComponent<PlayerCreature>();

        switch (_abilityType)
        {
            case AbilityType.Shield:
                CombatManager.Instance.ActivateShield();
                break;

            case AbilityType.Desperation:
                int targetHP = Mathf.RoundToInt(player.PlayerLifeController.MaxHealth * 0.25f);
                player.PlayerLifeController.ForceSetHp(targetHP);
                CombatManager.Instance.ActivateDesperation(_value);
                break;

            case AbilityType.Crit:
                CombatManager.Instance.ActivateCrit();
                break;

            case AbilityType.Heal:
                int healAmount = Mathf.RoundToInt(player.PlayerLifeController.MaxHealth * _value);
                player.PlayerLifeController.AddHp(healAmount);
                CombatManager.Instance.ApplyHealAPPenalty();
                break;

            case AbilityType.BonusVsStatus:
                CombatManager.Instance.ActivateBonusVsStatus(_value);
                break;
        }
    }
}
