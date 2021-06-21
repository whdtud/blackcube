using UnityEngine;

public class PageBaseData
{
    private UIPageKind mPageKind;

    public PageBaseData(UIPageKind kind)
    {
        mPageKind = kind;
    }
}

public interface IPageView
{
    UIPageKind GetPageKind();
    GameObject GetGameObject();

    void SetPageData(PageBaseData pageData);
    void OnPreEnable();
    void OnPostEnable();
    void OnPreDisable();
    void OnPostDisable();

    void OnChangePage(UIPageKind pageKind);
}

public abstract class PageView : MonoBehaviour, IPageView
{
    public abstract UIPageKind GetPageKind();

    public virtual GameObject GetGameObject()
    {
        return gameObject;
    }

    public virtual void SetPageData(PageBaseData pageData)
    {
    }

    public virtual void OnPreEnable()
    {
    }

    public virtual void OnPostEnable()
    {
    }

    public virtual void OnPreDisable()
    {
    }

    public virtual void OnPostDisable()
    {
    }

    public virtual void OnChangePage(UIPageKind pageKind)
    {
    }
}