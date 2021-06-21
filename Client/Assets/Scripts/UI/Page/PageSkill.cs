using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class PageSkill : PageView
{
    public Button[] SkillButtons;
    public Image[] SkillImages;
    public Text[] SkillTexts;

    private SkillBase[] mSkills;

    public override UIPageKind GetPageKind()
    {
        return UIPageKind.Page_Skill;
    }

    public override void OnPostEnable()
    {
        mSkills = SkillManager.Instance.GetLevelUpSkills();

        for (int i = 0; i < mSkills.Length; i++)
        {
            SkillImages[i].sprite = mSkills[i].Sprite;
            SkillTexts[i].text = mSkills[i].ToString();
        }
    }

    public override void OnPostDisable()
    {
        PageSystem.Instance.FadeInOutSystem.FadeIn(1f, true);
    }

    void Awake()
    {
        SkillButtons[0].onClick.AddListener(() => OnClickSkillButton(0));
        SkillButtons[1].onClick.AddListener(() => OnClickSkillButton(1));
        SkillButtons[2].onClick.AddListener(() => OnClickSkillButton(2));
    }

    private void OnClickSkillButton(int index)
    {
        GameController.Instance.ReturnToPrevState();

        mSkills[index].LevelUp();

        SceneSwitchManager.Instance.PopAdditivePage(UIPageKind.Page_Skill);
    }
}
