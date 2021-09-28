using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pager
{
    [RequireComponent(typeof(RectTransform))]
    public class PagerView : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [Header("Params")] public int defaultPoolSize = 3;
        public float dragMargin = 0f;
        public float dragMultiplier = 1f;
        public float easing = 0f;

        private Rect _pagerRect;
        private IPagerAdapter _pagerAdapter;

        private readonly Stack<IPageViewHolder> _pagesPool = new Stack<IPageViewHolder>();
        private readonly Dictionary<int, IPageViewHolder> _pages = new Dictionary<int, IPageViewHolder>();

        [SerializeField] private float dragPosition = 0f;
        [SerializeField] private int currentPage = 0;

        private void Start()
        {
            _pagerAdapter = GetComponent<IPagerAdapter>();
            _pagerRect = GetComponent<RectTransform>().rect;
            for (var i = 0; i < defaultPoolSize; i++)
            {
                var page = CreatePage();
                _pagesPool.Push(page);
            }
        }

        private IEnumerable<int> GetVisiblePages()
        {
            IEnumerable<int> pagePositions = new[] { currentPage };
            if (dragPosition < 0) pagePositions = new[] { currentPage, currentPage + 1 };
            else if (dragPosition > 0) pagePositions = new[] { currentPage - 1, currentPage };
            return pagePositions
                .Where(pageNumber => pageNumber >= 0)
                .Where(pageNumber => pageNumber < _pagerAdapter.GetPagesCount());
        }

        public void OnDrag(PointerEventData data)
        {
            StopAllCoroutines();
            var difference = data.delta.x;
            dragPosition += difference * dragMultiplier;
            RecalculatePagePositions();
        }

        public void OnEndDrag(PointerEventData data) => StartCoroutine(SmoothRevertDrag());

        IEnumerator SmoothRevertDrag()
        {
            var initialDragPosition = dragPosition;
            float targetDragPosition = 0;
            if (dragPosition > _pagerRect.width / 2)
                targetDragPosition = _pagerRect.width;
            else if (dragPosition < -_pagerRect.width / 2)
                targetDragPosition = -_pagerRect.width;
            float timer = 0f;
            while (timer <= 1.0)
            {
                timer += Time.deltaTime / easing;
                var progress = Mathf.SmoothStep(0f, 1f, timer);
                dragPosition = Mathf.Lerp(initialDragPosition, targetDragPosition, timer);
                RecalculatePagePositions();
                yield return null;
            }
        }

        private void RecalculatePagePositions()
        {
            var selectedPagePosition = currentPage + (int)(-dragPosition / _pagerRect.width);
            currentPage = Mathf.Clamp(selectedPagePosition, 0, _pagerAdapter.GetPagesCount() - 1);

            dragPosition %= _pagerRect.width;

            if (selectedPagePosition <= 0)
                dragPosition = Mathf.Min(dragPosition, dragMargin);
            else if (selectedPagePosition >= _pagerAdapter.GetPagesCount() - 1)
                dragPosition = Mathf.Max(dragPosition, -dragMargin);

            var visiblePages = GetVisiblePages();

            _pages
                .Select(pageNumberToPage => pageNumberToPage.Key)
                .Where(pageNumber => !visiblePages.Contains(pageNumber))
                .ToList()
                .ForEach(RemovePage);

            foreach (var pageNumber in visiblePages)
            {
                var pagePosition = (pageNumber - selectedPagePosition) * _pagerRect.width + dragPosition;
                GetPage(pageNumber).RectTransform.anchoredPosition = new Vector2(pagePosition, 0);
            }
        }

        private void RemovePage(int pageNumber)
        {
            if (!_pages.TryGetValue(pageNumber, out var page)) return;
            _pagerAdapter.OnRecycle(page);
            _pagesPool.Push(page);
            _pages.Remove(pageNumber);
        }

        private IPageViewHolder GetPage(int pageNumber)
        {
            if (_pages.TryGetValue(pageNumber, out var page))
                return page;

            var newPage = _pagesPool.Count > 0 ? _pagesPool.Pop() : CreatePage();
            newPage.RectTransform.SetParent(transform);
            _pages[pageNumber] = newPage;
            _pagerAdapter.OnBind(newPage, pageNumber);
            return newPage;
        }

        private IPageViewHolder CreatePage()
        {
            var page = _pagerAdapter.CreatePage();
            var pageTransform = page.RectTransform;
            pageTransform.SetParent(transform);
            pageTransform.anchoredPosition = Vector2.zero;
            pageTransform.sizeDelta = Vector2.zero;
            return page;
        }
    }
}