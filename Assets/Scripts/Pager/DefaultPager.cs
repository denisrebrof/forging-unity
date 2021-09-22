using UnityEngine;

public class DefaultPager : MonoBehaviour, IPager
{
    [SerializeField] private GameObject pagePrefab;

    public RectTransform CreatePage()
    {
        var page = Instantiate(pagePrefab);
        page.AddComponent<PageView>().state = "created";
        return page.GetComponent<RectTransform>();
    }

    public void OnBind(GameObject page, int pageNumber)
    {
        var pageView = page.GetComponent<PageView>();
        pageView.state = "bound";
        pageView.pageNumber = pageNumber;
    }

    public void OnRecycle(GameObject page)
    {
        page.GetComponent<PageView>().state = "recycled";
    }

    public int GetPagesCount()
    {
        return 5;
    }
}