using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : GenericSingleton<CoinManager>
{
    [SerializeField] private int _coins;
    public event Action<int> CoinChanged;
    public int Coins => _coins;
    public void AddCoin(int value)
    {
        _coins += value;
        CoinChanged?.Invoke(_coins);
    }

    public bool Spend(int amount)
    {
        if (_coins < amount) return false;
        _coins -= amount;
        CoinChanged?.Invoke(_coins);
        return true;
    }
}