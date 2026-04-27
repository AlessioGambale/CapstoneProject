using System;
using System.Collections;
using UnityEngine;

public enum EnemyActionType { Attack, Heal }

public class EnemyAI : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float _actionDelay = 1.5f;

    [Header("Soglie HP Nemico")]
    [SerializeField] private float _healThresholdLow = 0.3f;
    [SerializeField] private float _healThresholdMid = 0.6f;

    [Header("Soglia HP Player")]
    [SerializeField] private float _playerKillThreshold = 0.3f;

    [Header("Pesi Attacco (per soglia nemico)")]
    [SerializeField] private int _attackWeightHigh = 100;
    [SerializeField] private int _attackWeightMid = 70;
    [SerializeField] private int _attackWeightLow = 20;

    [Header("Pesi Cura (per soglia nemico)")]
    [SerializeField] private int _healWeightHigh = 0;
    [SerializeField] private int _healWeightMid = 30;
    [SerializeField] private int _healWeightLow = 80;

    [Header("Cura")]
    [SerializeField] private float _healPercent = 0.2f;

    private EnemyCreature _enemy;

    private void Awake()
    {
        _enemy = GetComponent<EnemyCreature>();
    }

    public void ExecuteTurn(PlayerCreature player, Action onFinished)
    {
        Debug.Log($"[EnemyAI] {_enemy.name} inizia il turno — attende {_actionDelay}s");
        StartCoroutine(TurnRoutine(player, onFinished));
    }

    private IEnumerator TurnRoutine(PlayerCreature player, Action onFinished)
    {
        yield return new WaitForSeconds(_actionDelay);

        EnemyActionType action = PickAction(player);
        Debug.Log($"[EnemyAI] {_enemy.name} sceglie: {action}");

        switch (action)
        {
            case EnemyActionType.Attack:
                ExecuteAttack(player);
                break;
            case EnemyActionType.Heal:
                ExecuteHeal();
                break;
        }

        Debug.Log($"[EnemyAI] {_enemy.name} turno finito");
        onFinished?.Invoke();
    }

    private EnemyActionType PickAction(PlayerCreature player)
    {
        float enemyHpPercent = (float)_enemy.LifeController.CurrentHealth / _enemy.LifeController.MaxHealth;
        float playerHpPercent = (float)player.LifeController.CurrentHealth / player.LifeController.MaxHealth;

        Debug.Log($"[EnemyAI] {_enemy.name} HP: {enemyHpPercent:P0} — Player HP: {playerHpPercent:P0}");

        if (playerHpPercent <= _playerKillThreshold)
        {
            Debug.Log("[EnemyAI] Player in fin di vita = attacca");
            return EnemyActionType.Attack;
        }

        if (_enemy.LifeController.CurrentHealth >= _enemy.LifeController.MaxHealth)
        {
            Debug.Log("[EnemyAI] HP pieni = attacca");
            return EnemyActionType.Attack;
        }

        int attackWeight;
        int healWeight;

        if (enemyHpPercent > _healThresholdMid)
        {
            attackWeight = _attackWeightHigh;
            healWeight = _healWeightHigh;
        }
        else if (enemyHpPercent > _healThresholdLow)
        {
            attackWeight = _attackWeightMid;
            healWeight = _healWeightMid;
        }
        else
        {
            if (playerHpPercent <= 0.5f)
            {
                attackWeight = 60;
                healWeight = 40;
                Debug.Log("[EnemyAI] HP bassi ma player meta vita = aggressivo");
            }
            else
            {
                attackWeight = _attackWeightLow;
                healWeight = _healWeightLow;
                Debug.Log("[EnemyAI] HP bassi = preferisce curarsi");
            }
        }

        int roll = UnityEngine.Random.Range(0, attackWeight + healWeight);
        Debug.Log($"[EnemyAI] Roll: {roll} su {attackWeight + healWeight} (attacco:{attackWeight} cura:{healWeight})");

        return roll < attackWeight ? EnemyActionType.Attack : EnemyActionType.Heal;
    }

    private void ExecuteAttack(PlayerCreature player)
    {
        float damage = _enemy.Stats.Attack;
        Debug.Log($"[EnemyAI] {_enemy.name} attacca il player per {damage} danni");
        player.Hit(damage);
    }

    private void ExecuteHeal()
    {
        float healAmount = _enemy.LifeController.MaxHealth * _healPercent;
        Debug.Log($"[EnemyAI] {_enemy.name} si cura di {(int)healAmount} HP");
        _enemy.LifeController.AddHp((int)healAmount);
    }
}
