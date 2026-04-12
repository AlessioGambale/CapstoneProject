using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTest : MonoBehaviour
{
    [SerializeField] private EnemyCreature _target;

    private void Update()
    {
        if (!TurnManager.Instance.IsPlayerTurn) return;
        if (Input.GetKeyDown(KeyCode.A))
            CombatManager.Instance.ExecuteBaseAttack(_target);
        if (Input.GetKeyDown(KeyCode.S))
            CombatManager.Instance.ExecuteSpecialAttack(_target);
        if (Input.GetKeyDown(KeyCode.D))
            CombatManager.Instance.ExecuteAbility(_target);
        if (Input.GetKeyDown(KeyCode.E))
           TurnManager.Instance.EndPlayerTurn();
    }
}
