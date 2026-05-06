using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : GenericSingleton<CombatManager>
{
    [SerializeField] private GameObject _combatHUD;

    private PlayerCreature _player;
    private List<EnemyCreature> _enemies = new List<EnemyCreature>();

    public event Action OnCombatStarted;
    public event Action OnCombatVictory;
    public event Action OnCombatDefeat;
   

    public bool IsShieldActive { get; private set; }
    public bool IsCritActive { get; private set; }
    public bool IsDesperationActive { get; private set; }
    public bool IsBonusVsStatusActive { get; private set; }
    public PlayerCreature Player => _player;
    public List<EnemyCreature> Enemies => _enemies;


    private float _desperationMultiplier = 1f;
    private float _bonusVsStatusMultiplier = 1f;
    private int _healAPPenalty = 0;

    public bool isTargeting;
    private System.Action<EnemyCreature> _onTargetSelected;

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
        if (_player == null || _enemies.Count == 0) return;

        TurnManager.Instance.OnTurnCycleFinished -= CheckCombatEnd;
        TurnManager.Instance.OnEnemyTurnStarted -= HandleEnemyTurn;
        TurnManager.Instance.OnTurnCycleFinished += CheckCombatEnd;
        TurnManager.Instance.OnEnemyTurnStarted += HandleEnemyTurn;

        TurnManager.Instance.SetUp(_player, _enemies);
        TurnManager.Instance.StartCombat();
        OnCombatStarted?.Invoke();

    }

    public void StartTargeting(System.Action<EnemyCreature> onSelected)
    {
        isTargeting = true;
        _onTargetSelected = onSelected;
    }

    public void SelectTarget(EnemyCreature target)
    {
        if (!isTargeting) return;
        isTargeting = false;
        _onTargetSelected?.Invoke(target);
    }

    public void EndCombat()
    {
        TurnManager.Instance.OnTurnCycleFinished -= CheckCombatEnd;
        TurnManager.Instance.OnEnemyTurnStarted -= HandleEnemyTurn;

        if (_combatHUD != null)
            _combatHUD.SetActive(false);

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
        _player.GetComponent<AnimationParamHandler>()?.Attack();

        float damage = CalculateDamage(_player.Stats.Attack, weapon.BaseDamage, target);

        ExecuteHit(target, damage, weapon.StatusBuildUp, weapon.StatusType, false);

        if (_player == null) return;

        int speedRoll = UnityEngine.Random.Range(0, 100);
        if (speedRoll < _player.Stats.Speed)
        {
            TurnManager.Instance.RestoreAP(1);
            Debug.Log($"[Speed] AP recuperato! ({_player.Stats.Speed}% chance)");
        }
    }

    public void ExecuteSpecialAttack(EnemyCreature target)
    {
        SO_Weapon weapon = InventoryManager.Instance.CurrentWeapon;
        if (weapon == null) return;
        if (!TurnManager.Instance.TrySpendAP(weapon.SpecialAttackAPCost)) return;
        _player.GetComponent<AnimationParamHandler>()?.SpecialAttack();

        float damage = CalculateDamage(_player.Stats.Attack, weapon.BaseDamage, target);
        float buildUp = weapon.StatusBuildUp * 2f;

        switch (weapon.SpecialType)
        {
            case SpecialAttackType.HeavyHit:
                damage *= weapon.SpecialDamageMultiplier;
                break;

            case SpecialAttackType.IgnoreDefense:
                ExecuteHit(target, damage, buildUp, weapon.StatusType, true);
                return;

            case SpecialAttackType.AOE:
                foreach (var enemy in _enemies)
                {
                    if (!enemy.IsDead)
                        ExecuteHit(enemy, damage, buildUp, weapon.StatusType, false);
                }
                return;

            case SpecialAttackType.ApplyPanic:
                StatusController status = target.GetComponent<StatusController>();
                if (status != null)
                {
                    status.AddBuildup(StatusType.Panic, 100f);
                }
                break;

            case SpecialAttackType.Execute:
                float hpPercent = (float)target.LifeController.CurrentHealth / target.LifeController.MaxHealth;
                if (hpPercent < 0.3f)
                {
                    damage *= weapon.SpecialDamageMultiplier;
                }
                break;
        }

        ExecuteHit(target, damage, buildUp, weapon.StatusType, false);

        int speedRoll = UnityEngine.Random.Range(0, 100);
        if (_player != null && speedRoll < _player.Stats.Speed)
        {
            TurnManager.Instance.RestoreAP(1);
            Debug.Log($"[Speed] AP recuperato! ({_player.Stats.Speed}% chance)");
        }
    }

    public void ExecuteAbility()
    {
        SO_Ability ability = InventoryManager.Instance.CurrentAbility;
        if (ability == null) return;
        if (!TurnManager.Instance.TrySpendAP(ability.ApCost)) return;
        _player.GetComponent<AnimationParamHandler>()?.Ability();
        ability.Use(_player.gameObject);
    }

    public void ExecuteHit(EnemyCreature target, float damage, float buildUp, StatusType statusType, bool ignoreDefence = false)
    {
        StatusController status = target.GetComponent<StatusController>();
        bool isFractured = status != null && status.ActiveStatus == StatusType.Fracture;

        if (ignoreDefence || isFractured)
            target.Hit(damage, 0f);
        else
            target.Hit(damage);

        ApplyStatusBuildup(target, buildUp, statusType);

        if (_enemies.All(e => e.IsDead))
        {
            Debug.Log("[CombatManager] Tutti i nemici morti — vittoria immediata!");
            OnCombatVictory?.Invoke();
            EndCombat();
        }
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
        Debug.Log($"[CombatManager] Fallback — {enemy.name} attacca per {damage}");
        _player.Hit(damage);
        TurnManager.Instance.NotifyEnemyTurnFinished();
    }

    private float CalculateDamage(int attack, int weaponDamage, EnemyCreature target = null)
    {
        float damage = attack + weaponDamage;
        Debug.Log($"[Damage] Base: {damage}");

        if (IsCritActive)
        {
            damage *= 2f;
            IsCritActive = false;
            Debug.Log($"[Damage] Dopo Crit: {damage}");
        }

        if (IsDesperationActive)
        {
            damage *= _desperationMultiplier;
            IsDesperationActive = false;
            Debug.Log($"[Damage] Dopo Desperation ({_desperationMultiplier}x): {damage}");
        }

        if (_player.LifeController.IsHpCritical)
        {
            damage *= 1.5f;
            Debug.Log($"[Damage] Dopo Passiva critica (HP={_player.LifeController.CurrentHealth}/{_player.LifeController.MaxHealth}): {damage}");
        }

        if (target != null)
        {
            StatusController status = target.GetComponent<StatusController>();

            if (status != null && status.ActiveStatus == StatusType.Weakness)
            {
                damage *= 1.25f;
                Debug.Log($"[Damage] Dopo Weakness: {damage}");
            }

            if (IsBonusVsStatusActive)
            {
                if (status != null && status.HasStatus)
                {
                    damage *= _bonusVsStatusMultiplier;
                    Debug.Log($"[Damage] Dopo BonusVsStatus ({_bonusVsStatusMultiplier}x): {damage}");
                }
                IsBonusVsStatusActive = false;
            }
        }

        int luckRoll = UnityEngine.Random.Range (0, 100);
        if (luckRoll < _player.Stats.Luck)
        {
            damage *= 2f;
            Debug.Log($"[Damage] Luck crit ({_player.Stats.Luck}% chance): {damage}");
        }
        Debug.Log($"[Damage] TOTALE: {damage}");
        return damage;
    }

    private void ApplyStatusBuildup(EnemyCreature target, float amount, StatusType type)
    {
        if (type == StatusType.None) return;
        target.GetComponent<StatusController>()?.AddBuildup(type, amount);
    }

    public EnemyCreature GetRandomAlly(EnemyCreature self)
    {
        List<EnemyCreature> allies = _enemies.FindAll(e => e != self && !e.IsDead);
        if (allies.Count == 0) return null;
        return allies[UnityEngine.Random.Range(0, allies.Count)];
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