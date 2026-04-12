using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : GenericSingleton<CombatManager>
{
    private PlayerCreature _player;
    private List<EnemyCreature> _enemies = new List<EnemyCreature>();

    public event Action OnCombatStarted;
    public event Action OnCombatVictory;
    public event Action OnCombatDefeat;

    protected override void Awake()
    {
        base.Awake();
        TurnManager.Instance.OnTurnCycleFinished += CheckCombatEnd;
        TurnManager.Instance.OnEnemyTurnStarted += HandleEnemyTurn;
    }

    public void RegisterPlayer(PlayerCreature player)
    {
        _player = player;
        Debug.Log($"[Combat] Player registrato: {player.name}");
        TryStartCombat();
    }

    public void RegisterEnemy(EnemyCreature enemy)
    {
        _enemies.Add(enemy);
        Debug.Log($"[Combat] Nemico registrato: {enemy.name}");
        TryStartCombat();
    }

    private void TryStartCombat()
    {
        if (_player == null || _enemies.Count == 0) return;
        Debug.Log($"[Combat] Combat iniziato — Player: {_player.name}, Nemici: {_enemies.Count}");
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
        Debug.Log("[Combat] Combat terminato");
    }

    public void ExecuteBaseAttack(EnemyCreature target)
    {
        SO_Weapon weapon = InventoryManager.Instance.CurrentWeapon;
        if (weapon == null) return;
        if (!TurnManager.Instance.TrySpendAP(weapon.BaseAttackAPCost)) return;

        float damage = CalculateDamage(_player.Stats.Attack, weapon.BaseDamage);
        Debug.Log($"[Combat] {_player.name} attacca {target.name} per {damage} danni");
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
        Debug.Log($"[Combat] {_player.name} attacca (speciale) {target.name} per {damage} danni");
        target.Hit(damage);
        ApplyStatusBuildup(target, weapon.StatusBuildUp * 2f, weapon.StatusType);
    }

    public void ExecuteAbility(EnemyCreature target)
    {
        SO_Ability ability = InventoryManager.Instance.CurrentAbility;
        if (ability == null) return;
        if (!TurnManager.Instance.TrySpendAP(ability.ApCost)) return;

        Debug.Log($"[Combat] {_player.name} usa abilità: {ability.Name}");
        ability.Use(_player.gameObject);
    }

    private void HandleEnemyTurn()
    {
        EnemyCreature enemy = TurnManager.Instance.CurrentEnemy;
        if (enemy == null) return;
        ExecuteEnemyAction(enemy);
    }

    private void ExecuteEnemyAction(EnemyCreature enemy)
    {
        float damage = CalculateDamage(enemy.Stats.Attack, enemy.Stats.Attack);
        Debug.Log($"[Combat] {enemy.name} attacca {_player.name} per {damage} danni");
        _player.Hit(damage);
        TurnManager.Instance.NotifyEnemyTurnFinished();
    }

    private float CalculateDamage(int attack, int weaponDamage)
    {
        return attack + weaponDamage;
    }

    private void ApplyStatusBuildup(EnemyCreature target, float amount, StatusType type)
    {
       
    }

    private void CheckCombatEnd()
    {
        if (_player.IsDead)
        {
            Debug.Log("[Combat] COGLIONE!");
            OnCombatDefeat?.Invoke();
            EndCombat();
            return;
        }

        foreach (var enemy in _enemies)
        {
            if (!enemy.IsDead) return;
        }

        Debug.Log("[Combat] LA GASI!");
        OnCombatVictory?.Invoke();
        EndCombat();
    }
}
