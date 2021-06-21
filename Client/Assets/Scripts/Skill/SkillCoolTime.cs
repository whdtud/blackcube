using UnityEngine;
using System.Collections;

public class SkillCoolTime : SkillBase {

	public SkillCoolTime(Sprite sprite)
        : base(sprite)
    {
        mMaxLevel = 3;
    }

    public override string ToString()
    {
        string text;

        if (mLevel < mMaxLevel - 1)
        {
            text = string.Format("LEVEL {0}\n\n\n스킬의 재사용 대기시간이\n감소합니다.\n\n\n재사용대기시간 : {1}% (+20%)", 
            mLevel + 1, mLevel * 20);
        }
        else
        {
			text = string.Format("LEVEL MAX\n\n\n스킬의 재사용 대기시간이\n감소합니다.\n\n\n재사용대기시간 : {0}% (+20%)", 
			mLevel * 20);
        }

        return text;
    }

    public override void LevelUp()
    {
        mLevel++;
		GameController.Instance.Player.Data.DashDelay = 3 - 3 * mLevel * 0.2f;
		GameController.Instance.Player.Data.SkillDelay = 9 - 9 * mLevel * 0.2f;
    }
}
