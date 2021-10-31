using System;
using System.Collections.Generic;
using ForgingDomain;
using Shop.Domain;
using UnityEngine;
using Zenject;

namespace Shop.Data
{
    [CreateAssetMenu(fileName = "ShopItemsHardcodedRepository")]
    public class ShopItemsHardcodedRepository : ScriptableObject, IShopItemsRepository
    {

        [SerializeField] private List<ShopItem> ShopItems;
        
        private Dictionary<ShopItemType, ShopItem> selectedItems = new Dictionary<ShopItemType, ShopItem>();

        [Inject]
        private IBalanceRepository balanceRepository;
        
        public ShopItem GetShopItem(long id)
        {
            return ShopItems.Find(item => item.ID == id);
        }

        public List<ShopItem> GetShopItemsPaged(int page, int pageSize)
        {
            var startIndex = page * pageSize;
            if (startIndex >= ShopItems.Count)
                return new List<ShopItem>();

            var size = Mathf.Min(ShopItems.Count - startIndex, pageSize);
            return ShopItems.GetRange(startIndex, size);
        }

        public int GetShopItemsCount()
        {
            return ShopItems.Count;
        }

        public ShopItemStatus TryToBuyShopItem(long id)
        {
            var selectedItem = GetShopItem(id);
            
            if (balanceRepository == null || selectedItem == null)
                return ShopItemStatus.Error;

            var currentBalance = balanceRepository.GetBalance();

            if (selectedItem.Bought)
            {
                UpdateOrAddSelectedItem(selectedItem);
                return ShopItemStatus.AlreadyBought;
            }
            
            if (selectedItem.Price > currentBalance)
                return ShopItemStatus.NotEnoughMoney;

            balanceRepository.AddBalance(-selectedItem.Price);
            selectedItem.Bought = true;
            return ShopItemStatus.Success;
        }

        private void UpdateOrAddSelectedItem(ShopItem item)
        {
            if (!selectedItems.ContainsKey(item.Type))
            {
                selectedItems.Add(item.Type, item);
                return;
            }
            
            selectedItems[item.Type] = item;
        }

        public List<ShopItem> GetCurrentSelectedShopItems()
        {
            List<ShopItem> toReturn = new List<ShopItem>();
            foreach (var type in (ShopItemType[]) Enum.GetValues(typeof(ShopItemType)))
            {
                if(!selectedItems.TryGetValue(type, out var item))
                    continue;
                
                toReturn.Add(item);
            }

            return toReturn;
        }

        public ShopItem GetSelectedShopItemByType(ShopItemType type)
        {
            return !selectedItems.TryGetValue(type, out var item) ? null : item;
        }
    }
}