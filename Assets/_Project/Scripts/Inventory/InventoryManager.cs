using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : GenericSingleton<InventoryManager>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private List<SO_GenericItem> _inventory;
    [SerializeField] private int _maxSlots;

    private KeyCode[] _keyCodes;

    public event Action OnInventoryChange;

    public int SlotCount => _inventory.Count;
    public SO_Weapon CurrentWeapon => _inventory.OfType<SO_Weapon>().FirstOrDefault();
    public SO_Ability CurrentAbility => _inventory.OfType<SO_Ability>().FirstOrDefault();

    protected override void Awake()
    {
        base.Awake();
        KeyCodeMap();
    }

    private void KeyCodeMap()
    {
        _keyCodes = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };
    }

    private void Update()
    {
        for (int i = 0; i < _keyCodes.Length; i++)
        {
            if (i >= _inventory.Count) break;
            if (_inventory[i] != null && Input.GetKeyDown(_keyCodes[i]))
            {
                TryToUse(i);
            }
        }
    }

    public void TryToUse(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= _inventory.Count) return;
        if (_inventory[itemIndex] == null) return;
        _inventory[itemIndex].Use(_player);
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
        {
            if (_inventory[i] == item) return i;
        }
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
}