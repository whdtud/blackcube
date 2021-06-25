using UnityEngine;

public class UIBattleZoneTop : MonoBehaviour, IGameStateListener
{
    public UIGameTimer GameTimer;
    public UIBossHp BossHp;

    void Awake()
    {
        GameController.Instance.GameStateListeners.Add(this);
    }

    public void OnChangeState(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.READY:
                {
                    GameTimer.SetColor(Color.green);

                    GameTimer.gameObject.SetActive(true);
                    BossHp.gameObject.SetActive(false);
                }
                break;
            case GameState.REST:
                {
                    StartCoroutine(GameTimer.Co_Rest(() => 
                    { 
                        GameController.Instance.ChangeState(GameState.PLAY);
                    }));
                }
                break;
            case GameState.BOSS:
                {
                    GameTimer.gameObject.SetActive(false);
                    BossHp.gameObject.SetActive(true);
                }
                break;
        }
    }
}
