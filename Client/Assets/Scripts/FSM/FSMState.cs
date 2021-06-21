public abstract class FSMState<T> where T : FSMSystem<T>
{
    public abstract int GetID();
    public abstract void OnEnter(T fsm, FSMState<T> prevState);
    public abstract void OnUpdate(T fsm);
    public abstract void OnExit(T fsm, FSMState<T> nextState);
}
