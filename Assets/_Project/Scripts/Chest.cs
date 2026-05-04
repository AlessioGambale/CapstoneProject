using UnityEngine;

public class Chest : InteractableObject 
{
    [SerializeField] private int _epCost;
    [SerializeField] private SO_GenericItem[] _possibleLoot;
    [SerializeField] private int _minCoins;
    [SerializeField] private int _maxCoins;
    private bool _opened;

    protected override void OnInteract()
    {
        if (_opened) return;
        if (_epCost > 0 && !ExplorationManager.Instance.TrySpendEP(_epCost)) return;
        _opened = true;
        GetComponentInParent<AnimationParamHandler>()?.Open();
        GiveLoot();
    }

    private void GiveLoot()
    {
        int coins = Random.Range(_minCoins, _maxCoins + 1);
        CoinManager.Instance.AddCoin(coins);

        if (_possibleLoot.Length > 0)
        {
            SO_GenericItem item = _possibleLoot[Random.Range(0, _possibleLoot.Length)];
            InventoryManager.Instance.AddItem(item);
        }
    }
}
