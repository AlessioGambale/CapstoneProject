using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : GenericSingleton<InventoryManager>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private int _maxSlots = 6;
    [SerializeField] private List<SO_GenericItem> _inventory = new List<SO_GenericItem>();

    private KeyCode[] _keyCodes;

    public event Action OnInventoryChange;
    protected override bool ShouldBeDestroyedOnLoad => false;

    public int SlotCount => _inventory.Count;
    public SO_Weapon CurrentWeapon => _inventory.OfType<SO_Weapon>().FirstOrDefault();
    public SO_Ability CurrentAbility => _inventory.OfType<SO_Ability>().FirstOrDefault();

    protected override void Awake()
    {
        base.Awake();
        _keyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };
    }

    private void Update()
    {
        for (int i = 0; i < _keyCodes.Length; i++)
        {
            if (i >= _inventory.Count) break;
            if (Input.GetKeyDown(_keyCodes[i]))
                TryToUse(i);
        }
    }

    public void TryToUse(int index)
    {
        if (index < 0 || index >= _inventory.Count) return;
        if (_inventory[index] == null) return;
        _inventory[index].Use(_player);
        if (_inventory[index].IsConsumable)
            RemoveItem(index);
        OnInventoryChange?.Invoke();
    }

    public SO_GenericItem GetItem(int index)
    {
        if (index < 0 || index >= _inventory.Count) return null;
        return _inventory[index];
    }

    public int FindItem(SO_GenericItem item)
    {
        for (int i = 0; i < _inventory.Count; i++)
            if (_inventory[i] == item) return i;
        return -1;
    }

    public bool HasItem(SO_GenericItem item) => FindItem(item) >= 0;

    public void AddItem(SO_GenericItem item)
    {
        if (_inventory.Count >= _maxSlots) return;
        _inventory.Add(item);
        OnInventoryChange?.Invoke();
    }

    public void RemoveItem(SO_GenericItem item) => RemoveItem(FindItem(item));

    public void RemoveItem(int index)
    {
        if (index < 0 || index >= _inventory.Count) return;
        _inventory.RemoveAt(index);
        OnInventoryChange?.Invoke();
    }

    public IEnumerable<SO_GenericItem> GetAllItems() => _inventory;

    public void ClearInventory()
    {
        _inventory.Clear();
        OnInventoryChange?.Invoke();
    }
}