using UnityEngine;
using System.Collections;

public class SkillMaxHp : SkillBase {

	public SkillMaxHp(Sprite sprite)
        : base(sprite)
    {
        mMaxLevel = 4;
    }

    public override string ToString()
    {
        string text;

        if (mLevel < mMaxLevel - 1)
        {
            text = string.Format("LEVEL {0}\n\n\n최대 체력이\n증가합니다.\n\n\n최대 체력 : {1} (+10)", 
            mLevel + 1, GameController.Instance.Player.Data.MaxHp);
        }
        else
        {
            text = string.Format("LEVEL MAX\n\n\n최대 체력이\n증가합니다.\n\n\n최대 체력:{0} (+10)", 
            GameController.Instance.Player.Data.MaxHp);
        }

        return text;
    }

    public override void LevelUp()
    {
        mLevel++;
        GameController.Instance.Player.Data.MaxHp = 10 + mLevel * 10;
        GameController.Instance.Player.Data.Hp = GameController.Instance.Player.Data.MaxHp;
    }
}
