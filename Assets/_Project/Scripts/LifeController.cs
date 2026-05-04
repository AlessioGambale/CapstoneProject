using System;
using UnityEngine;
using UnityEngine.Events;

public class LifeController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 90;
    [SerializeField] private int _currentHealth;

    public event Action<int, int> OnHealthChange;
    public event Action OnDeath;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;
    public bool IsHpCritical => (float)_currentHealth / _maxHealth <= 0.25f;

    private void Start()
    {
        SetHp(_maxHealth);
    }

    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = Mathf.Max(1, maxHealth);
    }

    public void RestoreFullHp()
    {
        SetHp(_maxHealth);
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(1f, damage);
        GetComponentInParent<AnimationParamHandler>()?.TakeHit();
        SetHp((int)(_currentHealth - finalDamage));
    }

    public void AddHp(int amount)
    {
        SetHp(_currentHealth + amount);
    }

    public void ForceSetHp(int hp)
    {
        SetHp(hp);
    }

    private void SetHp(int hp)
    {
        hp = Mathf.Clamp(hp, 0, _maxHealth);

        if (hp == _currentHealth) return;

        _currentHealth = hp;

        Debug.Log("[Life] " + gameObject.name + " HP: " + _currentHealth + "/" + _maxHealth);

        OnHealthChange?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            Debug.Log("[Life] " + gameObject.name + " morto");
            GetComponentInParent<AnimationParamHandler>()?.Death();
            OnDeath?.Invoke();
        }
    }
}