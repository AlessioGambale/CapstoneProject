using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MerchantWindow : MonoBehaviour
{
    [Header("Shop References")]
    [SerializeField] private UI_ShopItemSlot _slotPrefab;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;

    [Header("Item References")]
    [SerializeField] private TextMeshProUGUI _merchantNameText;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemSellPriceText;
    [SerializeField] private TextMeshProUGUI _itemBuyPriceText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private Image _itemIcon;

    private SO_Merchant _merchant;
    private SO_GenericItem _selectedItem;

    public void Setup(SO_Merchant merchant)
    {
        _merchant = merchant;
        _merchantNameText.SetText(_merchant.Name);

        foreach (var itemToSell in _merchant.ItemsToSell)
        {
            UI_ShopItemSlot itemSlot = Instantiate(_slotPrefab, _slotParent);
            itemSlot.Setup(itemToSell, OnSelected);
        }
        gameObject.SetActive(true);
    }

    public void OnSelected(SO_GenericItem item)
    {
        _selectedItem = item;
        RefreshUI();
    }

    public void OnBuyClicked()
    {
        if (_selectedItem == null) return;

        if (CoinManager.Instance.Coins >= _selectedItem.BuyPrice)
        {
            CoinManager.Instance.Spend(_selectedItem.BuyPrice);
            GameInstance.Instance.Inventory.AddItem(_selectedItem);
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (_selectedItem == null) return;
        bool canBuy = CoinManager.Instance.Coins >= _selectedItem.BuyPrice;
        bool canSell = GameInstance.Instance.Inventory.HasItem(_selectedItem);

        _buyButton.interactable = canBuy;
        _sellButton.interactable = canSell;

        _itemNameText.SetText(_selectedItem.Name);
        _itemSellPriceText.SetText(_selectedItem.SellPrice.ToString());
        _itemBuyPriceText.SetText(_selectedItem.BuyPrice.ToString());
        _itemDescriptionText.SetText(_selectedItem.Description);
        _itemIcon.sprite = _selectedItem.Icon;
    }
}

