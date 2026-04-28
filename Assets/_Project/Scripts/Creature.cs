using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [SerializeField] private string _creatureName;
    [SerializeField] private SO_Stats _stats;

    public LifeController LifeController { get; private set; }

    public string CreatureName => _creatureName;
    public SO_Stats Stats => _stats;
    public bool IsDead => LifeController.IsDead;
    public bool IsHpCritical => LifeController.IsHpCritical;

    protected virtual void Awake()
    {
        LifeController = GetComponent<LifeController>();
        LifeController.SetMaxHealth(_stats.MaxHP);
        LifeController.RestoreFullHp();
    }

    public virtual void Hit(float damage)
    {
        float defence = _stats.DefencePercent;
        float finalDamage = damage * (1f - Mathf.Clamp01(defence));

        LifeController.TakeDamage(finalDamage);
    }

    public void TakeDamageRaw(float damage)
    {
        LifeController.TakeDamage(damage);
    }

    public virtual void Die() { }
}