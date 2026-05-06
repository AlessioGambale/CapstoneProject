using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReparableCar : InteractableObject
{
    [SerializeField] private int _epCost = 4;
    [SerializeField] private SO_GenericItem _itemRequiered;
    [SerializeField] private GameObject _brokenCar;
    [SerializeField] private GameObject _repairedCar;
    private bool _repaired;

    private void Start()
    {
        if (RunManager.Instance != null && RunManager.Instance.CarRepaired)
        {
            _repaired = true;
            _brokenCar.SetActive(false);
            _repairedCar.SetActive(true);
        }
    }

    protected override void OnInteract()
    {
        if (_repaired) return;
        if (!InventoryManager.Instance.HasItem(_itemRequiered)) return;
        if (_epCost > 0 && !ExplorationManager.Instance.TrySpendEP(_epCost)) return;
        _repaired = true;
        RunManager.Instance.SetCarRepaired();
        InventoryManager.Instance.RemoveItem(_itemRequiered);
        ScreenFader.Instance.FadeInOut(() =>
        {
            _brokenCar.SetActive(false);
            _repairedCar.SetActive(true);
        });
    }
}
