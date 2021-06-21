using UnityEngine;
using System.Collections;

public class SkillAttackPower : SkillBase
{
    public SkillAttackPower(Sprite sprite)
        : base(sprite)
    {
        mMaxLevel = 4;
    }

    public override string ToString()
    {
        string text;

        if (mLevel < mMaxLevel - 1)
        {
            text = string.Format("LEVEL {0}\n\n\n미사일의 발사 수가\n증가합니다.\n\n\n발사 수 : {1} (+1)",
            mLevel + 1, GameController.Instance.Player.Data.AttackPower);
        }
        else
        {
            text = string.Format("LEVEL MAX\n\n\n미사일의 발사 수가\n증가합니다.\n\n\n발사 수 : {0} (+1)",
            GameController.Instance.Player.Data.AttackPower);
        }

        return text;
    }

    public override void LevelUp()
    {
        mLevel++;
        GameController.Instance.Player.Data.AttackPower = 1 + mLevel;
    }
}
