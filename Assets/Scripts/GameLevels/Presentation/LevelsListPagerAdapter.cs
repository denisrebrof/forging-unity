using System.Collections.Generic;
using GameLevels.Domain;
using Pager;
using UnityEngine;
using Zenject;

namespace GameLevels.Presentation
{
    public class LevelsListPagerAdapter : DefaultPagerAdapter<DefaultPageViewHolder>
    {
        [Inject] private GameLevelsUseCases levelsUseCases;

        [Inject] private ILevelItemImageRepository imageRepository;

        [SerializeField] private LevelItemView levelPrefab;

        [SerializeField] private int levelsOnPage = 16;

        private Queue<LevelItemView> levelItemsPool = new Queue<LevelItemView>();

        private void Awake()
        {
            for (var i = 0; i < levelsOnPage * 3; i++)
            {
                var levelItem = CreateLevelItem();
                levelItemsPool.Enqueue(levelItem);
            }
        }

        private void ReturnLevelItemToPool(LevelItemView item)
        {
            item.SetActive(false);
            levelItemsPool.Enqueue(item);
        }

        private LevelItemView GetLevelItem()
        {
            var levelItem = levelItemsPool.Count > 0 ? levelItemsPool.Dequeue() : CreateLevelItem();
            levelItem.SetActive(true);
            return levelItem;
        }

        private LevelItemView CreateLevelItem()
        {
            return Instantiate(levelPrefab);
        }

        public override int GetPagesCount()
        {
            var pagesCount = levelsUseCases.GetLevelsCount() / levelsOnPage;
            if (levelsUseCases.GetLevelsCount() % levelsOnPage != 0)
                pagesCount += 1;
            return pagesCount;
        }

        protected override void Bind(DefaultPageViewHolder viewHolder, int pageNumber)
        {
            base.Bind(viewHolder, pageNumber);
            var levels = levelsUseCases.GetLevelsPaged(pageNumber, levelsOnPage);
            foreach (var level in levels)
            {
                var levelItem = GetLevelItem();
                var sprite = imageRepository.GetSpriteForLevel(level.ID);
                levelItem.Bind(level.Number, sprite, level.Completed, viewHolder.RectTransform, level.ID);
            }

            viewHolder.RectTransform.sizeDelta = Vector2.zero;
            viewHolder.RectTransform.localScale = Vector3.one;
        }

        protected override void Recycle(DefaultPageViewHolder viewHolder)
        {
            base.Recycle(viewHolder);
            foreach (var child in viewHolder.GetComponentsInChildren<LevelItemView>())
            {
                ReturnLevelItemToPool(child);
            }
        }

        public abstract class LevelItemView : MonoBehaviour
        {
            public abstract void SetActive(bool active);

            public abstract void Bind(int levelNumber, Sprite preview, bool completedState, Transform parent,
                long levelId);
        }
    }
}