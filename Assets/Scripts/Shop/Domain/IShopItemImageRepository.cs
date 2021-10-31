using UnityEngine;

namespace Shop.Domain
{
    public interface IShopItemImageRepository
    {
        Sprite GetShopItemImage(long itemID);
    }
}