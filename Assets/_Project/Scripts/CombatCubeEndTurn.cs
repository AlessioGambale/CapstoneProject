using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCubeEndTurn : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        TurnManager.Instance.EndPlayerTurn();
    }

}
