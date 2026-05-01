using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : GenericSingleton<GameInstance>
{
    [SerializeField] private CoinManager _coinsState = new CoinManager();
    public CoinManager CoinsState => _coinsState;

    protected override void Awake()
    {
        base.Awake();
    }
}
