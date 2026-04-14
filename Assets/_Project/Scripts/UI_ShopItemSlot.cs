using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopItemSlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;

    private SO_GenericItem _item;
    private Action<SO_GenericItem> _onSelect;

    public void Setup(SO_GenericItem item, Action<SO_GenericItem> onSelect)
    {
        _onSelect = onSelect;
        _item = item;
        _image.sprite = item.Icon;
        _nameText.SetText(item.Name);
        _priceText.SetText(item.BuyPrice.ToString());
    }

    public void OnClick()
    {
        _onSelect?.Invoke(_item);
    }
}
