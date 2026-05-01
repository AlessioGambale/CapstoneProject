using System;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [SerializeField] private float _bleedPercent = 0.08f;

    private float _buildupValue;
    private float _buildupThreshold = 100f;
    private StatusType _activeStatus = StatusType.None;
    private float _activeDuration;

    public event Action<float> OnBuildupChanged;
    public event Action<StatusType> OnStatusApplied;
    public event Action OnStatusExpired;

    public StatusType ActiveStatus => _activeStatus;
    public float BuildupValue => _buildupValue;
    public bool HasStatus => _activeStatus != StatusType.None;

    public void AddBuildup(StatusType type, float amount)
    {
        if (HasStatus) return;

        _buildupValue += amount;
        OnBuildupChanged?.Invoke(_buildupValue / _buildupThreshold);

        if (_buildupValue >= _buildupThreshold)
        {
            _buildupValue = 0f;
            OnBuildupChanged?.Invoke(0f);
            ApplyStatus(type);
        }
    }

    private void ApplyStatus(StatusType type)
    {
        _activeStatus = type;
        _activeDuration = GetDuration(type);
        Debug.Log($"[Status] {gameObject.name} — {type} applicato per {_activeDuration} turni");
        OnStatusApplied?.Invoke(type);
    }

    public void OnTurnEnd()
    {
        if (!HasStatus) return;

        ApplyStatusEffect();
        _activeDuration -= 1f;

        if (_activeDuration <= 0f)
        {
            Debug.Log($"[Status] {gameObject.name} — {_activeStatus} scaduto");
            _activeStatus = StatusType.None;
            OnStatusExpired?.Invoke();
        }
    }

    public bool IsStunned()
    {
        return _activeStatus == StatusType.Stun;
    }

    public bool IsPanicked()
    {
        return _activeStatus == StatusType.Panic;
    }

    private void ApplyStatusEffect()
    {
        switch (_activeStatus)
        {
            case StatusType.Bleeding:
                Creature creature = GetComponent<Creature>();
                float bleedDamage = creature.LifeController.MaxHealth * _bleedPercent;
                creature.TakeDamageRaw(bleedDamage);
                Debug.Log($"[Status] {gameObject.name} — Bleeding: -{bleedDamage} HP");
                break;
            case StatusType.Weakness:
            case StatusType.Stun:
            case StatusType.Fracture:
            case StatusType.Panic:
                break;
        }
    }

    private float GetDuration(StatusType type)
    {
        switch (type)
        {
            case StatusType.Bleeding: return 3f;
            case StatusType.Weakness: return 2f;
            case StatusType.Stun: return 1f;
            case StatusType.Fracture: return 2f;
            case StatusType.Panic: return 2f;
            default: return 1f;
        }
    }
}
