using UnityEngine;

public interface IPager
{
    RectTransform CreatePage();
    void OnBind(GameObject page);
    void OnRecycle(GameObject page);
    int GetPagesCount();
}
