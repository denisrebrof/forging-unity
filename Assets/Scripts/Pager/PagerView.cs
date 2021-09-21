using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class PagerView : MonoBehaviour, IDragHandler
{
    private RectTransform _rectTransform;
    private IPager pager;

    private Stack<RectTransform> pagesPool = new Stack<RectTransform>();
    private Dictionary<int, RectTransform> pages = new Dictionary<int, RectTransform>();

    private float DragPosition = 0f;
    private int currentPage = 0;

    public int defaultPoolSize = 3;

    private void Start()
    {
        pager = GetComponent<IPager>();
        _rectTransform = GetComponent<RectTransform>();
        for (var i = 0; i < defaultPoolSize; i++)
        {
            var page = CreatePage();
            pagesPool.Push(page);
        }
    }

    private int[] GetVisiblePages()
    {
        if (DragPosition > 0)
            return new[] { currentPage, currentPage + 1 };

        if (DragPosition < 0)
            return new[] { currentPage - 1, currentPage };

        return new[] { currentPage };
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        DragPosition += difference;
        
        currentPage += (int)(DragPosition / _rectTransform.rect.width);
        DragPosition = DragPosition % _rectTransform.rect.width;

        var visiblePages = GetVisiblePages()
            .Where(pageNumber => pageNumber > 0)
            .Where(pageNumber => pageNumber < pager.GetPagesCount());

        pages
            .Select(pageNumberToPage => pageNumberToPage.Key)
            .Where(pageNumber => !visiblePages.Contains(pageNumber))
            .ToList().ForEach(RemovePage);

        foreach (var pageNumber in visiblePages)
        {
            var pagePosition = pageNumber * _rectTransform.rect.width + DragPosition;
            GetPage(pageNumber).anchoredPosition = new Vector2(pagePosition, 0);
        }
    }

    private void RemovePage(int pageNumber)
    {
        if (pages.TryGetValue(pageNumber, out var page))
        {
            pager.OnRecycle(page.gameObject);
            pagesPool.Push(page);
            pages.Remove(pageNumber);
            page.gameObject.SetActive(false);
        }
    }

    private RectTransform GetPage(int pageNumber)
    {
        if (pages.TryGetValue(pageNumber, out var page))
            return page;

        var newPage = pagesPool.Count > 0 ? pagesPool.Pop() : CreatePage();
        newPage.transform.SetParent(transform);
        newPage.gameObject.SetActive(true);
        pager.OnBind(newPage.gameObject);
        return newPage;
    }

    private RectTransform CreatePage()
    {
        var page = pager.CreatePage();
        page.parent = transform;
        page.anchoredPosition = Vector2.zero;
        page.gameObject.SetActive(false);
        return page;
    }
}