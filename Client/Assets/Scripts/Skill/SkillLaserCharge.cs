using UnityEngine;
using System.Collections;

public class SkillLaserCharge : SkillBase {

	public SkillLaserCharge(Sprite sprite)
        : base(sprite)
    {
        mMaxLevel = 4;
    }

    public override string ToString()
    {
        string text;

        if (mLevel < mMaxLevel - 1)
        {
			text = string.Format("LEVEL {0}\n\n\n레이져의 최대 충전량이\n증가합니다.\n\n\n충전량 : {1} (+25)", 
            mLevel + 1, GameController.Instance.Player.Data.LaserCharge * 100);
        }
        else
        {
			text = string.Format("LEVEL MAX\n\n\n레이져의 최대 충전량이\n증가합니다.\n\n\n충전량 : {0} (+25)", 
			GameController.Instance.Player.Data.LaserCharge * 100);
        }

        return text;
    }

    public override void LevelUp()
    {
        mLevel++;
		GameController.Instance.Player.Data.LaserCharge = 0.5f + mLevel * 0.25f;
    }
}
