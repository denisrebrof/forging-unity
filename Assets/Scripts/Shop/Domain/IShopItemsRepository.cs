using Shop.Data;

namespace Shop.Domain
{
    public interface IShopItemsRepository
    {
        ShopItem GetShopItem(long id);
    }
}