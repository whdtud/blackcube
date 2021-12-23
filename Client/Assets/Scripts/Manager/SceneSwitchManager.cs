using UnityEngine;

using System;
using System.Collections.Generic;

public class SceneSwitchManager : STController<SceneSwitchManager>
{
    #region UIPageData
    public class UIPageData
    {
        public UIPageKind Kind { get; private set; }
        public IPageView PageObj { get; private set; }

        public UIPageData(UIPageKind kind, IPageView pageObj)
        {
            Kind = kind;
            PageObj = pageObj;
        }
    }
    #endregion

    #region Additive
    class AdditiveUIPageData : UIPageData
    {
        public IPageView ParentPage { get; private set; }
        public AdditiveUIPageData(UIPageKind kind, IPageView pageObj, IPageView parentPage)
            : base(kind, pageObj)
        {
            ParentPage = parentPage;
        }
    }
    #endregion

    #region ESCHandling
    private Dictionary<string, Func<bool>> mEscHandlerDic = new Dictionary<string, Func<bool>>();

    public void AddEscHandler(string SceneName, Func<bool> escFunc)
    {
        if (mEscHandlerDic.ContainsKey(SceneName))
            mEscHandlerDic.Remove(SceneName);
        mEscHandlerDic.Add(SceneName, escFunc);
    }

    public void RemoveEscHandler(string sceneName)
    {
        if (mEscHandlerDic.ContainsKey(sceneName))
            mEscHandlerDic.Remove(sceneName);
    }
    #endregion

    void Awake()
    {
    }

    void Update()
    {
        if (UIHelper.CheckBackButton())
        {
            OnPressBackButton();
        }
    }

    public void OnPressBackButton()
    {
        Func<bool> escFunc = null;
        if (mCurrentAdditivePage != null)
        {

            string additivePageName = mCurrentAdditivePage.GetGameObject().name;
            if (mEscHandlerDic.TryGetValue(additivePageName, out escFunc) == false)
            {
                PopAdditivePage();
                return;
            }

            if (escFunc() == false)
                PopAdditivePage();

            return;
        }

        string pageName = mCurrentPage.GetGameObject().name;
        if (mEscHandlerDic.TryGetValue(pageName, out escFunc) == false
            || escFunc() == false)
        {
            if (GetPageCount() > 1)
                PopPage();
        }
    }

    #region Page
    private IPageView mCurrentPage = null;
    private IPageView mCurrentAdditivePage = null;
    
    private List<UIPageData> mUIPageStackList = new List<UIPageData>();
    private List<AdditiveUIPageData> mAdditiveUIPageList = new List<AdditiveUIPageData>();

    private Dictionary<UIPageKind, IPageView> mKeepPage = new Dictionary<UIPageKind, IPageView>();

    public void AddKeepPage(IPageView page)
    {
        mKeepPage[page.GetPageKind()] = page;
    }

    public IPageView GetPage(UIPageKind kind)
    {
        if (kind == UIPageKind.None || kind == UIPageKind.MaxCount)
            return null;

        IPageView findPage;
        if (mKeepPage.TryGetValue(kind, out findPage) && findPage != null)
            return findPage;
        return null;
    }

    private void CallOnChangePage(UIPageKind pageKind)
    {
        for (int index = 0; index < mUIPageStackList.Count; ++index)
        {
            mUIPageStackList[index].PageObj.OnChangePage(pageKind);
        }

        for (int index = 0; index < mAdditiveUIPageList.Count; ++index)
        {
            mAdditiveUIPageList[index].PageObj.OnChangePage(pageKind);
        }
    }

    public void ClearAllPage()
    {
        while (mUIPageStackList.Count > 1)
        {

        }
    }

    public void ClearAndPushPage(UIPageKind kind, PageBaseData pageData)
    {
        var tempUIPageStackList = new List<UIPageData>(mUIPageStackList);
        mUIPageStackList.Clear();

        for (int i = 0; i < tempUIPageStackList.Count; i++)
        {
            if (tempUIPageStackList[i] == null || tempUIPageStackList[i].PageObj == null)
                return;

            tempUIPageStackList[i].PageObj.OnPreDisable();
        }

        for (int i = 0; i < tempUIPageStackList.Count; i++)
        {
            if (tempUIPageStackList[i] == null || tempUIPageStackList[i].PageObj == null || tempUIPageStackList[i].PageObj.GetGameObject() == null)
                return;

            tempUIPageStackList[i].PageObj.GetGameObject().SetActive(false);
        }

        for (int i = 0; i < tempUIPageStackList.Count; ++i)
        {
            if (tempUIPageStackList[i] == null || tempUIPageStackList[i].PageObj == null)
                return;

            tempUIPageStackList[i].PageObj.OnPostDisable();
        }

        mCurrentPage = null;

        PushPage(kind, pageData);
    }

