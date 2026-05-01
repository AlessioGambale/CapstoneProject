using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCubeEndTurn : MonoBehaviour
{
    private void OnMouseDown()
    {
        TurnManager.Instance.EndPlayerTurn();
    }
}
