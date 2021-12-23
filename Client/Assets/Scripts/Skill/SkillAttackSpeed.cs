using UnityEngine;
using System.Collections;

public class SkillAttackSpeed : SkillBase
{

	public SkillAttackSpeed(Sprite sprite)
        : base(sprite)
    {
        mMaxLevel = 3;
    }

    public override string ToString()
    {
        string text;

        if (mLevel < mMaxLevel - 1)
        {
            text = string.Format("LEVEL {0}\n\n\n미사일의 연사 속도가\n빨라집니다.\n\n\n연사 속도 : {1} (-0.2)/SEC", 
            mLevel + 1, PlayerController.Instance.Data.AttackDelay);
        }
        else
        {
            text = string.Format("LEVEL MAX\n\n\n미사일의 연사 속도가\n빨라집니다.\n\n\n연사 속도 : {0} (-0.2)/SEC", 
            PlayerController.Instance.Data.AttackDelay);
        }

        return text;
    }

    public override void LevelUp()
    {
        mLevel++;
		PlayerController.Instance.Data.AttackDelay = 0.8f - mLevel * 0.2f;
    }
}
