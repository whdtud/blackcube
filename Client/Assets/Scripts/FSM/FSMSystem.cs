using UnityEngine;

using System.Collections.Generic;

public class FSMSystem<T> : MonoBehaviour where T : FSMSystem<T>
{
    private T mThis;

    private FSMState<T> mCurrentState;
    private Dictionary<int, FSMState<T>> mStateDic = new Dictionary<int, FSMState<T>>();

    void Awake()
    {
        mThis = this as T;
    }

    public void Reset()
    {
        mCurrentState = null;
        mStateDic.Clear();
    }

    public int GetCurrentState()
    {
        if (mCurrentState != null)
            return mCurrentState.GetID();

        return -1;
    }

    public void AddState(FSMState<T> state)
    {
        if (mStateDic.ContainsKey(state.GetID()))
            return;

        mStateDic.Add(state.GetID(), state);
    }

    protected virtual void Update()
    {
        if (mCurrentState == null)
            return;

        mCurrentState.OnUpdate(mThis);
    }

    public virtual void ChangeState(int Id)
    {
        if (mStateDic.ContainsKey(Id) == false)
            return;

        FSMState<T> beforeState = mCurrentState;
        FSMState<T> nextState = mStateDic[Id];
        mCurrentState = nextState;

        if (beforeState != null)
            beforeState.OnExit(mThis, nextState);

        mCurrentState.OnEnter(mThis, beforeState);
    }
}
