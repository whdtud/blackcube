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
    public Text Score;
    public Text ScoreBG;
    public Text Lv;
    public Text LvBG;
    public UIBattleZoneTop TopArea;
    public UIGameTimer GameTimer;
    public UIBossHp BossHp;

    public Joystick Joystick;


    public override UIPageKind GetPageKind()
    {
        return UIPageKind.Page_BattleZone;
    }

    public override void OnPreEnable()
    {
        PauseButton.gameObject.SetActive(true);
        SoundManager.Instance.PlayBGM("GamePlay");
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
        PlayerData player = PlayerController.Instance.Data;

        Hp.value = player.HpPercent;
        HpBG.value = Mathf.Lerp(HpBG.value, Hp.value, 0.05f);
        Xp.value = player.XpPercent;

        Lv.text = string.Format("{0}", player.Level);
        LvBG.text = string.Format("{0}", player.Level);

        string tempStr = string.Format("{0}", player.KillScore);

        Score.text = tempStr;
        ScoreBG.text = tempStr;
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
