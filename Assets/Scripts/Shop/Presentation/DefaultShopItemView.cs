using Shop.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.Presentation
{
    public class DefaultShopItemView : ShopItemView
    {
        [SerializeField] private Text price;
        [SerializeField] private Image background;
        [SerializeField] private Text type;
        [SerializeField] private Button button;
        
        private ShopItemsUseCases shopItemsUseCases;
        private long id;
        
        public override void SetActive(bool active)
        {
            gameObject.SetActive(active);
            transform.SetParent(null);
            button.interactable = true;
        }

        public override Button GetButton()
        {
            return button;
        }

        public override void Bind(ShopItem shopItem, ShopItemsUseCases cases, Transform parent, Sprite image)
        {
            shopItemsUseCases ??= cases;
            transform.SetParent(parent);
            id = shopItem.ID;
            price.text = shopItem.Price.ToString();
            background.sprite = image;
            type.text = shopItem.Type.ToString();
            transform.localScale = Vector3.one;
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ButtonClick);
        }

        private void ButtonClick()
        {
            shopItemsUseCases.TryToBuyShopItem(id);
        }
    }
}