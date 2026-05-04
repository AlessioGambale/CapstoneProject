using System;
using UnityEngine;

public enum ExplorationAction
{
    None = 0,
    OpenChest = 2 , 
    RepairCar = 5 ,
    OpenSpecialChest = 3,
}

public class ExplorationManager : GenericSingleton<ExplorationManager>
{
    [SerializeField] private int _maxEP = 6;
    private int _currentEP;

    public event Action<int> OnEPChanged;
    public int MaxEP => _maxEP;
    public int CurrentEP => _currentEP; 

    protected override void Awake()
    {
        base.Awake();
        _currentEP = _maxEP;
    }

    public bool TrySpendEP(int cost)
    {
        if (_currentEP < cost ) return false;
        _currentEP -= cost;
        OnEPChanged?.Invoke( _currentEP );
        return true;
    }

    public void ResetEP(int amount)
    {
        _currentEP = Mathf.Clamp(_currentEP + amount, 0, _maxEP);
        OnEPChanged?.Invoke( _currentEP );
    }
}
