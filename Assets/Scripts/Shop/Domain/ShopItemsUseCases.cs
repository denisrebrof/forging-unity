using System;
using System.Collections.Generic;
using GameLevels.Domain;
using Zenject;

namespace Shop.Domain
{
    public class ShopItemsUseCases
    {
        [Inject]
        private IShopItemsRepository shopItemsRepository;
        
        public event Action UpdateSelected;
        
        public ShopItem GetShopItem(int id) => shopItemsRepository.GetShopItem(id);

        public List<ShopItem> GetShopItemsPaged(int page, int pageSize) =>
            shopItemsRepository.GetShopItemsPaged(page, pageSize);

        public List<ShopItem> GetCurrentSelectedShopItems() => shopItemsRepository.GetCurrentSelectedShopItems();

        public int GetShopItemsCount() => shopItemsRepository.GetShopItemsCount();

        public ShopItemStatus TryToBuyShopItem(long id)
        {
           var status = shopItemsRepository.TryToBuyShopItem(id);

           if(status == ShopItemStatus.AlreadyBought)
               UpdateSelected?.Invoke();
           
           return status;
        }
    }
}