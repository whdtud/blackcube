using UnityEngine;
using System.Collections;

public class SkillRegenHp : SkillBase {

	public SkillRegenHp(Sprite sprite)
        : base(sprite)
    {
        mMaxLevel = 4;
    }

    public override string ToString()
    {
        string text;

        if (mLevel < mMaxLevel - 1)
        {
            text = string.Format("LEVEL {0}\n\n\n체력 회복량이\n증가합니다.\n\n\n회복량 : {1} (+0.1)/SEC", 
            mLevel + 1, PlayerController.Instance.Data.RegenHp);
        }
        else
        {
			text = string.Format("LEVEL MAX\n\n\n체력 회복량이\n증가합니다.\n\n\n회복량 : {0} (+0.1)/SEC", 
            PlayerController.Instance.Data.RegenHp);
        }

        return text;
    }

    public override void LevelUp()
    {
        mLevel++;
		PlayerController.Instance.Data.RegenHp = 0 + mLevel * 0.1f;
    }
}
