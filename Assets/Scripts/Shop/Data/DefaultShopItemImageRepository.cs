using Shop.Domain;
using UnityEngine;

namespace Shop.Data
{
    [CreateAssetMenu(fileName = "DefaultShopItemImageRepository")]
    public class DefaultShopItemImageRepository : ScriptableObject, IShopItemImageRepository
    {
        public Sprite TestImage;
        
        public Sprite GetShopItemImage(long itemID)
        {
            return TestImage;
        }
    }
}