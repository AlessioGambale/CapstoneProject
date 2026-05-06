using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MerchantWindow : MonoBehaviour
{
    [Header("Shop References")]
    [SerializeField] private UI_ShopItemSlot _slotPrefab;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private Button _buyButton;
    

    [Header("Item References")]
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemBuyPriceText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private Image _itemIcon;

    private SO_Merchant _merchant;
    private SO_GenericItem _selectedItem;


    public void Setup(SO_Merchant merchant)
    {
        _merchant = merchant;

        foreach (var itemToSell in _merchant.ItemsToSell)
        {
            UI_ShopItemSlot itemSlot = Instantiate(_slotPrefab, _slotParent);
            itemSlot.Setup(itemToSell, OnSelected);
        }
    }

    public void OnSelected(SO_GenericItem item)
    {
        _selectedItem = item;
        RefreshUI();
    }

    public void OnBuyClicked()
    {
        if (_selectedItem == null) return;
        if (CoinManager.Instance.Coins < _selectedItem.BuyPrice) return;

        if (_selectedItem is SO_Weapon w2 && RandomDropManager.Instance.IsWeaponUnlocked(w2))
        {
            PopupMessage.Instance.Show("Already unlocked");
            return;
        }
        if (_selectedItem is SO_Ability a2 && RandomDropManager.Instance.IsAbilityUnlocked(a2))
        {
            PopupMessage.Instance.Show("Already unlocked");
            return;
        }

        if (RunManager.Instance.FightsWon == 0)
        {
            if (_selectedItem is SO_Weapon && RunManager.Instance.HasBoughtFirstWeapon)
            {
                PopupMessage.Instance.Show("You already bought a weapon");
                return;
            }
            if (_selectedItem is SO_Ability && RunManager.Instance.HasBoughtFirstAbility)
            {
                PopupMessage.Instance.Show("You already bought an ability");
                return;
            }
        }

        CoinManager.Instance.Spend(_selectedItem.BuyPrice);
        if (_selectedItem is SO_Weapon w)
        {
            RunManager.Instance.SetFirstWeaponBought();
            RandomDropManager.Instance.UnlockWeapon(w);
        }
        else if (_selectedItem is SO_Ability a)
        {
            RunManager.Instance.SetFirstAbilityBought();
            RandomDropManager.Instance.UnlockAbility(a);
        }
        else
        {
            InventoryManager.Instance.AddItem(_selectedItem);
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (_selectedItem == null) return;
        bool canBuy = CoinManager.Instance.Coins >= _selectedItem.BuyPrice;
        bool canSell = InventoryManager.Instance.HasItem(_selectedItem);
        _buyButton.interactable = canBuy;
        _itemNameText.SetText(_selectedItem.Name);
        _itemBuyPriceText.SetText(_selectedItem.BuyPrice.ToString());
        _itemDescriptionText.SetText(_selectedItem.Description);
        _itemIcon.sprite = _selectedItem.Icon;
    }
    private void OnEnable()
    {
        UIManager.Instance?.OpenUI();
    }

    private void OnDisable()
    {
        UIManager.Instance?.CloseUI();
    }
}

