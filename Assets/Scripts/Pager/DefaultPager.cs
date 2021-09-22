using UnityEngine;
using UnityEngine.UI;

namespace Pager
{
    public class DefaultPager : MonoBehaviour, IPager
    {
        [SerializeField] private GameObject pagePrefab;

        public RectTransform CreatePage()
        {
            var page = Instantiate(pagePrefab);
            page.AddComponent<PageView>().state = "created";
            page.GetComponentInChildren<Text>().text = "C";
            return page.GetComponent<RectTransform>();
        }

        public void OnBind(GameObject page, int pageNumber)
        {
            var pageView = page.GetComponent<PageView>();
            pageView.state = "bound";
            page.GetComponentInChildren<Text>().text = pageNumber.ToString();
            pageView.pageNumber = pageNumber;
        }

        public void OnRecycle(GameObject page)
        {
            page.GetComponent<PageView>().state = "recycled";
            page.GetComponentInChildren<Text>().text = "R";
        }

        public int GetPagesCount()
        {
            return 5;
        }
    }
}