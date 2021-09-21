using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPager : MonoBehaviour, IPager
{
    [SerializeField] private GameObject pagePrefab;

    public RectTransform CreatePage()
    {
        return Instantiate(pagePrefab).GetComponent<RectTransform>();
    }

    public void OnBind(GameObject page)
    {
        page.name += "bound";
    }

    public void OnRecycle(GameObject page)
    {
        page.name += "recycled";
    }

    public int GetPagesCount()
    {
        return 5;
    }
}
