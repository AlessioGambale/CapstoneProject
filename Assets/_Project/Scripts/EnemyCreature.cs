using UnityEngine;

public class EnemyCreature : Creature , IInteractable
{
    [SerializeField] private string _zoneId;

    private void Start()
    {
        if (!string.IsNullOrEmpty(_zoneId) && RunManager.Instance != null && RunManager.Instance.IsZoneTriggered(_zoneId))
            gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        CombatManager.Instance.SelectTarget(this);
    }
    public override void Hit(float damage)
    {
        base.Hit(damage);
    }
    public override void Die()
    {
        Debug.Log("Enemy morto");
    }

    public void Interact()
    {
        CombatManager.Instance.SelectTarget(this);
    }
}
