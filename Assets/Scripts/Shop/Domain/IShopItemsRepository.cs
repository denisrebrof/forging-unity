using System.Collections.Generic;
using ForgingDomain;
using Shop.Data;

namespace Shop.Domain
{
    public interface IShopItemsRepository
    {
        ShopItem GetShopItem(long id);
        List<ShopItem> GetShopItemsPaged(int page, int pageSize);
        int GetShopItemsCount();
        ShopItemStatus TryToBuyShopItem(long id);
        List<ShopItem> GetCurrentSelectedShopItems();
        ShopItem GetSelectedShopItemByType(ShopItemType type);
        void ResolveBalanceRepository(IBalanceRepository balanceRepository);
    }
}