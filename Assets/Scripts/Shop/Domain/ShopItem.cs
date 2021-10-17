using System;

namespace Shop.Domain
{
    [Serializable]
    public class ShopItem
    {
        public ShopItemType Type;
        public long ID;
        public int Number;
        public int Price;
        public bool Bought;
        public string ImageUri;
        public string ShopItemUri;
    }
}