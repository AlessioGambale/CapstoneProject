using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private EnemyCreature _currentTarget;

    public void OnBaseAttack()
    {
        CombatManager.Instance.ExecuteBaseAttack(_currentTarget);
    }

    public void OnSpecialAttack()
    {
        CombatManager.Instance.ExecuteSpecialAttack(_currentTarget);
    }

    public void OnAbility()
    {
        CombatManager.Instance.ExecuteAbility(_currentTarget);
    }

    public void OnEndTurn()
    {
        TurnManager.Instance.EndPlayerTurn();
    }
}
