using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : GenericSingleton<CombatManager>
{
    private PlayerCreature _player;
    private List<EnemyCreature> _enemies = new List<EnemyCreature>();

    public event Action OnCombatStarted;
    public event Action OnCombatVictory;
    public event Action OnCombatDefeat;


    public bool IsShieldActive { get; private set; }
    public bool IsCritActive { get; private set; }
    public bool IsDesperationActive { get; private set; }
    public bool IsBonusVsStatusActive { get; private set; }

    private float _desperationMultiplier = 1f;
    private float _bonusVsStatusMultiplier = 1f;
    private int _healAPPenalty = 0;

    public void RegisterPlayer(PlayerCreature player)
    {
        _player = player;
        TryStartCombat();
    }

    public void RegisterEnemy(EnemyCreature enemy)
    {
        _enemies.Add(enemy);
        TryStartCombat();
    }

    private void TryStartCombat()
    {
        Debug.Log($"TryStartCombat — player: {_player}, nemici: {_enemies.Count}");
        if (_player == null || _enemies.Count == 0) return;
        Debug.Log("Combat avviato");

        TurnManager.Instance.OnTurnCycleFinished -= CheckCombatEnd;
        TurnManager.Instance.OnEnemyTurnStarted -= HandleEnemyTurn;
        TurnManager.Instance.OnTurnCycleFinished += CheckCombatEnd;
        TurnManager.Instance.OnEnemyTurnStarted += HandleEnemyTurn;

        TurnManager.Instance.SetUp(_player, _enemies);
        TurnManager.Instance.StartCombat();
        OnCombatStarted?.Invoke();
    }

    public void EndCombat()
    {
        TurnManager.Instance.OnTurnCycleFinished -= CheckCombatEnd;
        TurnManager.Instance.OnEnemyTurnStarted -= HandleEnemyTurn;
        _enemies.Clear();
        _player = null;
    }




    public void ActivateShield() => IsShieldActive = true;
    public void ConsumeShield() => IsShieldActive = false;
    public void ActivateCrit() => IsCritActive = true;

    public void ActivateDesperation(float multiplier)
    {
        IsDesperationActive = true;
        _desperationMultiplier = multiplier;
    }

    public void ActivateBonusVsStatus(float multiplier)
    {
        IsBonusVsStatusActive = true;
        _bonusVsStatusMultiplier = multiplier;
    }

    public void ApplyHealAPPenalty() => _healAPPenalty = 1;

    public int ConsumeHealPenalty()
    {
        int penalty = _healAPPenalty;
        _healAPPenalty = 0;
        return penalty;
    }





    public void ExecuteBaseAttack(EnemyCreature target)
    {
        SO_Weapon weapon = InventoryManager.Instance.CurrentWeapon;
        if (weapon == null) return;
        if (!TurnManager.Instance.TrySpendAP(weapon.BaseAttackAPCost)) return;

        float damage = CalculateDamage(_player.Stats.Attack, weapon.BaseDamage, target);
        ExecuteHit(target, damage, weapon.StatusBuildUp, weapon.StatusType);
    }

    public void ExecuteSpecialAttack(EnemyCreature target)
    {
        SO_Weapon weapon = InventoryManager.Instance.CurrentWeapon;
        if (weapon == null) return;
        if (!TurnManager.Instance.TrySpendAP(weapon.SpecialAttackAPCost)) return;

        float damage = CalculateDamage(_player.Stats.Attack, weapon.BaseDamage, target)
                       * weapon.SpecialDamageMultiplier;
        ExecuteHit(target, damage, weapon.StatusBuildUp * 2f, weapon.StatusType);
    }

    public void ExecuteAbility(EnemyCreature target)
    {
        SO_Ability ability = InventoryManager.Instance.CurrentAbility;
        if (ability == null) return;
        if (!TurnManager.Instance.TrySpendAP(ability.ApCost)) return;
        ability.Use(_player.gameObject);
    }

    private void ExecuteHit(EnemyCreature target, float damage, float buildUp, StatusType statusType)
    {
        StatusController status = target.GetComponent<StatusController>();
        bool isFractured = status != null && status.ActiveStatus == StatusType.Fracture;

        float finalDamage = isFractured ? damage * 1.25f : damage;

        Debug.Log($"[CombatManager] {_player.CreatureName} attacca {target.CreatureName} per {finalDamage}");

        target.Hit(finalDamage);

        ApplyStatusBuildup(target, buildUp, statusType);
    }





    private void HandleEnemyTurn()
    {
        EnemyCreature enemy = TurnManager.Instance.CurrentEnemy;
        if (enemy == null)
        {
            Debug.LogWarning("[CombatManager] HandleEnemyTurn — CurrentEnemy è null");
            return;
        }

        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai == null)
        {
            Debug.LogWarning($"[CombatManager] {enemy.name} non ha EnemyAI — uso fallback");
            ExecuteEnemyFallback(enemy);
            return;
        }

        ai.ExecuteTurn(_player, () => TurnManager.Instance.NotifyEnemyTurnFinished());
    }

    private void ExecuteEnemyFallback(EnemyCreature enemy)
    {
        float damage = CalculateDamage(enemy.Stats.Attack, enemy.Stats.Attack);
        _player.Hit(damage);

        TurnManager.Instance.NotifyEnemyTurnFinished();
    }

    public EnemyCreature GetRandomAlly(EnemyCreature self)
    {
        List<EnemyCreature> allies = _enemies.FindAll(e => e != self && !e.IsDead);
        if (allies.Count == 0) return null;
        return allies[UnityEngine.Random.Range(0, allies.Count)];
    }

   



    private float CalculateDamage(int attack, int weaponDamage, EnemyCreature target = null)
    {
        float damage = attack + weaponDamage;

        if (IsCritActive)
        {
            damage *= 2f;
            IsCritActive = false;
            Debug.Log($"[CombatManager] CRITICO — danno: {damage}");
        }

        if (IsDesperationActive)
        {
            damage *= _desperationMultiplier;
            IsDesperationActive = false;
            Debug.Log($"[CombatManager] Disperazione — danno: {damage}");
        }

        if (target != null)
        {
            StatusController status = target.GetComponent<StatusController>();
            if (status != null)
            {
                if (status.ActiveStatus == StatusType.Weakness)
                {
                    damage *= 1.25f;
                    Debug.Log($"[CombatManager] Weakness — danno: {damage}");
                }

                if (IsBonusVsStatusActive && status.HasStatus)
                {
                    damage *= _bonusVsStatusMultiplier;
                    IsBonusVsStatusActive = false;
                    Debug.Log($"[CombatManager] BonusVsStatus — danno: {damage}");
                }
            }
        }

        return damage;
    }

    private void ApplyStatusBuildup(EnemyCreature target, float amount, StatusType type)
    {
        if (type == StatusType.None) return;
        target.GetComponent<StatusController>()?.AddBuildup(type, amount);
    }

    



    private void CheckCombatEnd()
    {
        if (_player == null) return;

        if (_player.IsDead)
        {
            Debug.Log("[CombatManager] Sconfitta!");
            OnCombatDefeat?.Invoke();
            EndCombat();
            return;
        }

        if (_enemies.All(e => e.IsDead))
        {
            Debug.Log("[CombatManager] Vittoria!");
            OnCombatVictory?.Invoke();
            EndCombat();
        }
    }
}
