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
        Debug.Log($"TryStartCombat , player: {_player}, nemici: {_enemies.Count}");
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
        TurnManager.Instance.StopCombat();
        TurnManager.Instance.OnTurnCycleFinished -= CheckCombatEnd;
        TurnManager.Instance.OnEnemyTurnStarted -= HandleEnemyTurn;
        _enemies.Clear();
        _player = null;
    }

    public void ExecuteBaseAttack(EnemyCreature target)
    {
        SO_Weapon weapon = InventoryManager.Instance.CurrentWeapon;
        if (weapon == null) return;
        if (!TurnManager.Instance.TrySpendAP(weapon.BaseAttackAPCost)) return;

        float damage = CalculateDamage(_player.Stats.Attack, weapon.BaseDamage);
        target.Hit(damage);
        ApplyStatusBuildup(target, weapon.StatusBuildUp, weapon.StatusType);
    }

    public void ExecuteSpecialAttack(EnemyCreature target)
    {
        SO_Weapon weapon = InventoryManager.Instance.CurrentWeapon;
        if (weapon == null) return;
        if (!TurnManager.Instance.TrySpendAP(weapon.SpecialAttackAPCost)) return;

        float damage = CalculateDamage(_player.Stats.Attack, weapon.BaseDamage)
                       * weapon.SpecialDamageMultiplier;
        target.Hit(damage);
        ApplyStatusBuildup(target, weapon.StatusBuildUp * 2f, weapon.StatusType);
    }

    public void ExecuteAbility(EnemyCreature target)
    {
        SO_Ability ability = InventoryManager.Instance.CurrentAbility;
        if (ability == null) return;
        if (!TurnManager.Instance.TrySpendAP(ability.ApCost)) return;

        ability.Use(_player.gameObject);
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
            Debug.LogWarning($"[CombatManager] {enemy.name} non ha EnemyAI — uso attacco base di fallback");
            ExecuteEnemyAction(enemy);
            return;
        }

        Debug.Log($"[CombatManager] Delego turno a EnemyAI di {enemy.name}");
        ai.ExecuteTurn(_player, () =>
        {
            TurnManager.Instance.NotifyEnemyTurnFinished();
        });
    }

    private void ExecuteEnemyAction(EnemyCreature enemy)
    {
        float damage = CalculateDamage(enemy.Stats.Attack, enemy.Stats.Attack);
        Debug.Log($"[CombatManager] Fallback — {enemy.name} attacca per {damage}");
        _player.Hit(damage);
        TurnManager.Instance.NotifyEnemyTurnFinished();
    }

    private float CalculateDamage(int attack, int weaponDamage) => attack + weaponDamage;

    private void ApplyStatusBuildup(EnemyCreature target, float amount, StatusType type) { }

    private void CheckCombatEnd()
    {
        if (_player == null) return;

        if (_player.IsDead)
        {
            OnCombatDefeat?.Invoke();
            EndCombat();
            return;
        }

        if (_enemies.All(e => e.IsDead))
        {
            OnCombatVictory?.Invoke();
            EndCombat();
        }
    }
}
