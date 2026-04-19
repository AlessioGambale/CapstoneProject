using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTriggerZone : MonoBehaviour
{
    [SerializeField] private List<EnemyCreature> _enemies;
    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;
        Debug.Log("Trigger entrato");
        _triggered = true;
        var toCombat = other.GetComponentInChildren<PlayerToCombatTransition>();
        Debug.Log($"ToCombat trovato: {toCombat}");
        toCombat.TriggerTransition = true;

        foreach (var enemy in _enemies)
            CombatManager.Instance.RegisterEnemy(enemy);
    }
}
