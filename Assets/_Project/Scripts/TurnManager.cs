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

    public void SetUp(PlayerCreature player, List<EnemyCreature> enemies)
    {
        _player = player;
        _enemies = enemies;
    }

    public void StartCombat()
    {
        IsPlayerTurn = true;
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        IsPlayerTurn = true;
        CurrentAP = Mathf.Max(0, _player.Stats.MaxAP - CombatManager.Instance.ConsumeHealPenalty());
        OnAPChanged?.Invoke(CurrentAP);
        OnPlayerTurnStarted?.Invoke();
        Debug.Log($"[Turn] Turno player — AP: {CurrentAP}");
    }

    public bool TrySpendAP(int cost)
    {
        if (CurrentAP < cost)
        {
            Debug.Log($"[Turn] AP insufficienti — richiesti: {cost}, disponibili: {CurrentAP}");
            return false;
        }
        CurrentAP -= cost;
        OnAPChanged?.Invoke(CurrentAP);
        Debug.Log($"[Turn] AP spesi: {cost} — AP rimasti: {CurrentAP}");
        return true;
    }

    public void RestoreAP(int amount)
    {
        CurrentAP = Mathf.Clamp(CurrentAP + amount, 0, _player.Stats.MaxAP);
        OnAPChanged?.Invoke(CurrentAP);
        Debug.Log($"[Turn] AP recuperati: {amount} — AP totali: {CurrentAP}");
    }

    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
        Debug.Log("[Turn] Fine turno player — inizia turno nemici");
        StartCoroutine(EnemyTurnRoutine());
    }

    public void NotifyEnemyTurnFinished()
    {
        _enemyTurnFinished = true;
    }

    private IEnumerator EnemyTurnRoutine()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.IsDead) continue;

            CurrentEnemy = enemy;
            _enemyTurnFinished = false;
            Debug.Log($"[Turn] Turno nemico — {enemy.CreatureName}");
            OnEnemyTurnStarted?.Invoke();

            yield return new WaitUntil(() => _enemyTurnFinished || enemy.IsDead);

            enemy.GetComponent<StatusController>()?.OnTurnEnd();
        }

        CurrentEnemy = null;
        OnTurnCycleFinished?.Invoke();
    }
}