    public void PushPage(UIPageKind kind, PageBaseData pageData)
    {
        if (kind == UIPageKind.None || kind == UIPageKind.MaxCount)
            return;

        if (mUIPageStackList.Count > 0 && mUIPageStackList[mUIPageStackList.Count - 1].Kind == kind)
            return;

        var pushPage = GetPage(kind);
        if (pushPage == null)
            return;

        pushPage.SetPageData(pageData);
        pushPage.OnPreEnable();

        var tempAdditiveUIPageList = new List<AdditiveUIPageData>(mAdditiveUIPageList);
        mAdditiveUIPageList.Clear();
        mCurrentAdditivePage = null;
        for (int i = tempAdditiveUIPageList.Count - 1; i >= 0; i--)
        {
            var page = tempAdditiveUIPageList[i].PageObj;
            if (page == null || page.GetGameObject() == null)
                continue;

            page.OnPreDisable();
            page.GetGameObject().SetActive(false);
        }

        var lastPage = mCurrentPage;
        if (lastPage != null && lastPage.GetGameObject() != null)
        {
            lastPage.OnPreDisable();
            lastPage.GetGameObject().SetActive(false);
        }

        mCurrentPage = pushPage;
        mUIPageStackList.Add(new UIPageData(kind, mCurrentPage));
        pushPage.GetGameObject().SetActive(true);
        pushPage.OnPostEnable();

        for (int i = tempAdditiveUIPageList.Count - 1; i >= 0; i--)
        {
            IPageView page = tempAdditiveUIPageList[i].PageObj;
            if (page == null)
                continue;

            page.OnPostDisable();
        }

        if (lastPage != null && lastPage.GetGameObject() != null)
        {
            lastPage.OnPostDisable();
        }

        CallOnChangePage(kind);
    }

    public void PopPage()
    {
        if (mUIPageStackList.Count <= 1)
            return;

        if (mCurrentPage == null || mUIPageStackList.Count == 0)
        {
            ClearAndPushPage(UIPageKind.Page_Title, null);
            return;
        }

        var prePage = mCurrentPage;
        prePage.OnPreDisable();

        var popPage = mUIPageStackList[mUIPageStackList.Count - 1];
        mUIPageStackList.Remove(popPage);
        prePage.GetGameObject().SetActive(false);

        var next = mUIPageStackList[mUIPageStackList.Count - 1].PageObj;
        mCurrentPage = next;

        if (next != null)
        {
            next.GetGameObject().SetActive(true);
        }

        prePage.OnPostDisable();

        CallOnChangePage(popPage.Kind);
    }

    public int GetPageCount()
    {
        return mUIPageStackList.Count;
    }

    public IPageView GetCurrentPage()
    {
        return mCurrentPage;
    }

    public UIPageKind GetCurrentPageKind()
    {
        if (mCurrentPage == null)
            return UIPageKind.None;

        return mCurrentPage.GetPageKind();
    }

    public UIPageKind GetCurrentAdditivePageKind()
    {
        if (mCurrentAdditivePage == null)
            return UIPageKind.None;

        return mCurrentAdditivePage.GetPageKind();
    }
    #endregion

    #region AdditivePage
    public void ClearAdditivePage(bool IsOutSideCall = true)
    {
        if (mAdditiveUIPageList.Count <= 0)
            return;

        var tempUIPageList = new List<AdditiveUIPageData>(mAdditiveUIPageList);

        mAdditiveUIPageList.Clear();
        mCurrentAdditivePage = null;

        for (int i = tempUIPageList.Count - 1; i >= 0; i--)
        {
            var page = tempUIPageList[i].PageObj;
            if (page == null)
                continue;

            page.OnPreDisable();
        }

        for (int i = tempUIPageList.Count - 1; i >= 0; i--)
        {
            var page = tempUIPageList[i].PageObj;
            if (page == null || page.GetGameObject() == null)
                continue;

            page.GetGameObject().SetActive(false);
        }

        for (int i = tempUIPageList.Count - 1; i >= 0; i--)
        {
            var page = tempUIPageList[i].PageObj;
            if (page == null)
                continue;

            page.OnPostDisable();
        }

        if (IsOutSideCall)
            CallOnChangePage(UIPageKind.None);
    }

    public void PushAdditivePage(UIPageKind kind, PageBaseData pageData)
    {
        if (kind == UIPageKind.None || kind == UIPageKind.MaxCount)
            return;

        if (mAdditiveUIPageList.Find(x => x.Kind == kind) != null)
            return;

        var page = GetPage(kind);
        if (page == null)
            return;

        page.SetPageData(pageData);
        page.OnPreEnable();

        mCurrentAdditivePage = page;
        var additivePageData = new AdditiveUIPageData(
            kind,
            mCurrentAdditivePage,
            mCurrentPage);

        mAdditiveUIPageList.Add(additivePageData);

        mCurrentAdditivePage.GetGameObject().SetActive(true);
        mCurrentAdditivePage.OnPostEnable();

        CallOnChangePage(kind);
    }

    public void PopAdditivePage(UIPageKind kind = UIPageKind.None)
    {
        if (mAdditiveUIPageList.Count <= 0)
            return;

        int popPageIndex = -1;

        if (kind != UIPageKind.None)
            popPageIndex = mAdditiveUIPageList.FindLastIndex(x => x.Kind == kind);
        else
            popPageIndex = mAdditiveUIPageList.Count - 1;

        if (popPageIndex < 0)
            return;

        var page = mAdditiveUIPageList[popPageIndex].PageObj;
        if (page == null)
            return;

        page.OnPreDisable();

        mAdditiveUIPageList.RemoveAt(popPageIndex);
        if (mAdditiveUIPageList.Count > 0)
        {
            var additivePageData = mAdditiveUIPageList[mAdditiveUIPageList.Count - 1];
            mCurrentAdditivePage = additivePageData.PageObj;
        }
        else
        {
            mCurrentAdditivePage = null;
        }

        page.GetGameObject().SetActive(false);
        page.OnPostDisable();

        CallOnChangePage(kind);
    }
    #endregion
}
