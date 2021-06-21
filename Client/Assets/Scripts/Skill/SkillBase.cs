using UnityEngine;
using System.Collections;

public class SkillBase
{
    protected Sprite _sprite;
    protected int mLevel;
    protected int mMaxLevel;

    public SkillBase(Sprite sprite)
    {
		_sprite = sprite;
		mLevel = 0;
    }

    public virtual void LevelUp() {}

    public Sprite Sprite
    {
		get { return _sprite; }
		set { _sprite = value; }
    }

    public bool IsMaxLevel()
    {
        if(mLevel >= mMaxLevel) 
        {
            return true;
        }
        return false;
    }

    public void Reset()
    {
        mLevel = 0;
    }
}
