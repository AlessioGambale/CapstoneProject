using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _icon;

    public void SetSlot(SO_GenericItem item)
    {
        if (item == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        _icon.sprite = item.Icon;
    }

}
