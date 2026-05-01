using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
   private SO_GenericItem _item;
   private bool _playerInRange;

    public void SetUp(SO_GenericItem item)
    {
        _item = item;
    }
    private void Update()
    {
        if (!_playerInRange) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;
        InventoryManager.Instance.AddItem(_item);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }
}
