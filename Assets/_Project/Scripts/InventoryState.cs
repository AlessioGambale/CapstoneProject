using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryState
{
    [Header("References")]
    [SerializeField] GameObject _player;

    [Header("Inventory Settings")]
    [SerializeField] private int _maxSlots = 4;
    [SerializeField] private List<InventorySlotData> _inventory;

    private KeyCode[] _hotKeys;

    public event Action OnInventoryChanged;
    public int SlotCount => _inventory.Count;

    public void Setup()
    {
        _inventory = new List<InventorySlotData>();
        MapKeys();
    }

    public void MapKeys()
    {
        _hotKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
    }

    public InventorySlotData GetSlot(int index)
    {
        if (index < 0 || index >= _inventory.Count) return null;
        return _inventory[index];
    }

    public void TryToUse(int index)
    {
        if (index < 0 || index >= _inventory.Count) return;

        InventorySlotData slot = _inventory[index];

        if (slot.Item == null) return;

        slot.Item.Use(_player);

        if (slot.Item.IsConsumable)
        {
            if (slot.Item.IsStackable)
            {
                slot.Quantity--;

                if (slot.Quantity <= 0)
                {
                    _inventory.RemoveAt(index);
                }
            }
            else
            {
                _inventory.RemoveAt(index);
            }
        }

        OnInventoryChanged?.Invoke();
    }

    public int FindItem(SO_GenericItem item)
    {
        for (int i = 0; i < _inventory.Count; i++)
        {
            if (_inventory[i].Item == item) return i;
        }
        return -1;
    }

    public bool HasItem(SO_GenericItem item)
    {
        return FindItem(item) >= 0;
    }

    public void AddItem(SO_GenericItem item)
    {
        if (item.IsStackable)
        {
            int index = FindItem(item);

            if (index >= 0)
            {
                _inventory[index].Quantity++;
            }
            else
            {
                if (_inventory.Count >= _maxSlots) return;

                _inventory.Add(new InventorySlotData(item));
            }
        }
        else
        {
            if (_inventory.Count >= _maxSlots) return;

            _inventory.Add(new InventorySlotData(item));
        }

        OnInventoryChanged?.Invoke();
    }

    public void AddItems(SO_GenericItem item, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            AddItem(item);
        }
    }

    public void RemoveItem(SO_GenericItem item)
    {
        int foundIndex = FindItem(item);
        RemoveItem(foundIndex);
    }

    public void RemoveItem(int index)
    {
        if (index < 0 || index >= _inventory.Count) return;

        InventorySlotData slot = _inventory[index];
        if (slot.Item == null) return;

        if (slot.Item.IsStackable)
        {
            slot.Quantity--;

            if (slot.Quantity <= 0)
            {
                _inventory.RemoveAt(index);
            }
        }
        else
        {
            _inventory.RemoveAt(index);
        }

        OnInventoryChanged?.Invoke();
    }

    public void ClearInvetory()
    {
        _inventory.Clear();
        OnInventoryChanged?.Invoke();
    }

    public void Update()
    {
        for (int i = 0; i < _hotKeys.Length; i++)
        {
            if (i >= _inventory.Count) break;

            if (_inventory[i] != null && Input.GetKeyDown(_hotKeys[i]))
            {
                TryToUse(i);
            }
        }
    }
}
