using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : PickUp
{
    [SerializeField] private int _value = 1;
    protected override void OnPick(GameObject player)
    {
        base.OnPick(player);
        CoinManager.Instance.AddCoin(_value);
    }
}
