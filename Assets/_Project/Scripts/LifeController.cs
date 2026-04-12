using UnityEngine;
using UnityEngine.Events;

public class LifeController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 90;
    [SerializeField] private int _currentHealth;

    [Header("Events")]
    [SerializeField] private UnityEvent _onDeath;
    [SerializeField] private UnityEvent<int, int> _onHealthChange;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;
    public bool IsHpCritical => (float)_currentHealth / _maxHealth <= 0.25f;

    private void Start()
    {
        SetHp(_maxHealth);
    }

    public void SetMaxHealth(int maxHealth) => _maxHealth = Mathf.Max(1, maxHealth);

    public void RestoreFullHp() => SetHp(_maxHealth);

    public void TakeDamage(float damage, float defensePercent = 0f)
    {
        float finalDamage = damage * (1f - Mathf.Clamp01(defensePercent));
        finalDamage = Mathf.Max(1f, finalDamage);
        SetHp((int)(_currentHealth - finalDamage));
    }

    public void AddHp(int amount) => SetHp(_currentHealth + amount);

    public void ForceSetHp(int hp) => SetHp(hp);

    private void SetHp(int hp)
    {
        hp = Mathf.Clamp(hp, 0, _maxHealth);
        if (hp != _currentHealth)
        {
            _currentHealth = hp;
            Debug.Log($"[Life] {gameObject.name} HP: {_currentHealth}/{_maxHealth}");
            _onHealthChange?.Invoke(_currentHealth, _maxHealth);
            if (_currentHealth <= 0)
            {
                Debug.Log($"[Life] {gameObject.name} è schiattattt");
                _onDeath?.Invoke();
            }
        }
    }
}