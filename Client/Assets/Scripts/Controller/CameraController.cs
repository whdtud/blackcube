using UnityEngine;

using System.Collections;

public class CameraController : STController<CameraController>, IGameStateListener
{
    public IsometricCamera BattleZoneCamera { get; private set; }

    void Awake()
    {
        GameController.Instance.GameStateListeners.Add(this);
    }

    public void Init()
    {
        BattleZoneCamera = FindObjectOfType<IsometricCamera>();
        BattleZoneCamera.ZoomValue = 1f;

        SetTarget(PlayerController.Instance.Character.Tm);
    }

    public void OnChangeState(GameState prevState, GameState currentState)
    {
        if (currentState == GameState.READY)
        {
            BattleZoneCamera.ZoomValue = 1f;
        }
        else if (currentState == GameState.OVER)
        {
            BattleZoneCamera.ZoomValue = 0.3f;
            BattleZoneCamera.SetTarget(null);
        }
    }

    public void SetTarget(Transform tm)
    {
        BattleZoneCamera.SetTarget(tm);
        BattleZoneCamera.SetPosition(new Vector3(0f, 1000f, 5.5f));
    }

    public void Shake()
    {
        BattleZoneCamera.Shake();
	}

    public void Shake(float duration, float amplitude)
    {
        BattleZoneCamera.Shake(duration, amplitude);
	}

    public void Shake(float duration, float amplitude, float delay)
    {
        BattleZoneCamera.Shake(duration, amplitude, delay);
    }
}
