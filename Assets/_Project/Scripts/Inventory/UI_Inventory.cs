using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private List<UI_InventorySlot> _slots;

    private void OnEnable()
    {
        _inventoryManager.OnInventoryChange += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        if (_inventoryManager.SlotCount == 0)
        {
            _group.alpha = 0f;
            return;
        }
        _group.alpha = 1f;

        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < _inventoryManager.SlotCount)
            {
                _slots[i].SetSlot(_inventoryManager.GetItem(i));
            }
            else
            {
                _slots[i].SetSlot(null);
            }
        }
    }

    private void OnDisable()
    {
        _inventoryManager.OnInventoryChange -= Refresh;
    }
}
