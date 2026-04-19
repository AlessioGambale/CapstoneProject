using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnManager : GenericSingleton<TurnManager>
{
    private PlayerCreature _player;
    private List<EnemyCreature> _enemies = new List<EnemyCreature>();

    public event Action OnPlayerTurnStarted;
    public event Action OnEnemyTurnStarted;
    public event Action OnTurnCycleFinished;
    public event Action<int> OnAPChanged;

    public bool IsPlayerTurn { get; private set; }
    public EnemyCreature CurrentEnemy { get; private set; }
    public int CurrentAP { get; private set; }

    private bool _enemyTurnFinished;
    private bool _combatActive;

    public void SetUp(PlayerCreature player, List<EnemyCreature> enemies)
    {
        _player = player;
        _enemies = enemies;
        _combatActive = true;
    }

    public void StopCombat()
    {
        _combatActive = false;
        StopAllCoroutines();
    }

    public void StartCombat()
    {
        IsPlayerTurn = true;
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        if (!_combatActive) return;
        IsPlayerTurn = true;
        CurrentAP = _player.Stats.MaxAP;
        Debug.Log($"Turno player — MaxAP: {_player.Stats.MaxAP}, CurrentAP: {CurrentAP}");
        OnAPChanged?.Invoke(CurrentAP);
        OnPlayerTurnStarted?.Invoke();
    }

    public bool TrySpendAP(int cost)
    {
        Debug.Log($"TrySpendAP — costo: {cost}, AP disponibili: {CurrentAP}");
        if (CurrentAP < cost)
        {
            Debug.Log("AP insufficienti");
            return false;
        }
        CurrentAP -= cost;
        OnAPChanged?.Invoke(CurrentAP);
        Debug.Log($"AP rimasti: {CurrentAP}");
        return true;
    }

    public void RestoreAP(int amount)
    {
        CurrentAP = Mathf.Clamp(CurrentAP + amount, 0, _player.Stats.MaxAP);
        OnAPChanged?.Invoke(CurrentAP);
    }

    public void EndPlayerTurn()
    {
        if (!_combatActive) return;
        IsPlayerTurn = false;
        StartEnemyTurn();
    }

    public void StartEnemyTurn() => StartCoroutine(EnemyTurnRoutine());

    public void NotifyEnemyTurnFinished() => _enemyTurnFinished = true;

    private IEnumerator EnemyTurnRoutine()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.IsDead) continue;

            CurrentEnemy = enemy;
            _enemyTurnFinished = false;
            OnEnemyTurnStarted?.Invoke();
            Debug.Log($"Aspetto fine turno {enemy.name}");

            float timer = 0f;
            yield return new WaitUntil(() =>
            {
                timer += Time.deltaTime;
                return _enemyTurnFinished || enemy.IsDead || timer >= 5f;
            });

            Debug.Log($"Fine turno {enemy.name} — finished:{_enemyTurnFinished} dead:{enemy.IsDead} timeout:{timer >= 5f}");
        }

        Debug.Log("OnTurnCycleFinished");
        CurrentEnemy = null;
        OnTurnCycleFinished?.Invoke();

        if (_combatActive)
            StartPlayerTurn();
    }
}
