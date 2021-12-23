using UnityEngine;
using System.Collections;

public class SkillMoveSpeed : SkillBase
{

	public SkillMoveSpeed(Sprite sprite)
        : base(sprite)
    {
        mMaxLevel = 3;
    }

    public override string ToString()
    {
        string text;

        if (mLevel < mMaxLevel - 1)
        {
            text = string.Format("LEVEL {0}\n\n\n이동속도가\n증가합니다.\n\n\n속도 : {1} (+0.5)",
            mLevel + 1, PlayerController.Instance.Data.MoveSpeed);
        }
        else
        {
            text = string.Format("LEVEL MAX\n\n\n이동속도가\n증가합니다.\n\n\n속도 : {0} (+0.5)",
                PlayerController.Instance.Data.MoveSpeed);
        }

        return text;
    }

    public override void LevelUp()
    {
        mLevel++;
        PlayerController.Instance.Data.MoveSpeed = 1 + 0.5f * mLevel;
    }
}
