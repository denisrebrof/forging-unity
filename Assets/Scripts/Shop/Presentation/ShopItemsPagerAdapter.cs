using System;
using System.Collections.Generic;
using GameLevels.Domain;
using GameLevels.Presentation;
using Pager;
using Shop.Domain;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shop.Presentation
{
    public class ShopItemsPagerAdapter : DefaultPagerAdapter<DefaultPageViewHolder>
    {
        [Inject]
        private IShopItemImageRepository shopItemImageRepository;

        [Inject]
        private ShopItemsUseCases shopItemsUseCases;

        [SerializeField] private ShopItemView shopItemPrefab;

        [SerializeField] private RectTransform selectedItemsTransform;
        
        [SerializeField] private int itemsOnPage = 16;

        private Queue<ShopItemView> shopItemsPool = new Queue<ShopItemView>();
        
        private List<ShopItemView> boughtItems = new List<ShopItemView>();

        
        private void ReturnShopItemToPool(ShopItemView item)
        {
            item.SetActive(false);
            shopItemsPool.Enqueue(item);
        }

        private ShopItemView GetShopItem()
        {
            var shopItem = shopItemsPool.Count > 0 ? shopItemsPool.Dequeue() : CreateShopItem();
            shopItem.SetActive(true);
            return shopItem;
        }

        protected override void Bind(DefaultPageViewHolder viewHolder, int pageNumber)
        {
            base.Bind(viewHolder, pageNumber);
            var items = shopItemsUseCases.GetShopItemsPaged(pageNumber, itemsOnPage);
            
            foreach (var item in items)
            {
                var shopItem = GetShopItem();
                var sprite = shopItemImageRepository.GetShopItemImage(item.ID);
                shopItem.Bind(item, shopItemsUseCases, viewHolder.RectTransform, sprite);
            }
        }

        protected override void Recycle(DefaultPageViewHolder viewHolder)
        {
            base.Recycle(viewHolder);
            foreach (var child in viewHolder.GetComponentsInChildren<ShopItemView>())
            {
                ReturnShopItemToPool(child);
            }
        }

        private void Awake()
        {
            for (int i = 0; i < itemsOnPage * 3; i++)
            {
                var shopItem = CreateShopItem();
                shopItemsPool.Enqueue(shopItem);
            }

            shopItemsUseCases.UpdateSelected += UpdateSelectedItemsView;
            UpdateSelectedItemsView();
        }

        private void UpdateSelectedItemsView()
        {
            var children = selectedItemsTransform.GetComponentsInChildren<ShopItemView>();
            
            if (children.Length != 0)
                foreach (var child in children)
                {
                    Destroy(child.gameObject);
                }
            
            boughtItems.Clear();
            
            var selectedShopItems = shopItemsUseCases.GetCurrentSelectedShopItems();
            if(selectedShopItems.Count == 0)
                return;

            foreach (var selectedItem in selectedShopItems)
            {
                var view = CreateShopItem();
                var sprite = shopItemImageRepository.GetShopItemImage(selectedItem.ID);
                view.Bind(selectedItem, shopItemsUseCases, selectedItemsTransform, sprite);
                view.GetButton().interactable = false;
                boughtItems.Add(view);
            }
        }

        private ShopItemView CreateShopItem()
        {
            return Instantiate(shopItemPrefab);
        }

        public override int GetPagesCount()
        {
            var pagesCount = shopItemsUseCases.GetShopItemsCount() / itemsOnPage;
            if (shopItemsUseCases.GetShopItemsCount() % itemsOnPage != 0)
                pagesCount += 1;
            return pagesCount;
        }
    }

    public abstract class ShopItemView : MonoBehaviour
    {
        public abstract Button GetButton();
        public abstract void SetActive(bool active);
        public abstract void Bind(ShopItem item, ShopItemsUseCases cases, Transform parent, Sprite image);
    }
}