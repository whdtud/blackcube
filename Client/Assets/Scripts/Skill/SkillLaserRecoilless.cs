using UnityEngine;
using System.Collections;

public class SkillLaserRecoilless : SkillBase {

	public SkillLaserRecoilless(Sprite sprite)
		: base(sprite)
	{
		mMaxLevel = 1;
	}
	
	public override string ToString()
	{
		string text;
		
		text = string.Format("LEVEL MAX \n\n\n레이져 사용 시 발생하는\n반동이 사라집니다.\n\n\n반동량 : 10 (-10)");
		
		return text;
	}
	
	public override void LevelUp()
	{
		mLevel++;
		PlayerController.Instance.Data.LaserRecoilless = true;
	}
}
