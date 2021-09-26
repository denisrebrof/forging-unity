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
        [Header("Params")] 
        public int defaultPoolSize = 3;
        public float dragMargin = 0f;
        public float dragMultiplier = 1f;
        public float easing = 0f;
        
        private Rect pagerRect;
        private IPagerAdapter _pagerAdapter;

        private readonly Stack<RectTransform> pagesPool = new Stack<RectTransform>();
        private readonly Dictionary<int, RectTransform> pages = new Dictionary<int, RectTransform>();

        [SerializeField] private float DragPosition = 0f;
        [SerializeField] private int currentPage = 0;

        private void Start()
        {
            _pagerAdapter = GetComponent<IPagerAdapter>();
            pagerRect = GetComponent<RectTransform>().rect;
            for (var i = 0; i < defaultPoolSize; i++)
            {
                var page = CreatePage();
                pagesPool.Push(page);
            }
        }

        private IEnumerable<int> GetVisiblePages()
        {
            IEnumerable<int> pagePositions = new[] { currentPage };
            if (DragPosition < 0) pagePositions = new[] { currentPage, currentPage + 1 };
            else if (DragPosition > 0) pagePositions = new[] { currentPage - 1, currentPage };
            return pagePositions
                .Where(pageNumber => pageNumber >= 0)
                .Where(pageNumber => pageNumber < _pagerAdapter.GetPagesCount());
        }

        public void OnDrag(PointerEventData data)
        {
            StopAllCoroutines();
            var difference = data.delta.x;
            DragPosition += difference * dragMultiplier;
            RecalculatePagePositions();
        }

        public void OnEndDrag(PointerEventData data)
        {
            StartCoroutine(SmoothRevertDrag());
        }

        IEnumerator SmoothRevertDrag()
        {
            var initialDragPosition = DragPosition;
            float targetDragPosition = 0;
            if (DragPosition > pagerRect.width / 2)
                targetDragPosition = pagerRect.width;
            else if (DragPosition < -pagerRect.width / 2)
                targetDragPosition = -pagerRect.width;
            float timer = 0f;
            while (timer <= 1.0)
            {
                timer += Time.deltaTime / easing;
                var progress = Mathf.SmoothStep(0f, 1f, timer);
                DragPosition = Mathf.Lerp(initialDragPosition, targetDragPosition, timer);
                RecalculatePagePositions();
                yield return null;
            }
        }

        private void RecalculatePagePositions()
        {
            var selectedPagePosition = currentPage + (int)(-DragPosition / pagerRect.width);
            currentPage = Mathf.Clamp(selectedPagePosition, 0, _pagerAdapter.GetPagesCount() - 1);

            DragPosition %= pagerRect.width;

            if (selectedPagePosition <= 0)
                DragPosition = Mathf.Min(DragPosition, dragMargin);
            else if (selectedPagePosition >= _pagerAdapter.GetPagesCount() - 1)
                DragPosition = Mathf.Max(DragPosition, -dragMargin);

            var visiblePages = GetVisiblePages();

            pages
                .Select(pageNumberToPage => pageNumberToPage.Key)
                .Where(pageNumber => !visiblePages.Contains(pageNumber))
                .ToList()
                .ForEach(RemovePage);

            foreach (var pageNumber in visiblePages)
            {
                var pagePosition = (pageNumber - selectedPagePosition) * pagerRect.width + DragPosition;
                GetPage(pageNumber).anchoredPosition = new Vector2(pagePosition, 0);
            }
        }

        private void RemovePage(int pageNumber)
        {
            if (!pages.TryGetValue(pageNumber, out var page)) return;
            _pagerAdapter.OnRecycle(page.gameObject);
            pagesPool.Push(page);
            pages.Remove(pageNumber);
            page.gameObject.SetActive(false);
        }

        private RectTransform GetPage(int pageNumber)
        {
            if (pages.TryGetValue(pageNumber, out var page))
                return page;

            var newPage = pagesPool.Count > 0 ? pagesPool.Pop() : CreatePage();
            newPage.transform.SetParent(transform);
            newPage.gameObject.SetActive(true);
            pages[pageNumber] = newPage;
            _pagerAdapter.OnBind(newPage.gameObject, pageNumber);
            return newPage;
        }

        private RectTransform CreatePage()
        {
            var page = _pagerAdapter.CreatePage();
            page.SetParent(transform);
            page.anchoredPosition = Vector2.zero;
            page.sizeDelta = Vector2.zero;
            page.gameObject.SetActive(false);
            return page;
        }
    }
}