using UnityEngine;
using UnityEngine.UI;

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
        SoundManager.Instance.PlayBGM("Title");
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

        BGAnimator.SetBool(Defines.ANI_GAME_START, true);
        SoundManager.Instance.PlaySE(Defines.SE_START);
        
        yield return new WaitForSeconds(1.65f);

        BackgroundTiles.PlayAnimation();

        yield return new WaitForSeconds(0.05f);

        SoundManager.Instance.PlaySE(Defines.SE_EXPLOSION_2);
        
        while (BGAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        SoundManager.Instance.PauseBGM();

        yield return null;

        yield return GameController.Instance.Co_StartGame();
    }
}
