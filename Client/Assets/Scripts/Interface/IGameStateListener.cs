public interface IGameStateListener
{
    public void OnChangeState(GameState prevState, GameState currentState);
}
