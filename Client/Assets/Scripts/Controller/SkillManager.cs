using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
	private Dictionary<string, SkillBase> mSkillMap = new Dictionary<string, SkillBase>();
	private List<SkillBase> mSkillList = new List<SkillBase>();

	public const int SKILL_DISPLAY_COUNT = 3;

	public static SkillManager Instance { get; private set; }

	void Awake()
    {
		if (Instance != null)
			return;

		Instance = this;
    }

	void Start()
	{	
		Sprite[] sprites = Resources.LoadAll<Sprite>(@"Sprites/ui_sheet");
		
		SkillBase newSkill = new SkillAttackPower(sprites[0]);
		mSkillMap.Add("AttackPower", newSkill);
		mSkillList.Add(newSkill);
		
		newSkill = new SkillAttackSpeed(sprites[1]);
		mSkillMap.Add("AttackSpeed", newSkill);
		mSkillList.Add(newSkill);

		newSkill = new SkillAttackRange(sprites[2]);
		mSkillMap.Add("AttackRange", newSkill);
		mSkillList.Add(newSkill);

		newSkill = new SkillLaserCharge(sprites[4]);
		mSkillMap.Add("LaserCharge", newSkill);
		mSkillList.Add(newSkill);

		newSkill = new SkillLaserRecoilless(sprites[5]);
		mSkillMap.Add("LaserRecoilless", newSkill);
		mSkillList.Add(newSkill);

		newSkill = new SkillCoolTime(sprites[7]);
		mSkillMap.Add("Cooltime", newSkill);
		mSkillList.Add(newSkill);
		
		newSkill = new SkillMoveSpeed(sprites[8]);
		mSkillMap.Add("MoveSpeed", newSkill);
		mSkillList.Add(newSkill);

		newSkill = new SkillMaxHp(sprites[9]);
		mSkillMap.Add("MaxHp", newSkill);
		mSkillList.Add(newSkill);

		newSkill = new SkillRegenHp(sprites[10]);
		mSkillMap.Add("RegenHp", newSkill);
		mSkillList.Add(newSkill);
	}
	
	public SkillBase[] GetLevelUpSkills()
	{
		int ran;
		int[] temp = new int[SKILL_DISPLAY_COUNT];
		SkillBase[] skills = new SkillBase[SKILL_DISPLAY_COUNT];
		
		for (int i = 0; i < SKILL_DISPLAY_COUNT; i++)
		{
			ran = Random.Range(0, mSkillList.Count);
			temp[i] = ran;
			skills[i] = mSkillList[ran];
			
            // 최대 레벨 스킬은 제외
            if (skills[i].IsMaxLevel())
            {
                i--;
                continue;
            }

			// 중복 체크
			for (int j = 0; j < i; j++)
			{
				if (temp[i] == temp[j])
				{
					i--;
					break;
				}
			}
		}
		return skills;
	}
	
	public void LevelUpSkill(string name)
	{
		mSkillMap[name].LevelUp();
	}

    public void Reset()
    {
        foreach (SkillBase skill in mSkillList)
        {
            skill.Reset();
        }
    }
}
