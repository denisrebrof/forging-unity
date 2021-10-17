using System.Collections.Generic;
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
    }
}