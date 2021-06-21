using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class PagePause : PageView
{
    public Button ResumeButton;
    public Button RestartButton;
    public Button QuitButton;

    public GameObject GameOverObj;

    private Vector3 mRestartStartPos;
    private Vector3 mRestartEndPos;
    private Vector3 mQuitStartPos;
    private Vector3 mQuitEndPos;

    public override UIPageKind GetPageKind()
    {
        return UIPageKind.Page_Pause;
    }

    public override void OnPostEnable()
    {
        if (GameController.Instance.CurrentState == GameState.PAUSE)
        {
            Time.timeScale = 0f;
            PageSystem.Instance.FadeInOutSystem.FadeOut(0.7f, 1000f);

            GameOverObj.SetActive(false);
            ResumeButton.gameObject.SetActive(true);
            RestartButton.gameObject.SetActive(true);
            QuitButton.gameObject.SetActive(true);

            StartCoroutine(Co_PauseAnimation());
        }
        else if (GameController.Instance.CurrentState == GameState.OVER)
        {
            ResumeButton.gameObject.SetActive(false);
            RestartButton.gameObject.SetActive(false);
            QuitButton.gameObject.SetActive(false);
            PlatformHelper.ReportScore(GameController.Instance.Player.Data.KillScore);

            StartCoroutine(Co_GameOverAnimation());
        }
    }

    public override void OnPreDisable()
    {
        Time.timeScale = 1f;

        PageSystem.Instance.FadeInOutSystem.End();
    }

    void Awake()
    {
        ESCHandler.SetESCHandler(gameObject, HandleEsc);

        ResumeButton.onClick.AddListener(OnClickResumeButton);
        RestartButton.onClick.AddListener(OnClickRestartButton);
        QuitButton.onClick.AddListener(OnClickQuitButton);

        Canvas canvas = UICanvas.Instance.GetComponent<Canvas>();

        float restartStartX = RestartButton.transform.position.x - RestartButton.image.rectTransform.rect.width * canvas.scaleFactor;
        mRestartStartPos = new Vector3(restartStartX, RestartButton.transform.position.y, 0);
        mRestartEndPos = new Vector3(RestartButton.transform.position.x, RestartButton.transform.position.y, 0);

        float quitStartX = QuitButton.transform.position.x + QuitButton.image.rectTransform.rect.width * canvas.scaleFactor;
        mQuitStartPos = new Vector3(quitStartX, QuitButton.transform.position.y, 0);
        mQuitEndPos = new Vector3(QuitButton.transform.position.x, QuitButton.transform.position.y, 0);
    }

    private IEnumerator Co_PauseAnimation()
    {
        Transform restartTm = RestartButton.transform;
        Transform quitTm = QuitButton.transform;

        restartTm.position = mRestartStartPos;
        quitTm.position = mQuitStartPos;

        while (true)
        {
            restartTm.position = Vector3.Lerp(restartTm.position, mRestartEndPos, 0.1f);
            quitTm.position = Vector3.Lerp(quitTm.transform.position, mQuitEndPos, 0.1f);

            if (restartTm.position.x >= mRestartEndPos.x - 0.1f)
                break;

            yield return null;
        }
    }

    private IEnumerator Co_ResumeAnimation()
    {
        Transform restartTm = RestartButton.transform;
        Transform quitTm = QuitButton.transform;

        restartTm.position = mRestartEndPos;
        quitTm.position = mQuitEndPos;

        while (true)
        {
            restartTm.position = Vector3.Lerp(restartTm.position, mRestartStartPos, 0.1f);
            quitTm.position = Vector3.Lerp(quitTm.transform.position, mQuitStartPos, 0.1f);

            if (restartTm.position.x >= mRestartStartPos.x + 0.1f)
                break;

            yield return null;
        }

        SceneSwitchManager.Instance.PopAdditivePage(UIPageKind.Page_Pause);
    }

    private IEnumerator Co_GameOverAnimation()
    {
        yield return new WaitForSeconds(2.5f);

        PageSystem.Instance.FadeInOutSystem.FadeOut(0.8f, 1f);

        yield return new WaitForSeconds(1f);

        GameOverObj.SetActive(true);

        yield return new WaitForSeconds(1f);

        yield return Co_PauseAnimation();
    }

    private bool HandleEsc()
    {
        GameState gameState = GameController.Instance.CurrentState;

        if (gameState == GameState.OVER ||
            gameState == GameState.QUIT)
        {
            OnClickQuitButton();
        }
        else
        {
            OnClickResumeButton();
        }

        return true;
    }

    private void OnClickResumeButton()
    {
        GameController.Instance.ReturnToPrevState();

        StartCoroutine(Co_ResumeAnimation());
    }

    private void OnClickRestartButton()
    {
        // game reset.
    }

    private void OnClickQuitButton()
    {
        StartCoroutine(GameController.Instance.Co_ReturnToTitle());
    }
}
