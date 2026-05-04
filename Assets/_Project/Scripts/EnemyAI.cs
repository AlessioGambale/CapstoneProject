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

    [Header("Pesi Attacco")]
    [SerializeField] private int _attackWeightHigh = 100;
    [SerializeField] private int _attackWeightMid = 70;
    [SerializeField] private int _attackWeightLow = 20;

    [Header("Pesi Cura")]
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
        Debug.Log("EnemyAI start turn " + _enemy.name);
        StartCoroutine(TurnRoutine(player, onFinished));
    }

    private IEnumerator TurnRoutine(PlayerCreature player, Action onFinished)
    {
        yield return new WaitForSeconds(_actionDelay);

        StatusController status = _enemy.GetComponent<StatusController>();
        if (status != null && status.IsStunned())
        {
            Debug.Log("EnemyAI stunned skip turn " + _enemy.name);
            onFinished?.Invoke();
            yield break;
        }

        EnemyActionType action = PickAction(player);
        Debug.Log("EnemyAI action " + action);

        switch (action)
        {
            case EnemyActionType.Attack:
                ExecuteAttack(player);
                break;
            case EnemyActionType.Heal:
                ExecuteHeal();
                break;
        }

        onFinished?.Invoke();
    }

    private EnemyActionType PickAction(PlayerCreature player)
    {
        float enemyHpPercent = (float)_enemy.LifeController.CurrentHealth / _enemy.LifeController.MaxHealth;
        float playerHpPercent = (float)player.LifeController.CurrentHealth / player.LifeController.MaxHealth;

        if (playerHpPercent <= _playerKillThreshold)
        {
            return EnemyActionType.Attack;
        }

        if (_enemy.LifeController.CurrentHealth >= _enemy.LifeController.MaxHealth)
        {
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
            }
            else
            {
                attackWeight = _attackWeightLow;
                healWeight = _healWeightLow;
            }
        }

        int roll = UnityEngine.Random.Range(0, attackWeight + healWeight);
        return roll < attackWeight ? EnemyActionType.Attack : EnemyActionType.Heal;
    }

    private void ExecuteAttack(PlayerCreature player)
    {
        StatusController status = _enemy.GetComponent<StatusController>();
        _enemy.GetComponentInParent<AnimationParamHandler>()?.Attack();
        if (status != null && status.IsPanicked())
        {
            EnemyCreature ally = CombatManager.Instance.GetRandomAlly(_enemy);
            if (ally != null)
            {
                float roll = UnityEngine.Random.Range(0f, 1f);
                if (roll < 0.5f)
                {
                    Debug.Log("EnemyAI panic attack ally " + ally.name);
                    ally.Hit(_enemy.Stats.Attack);
                    return;
                }
            }
        }

        float damage = _enemy.Stats.Attack;
        Debug.Log("EnemyAI attack player damage " + damage);
        player.Hit(damage);
    }

    private void ExecuteHeal()
    {
        _enemy.GetComponentInParent<AnimationParamHandler>()?.Heal();
        float healAmount = _enemy.LifeController.MaxHealth * _healPercent;
        Debug.Log("EnemyAI heal " + (int)healAmount);
        _enemy.LifeController.AddHp((int)healAmount);
    }
}
