using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;

public class PageBattleZone : PageView
{
    public Button PauseButton;
    public Slider Hp;
    public Slider HpBG;
    public Slider Xp;
    public GameObject BossHpObj;
    public Slider BossHp;
    public Slider BossHpBG;
    public Text Score;
    public Text ScoreBG;
    public GameObject GameTimerObj;
    public Text GameTimer;
    public Text GameTimerBG;
    public Text Lv;
    public Text LvBG;

    public Joystick Joystick;


    public override UIPageKind GetPageKind()
    {
        return UIPageKind.Page_BattleZone;
    }

    public override void OnPreEnable()
    {
        PauseButton.gameObject.SetActive(true);
    }

    public override void OnChangePage(UIPageKind pageKind)
    {
        if (pageKind != UIPageKind.Page_Pause)
            return;

        if (SceneSwitchManager.Instance.GetCurrentAdditivePageKind() == UIPageKind.Page_Pause)
            PauseButton.gameObject.SetActive(false);
        else
            PauseButton.gameObject.SetActive(true);
    }

    void Awake()
    {
        ESCHandler.SetESCHandler(gameObject, HandleEsc);

        PauseButton.onClick.AddListener(OnClickPauseButton);
    }

    void Update()
    {
        PlayerData player = GameController.Instance.Player.Data;

        Hp.value = player.HpPercent;
        HpBG.value = Mathf.Lerp(HpBG.value, Hp.value, 0.05f);
        Xp.value = player.XpPercent;

        Lv.text = string.Format("{0}", player.Level);
        LvBG.text = string.Format("{0}", player.Level);

        string tempStr = string.Format("{0}", player.KillScore);

        Score.text = tempStr;
        ScoreBG.text = tempStr;

        GameState gameState = GameController.Instance.CurrentState;

        if (gameState == GameState.BOSS)
        {
            BossHp.value = GameController.Instance.Boss.HpPercent;
            BossHpBG.value = Mathf.Lerp(BossHpBG.value, BossHp.value, 0.05f);
        }
        else
        {
            tempStr = string.Format("{0:00.00}", GameController.Instance.GameTime);
            GameTimer.text = tempStr;
            GameTimerBG.text = tempStr;
        }
    }

    public void StartBoss()
    {
        BossHpObj.SetActive(true);
        GameTimerObj.SetActive(false);
    }

    public void EndBoss()
    {
        GameTimer.color = Color.green;

        BossHpObj.SetActive(false);
        GameTimerObj.SetActive(true);
    }

    public void TimerAnimation(Action callback)
    {
        StartCoroutine(Co_TimerAnimation(callback));
    }

    private IEnumerator Co_TimerAnimation(Action callback)
    {
        while (GameTimer.color.r < 0.99f)
        {
            float r = Mathf.Lerp(GameTimer.color.r, 1.0f, 0.05f);
            float g = Mathf.Lerp(GameTimer.color.g, 0, 0.05f);
            float b = Mathf.Lerp(GameTimer.color.b, 0, 0.05f);

            GameTimer.color = new Vector4(r, g, b, GameTimer.color.a);

            yield return null;
        }

        callback();
    }

    private bool HandleEsc()
    {
        OnClickPauseButton();

        return true;
    }

    private void OnClickPauseButton()
    {
        GameController.Instance.ChangeState(GameState.PAUSE);

        SceneSwitchManager.Instance.PushAdditivePage(UIPageKind.Page_Pause, null);
    }
}
