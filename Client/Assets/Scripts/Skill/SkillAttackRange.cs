using UnityEngine;
using System.Collections;

public class SkillAttackRange : SkillBase
{

	public SkillAttackRange(Sprite sprite)
        : base(sprite)
    {
        mMaxLevel = 2;
    }

    public override string ToString()
    {
        string text;

        if (mLevel < mMaxLevel - 1)
        {
            text = string.Format("LEVEL {0}\n\n\n미사일의 사정거리가\n증가합니다.\n\n\n사정거리 : {1} (+4)", 
            mLevel + 1, PlayerController.Instance.Data.AttackRange * 10);
        }
        else
        {
            text = string.Format("LEVEL MAX\n\n\n미사일의 사정거리가\n증가합니다.\n\n\n사정거리 : {0} (+4)", 
			PlayerController.Instance.Data.AttackRange * 10);
        }

        return text;
    }

    public override void LevelUp()
    {
        mLevel++;
		PlayerController.Instance.Data.AttackRange = 0.4f + mLevel * 0.4f;
    }
}
