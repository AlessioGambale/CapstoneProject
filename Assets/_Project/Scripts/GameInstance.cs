using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : GenericSingleton<GameInstance>
{
    [SerializeField] private InventoryState _inventoryState = new InventoryState();
    [SerializeField] private CoinManager _coinsState = new CoinManager();

    public InventoryState Inventory => _inventoryState;
    public CoinManager CoinsState => _coinsState;
    //public override bool ShouldBeDestroyedOnLoad { get; set; } = false;

    protected override void Awake()
    {
        base.Awake();
        _inventoryState.Setup();
    }

    private void Update()
    {
        _inventoryState.Update();
    }
}
