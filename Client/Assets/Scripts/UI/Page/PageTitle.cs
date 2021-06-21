using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

public class PageTitle : PageView {

    public Animator BGAnimator;
    public Button StartButton;
    public Button ReaderBoardButton;

    public TitleBackgroundTiles BackgroundTiles;

    public override UIPageKind GetPageKind()
    {
        return UIPageKind.Page_Title;
    }

    public override void OnPreEnable()
    {
        BackgroundTiles.Reset();
    }

    public override void OnPostEnable()
    {
        BGAnimator.Rebind();
        PageSystem.Instance.FadeInOutSystem.FadeIn();
    }

    public override void OnPreDisable()
    {
        BGAnimator.Rebind();
    }

    void Awake()
    {
        ESCHandler.SetESCHandler(gameObject, HandleEsc);

        StartButton.onClick.AddListener(OnClickStart);
        ReaderBoardButton.onClick.AddListener(PlatformHelper.ShowLeaderBoard);
    }

    private bool HandleEsc()
    {
        Application.Quit();

        return true;
    }

    private void OnClickStart()
    {
        StartCoroutine(Co_StartGameAnimation());
    }

    private IEnumerator Co_StartGameAnimation()
    {
        PageSystem.Instance.SetIgnoreTouch(true);

        BGAnimator.SetBool("Start", true);

        yield return new WaitForSeconds(1.5f);

        BackgroundTiles.PlayAnimation();

        while (BGAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        yield return GameController.Instance.Co_StartGame();
    }
}
