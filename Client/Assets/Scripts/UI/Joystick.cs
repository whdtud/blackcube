using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public enum JoystickType
{
    MOVE,
    ATTACK,
    END,
}

public class Joystick : MonoBehaviour {

    public GameObject[] Effect;
    public Image[] Time;
    public Image[] Fill;
    public Image[] Stick;
    public Collider[] TouchArea;

    private bool mIsLockAttack;
    private bool mIsLockSkill;

#if !UNITY_EDITOR
    private float mRadius;
    private float mCanvasCenterX;
    private bool mIsLockDash;
    private int mMoveTouchCount;
    private int mAttackTouchCount;
    private float mMoveTouchCountTime;
    private float mAttackTouchCountTime;

    private List<int> mMoveTouches = new List<int>();
    private List<int> mAttackTouches = new List<int>();
#endif

    private const float FILL_SMOOTH = 0.15f;

    void Awake()
    {
        Canvas canvas = UICanvas.Instance.GetComponent<Canvas>();

#if !UNITY_EDITOR
        mRadius = canvas.scaleFactor * Time[(int)JoystickType.MOVE].rectTransform.sizeDelta.x / 2f;
        mCanvasCenterX = canvas.pixelRect.width / 2f;
#endif
    }

    void Update()
    {
        PlayerCharacter player = GameController.Instance.Player.Character;

        var currentState = GameController.Instance.CurrentState;
        if (currentState == GameState.OVER || currentState == GameState.PAUSE)
        {
            player.MoveDir = Vector3.zero;
            return;
        }

#if UNITY_EDITOR
        float w = Input.GetAxis("Horizontal");
        float h = Input.GetAxis("Vertical");

        player.MoveDir = new Vector3(w, 0f, h);

        Image moveFill = Fill[(int)JoystickType.MOVE];
        
        if (w != 0 || h != 0)
        {
            if (moveFill.rectTransform.sizeDelta.x < 340)
                moveFill.rectTransform.sizeDelta = Vector2.Lerp(moveFill.rectTransform.sizeDelta, new Vector2(340, 340), FILL_SMOOTH);

            if (Input.GetKeyDown("space"))
            {
                if (Time[(int)JoystickType.MOVE].fillAmount >= 1)
                    GameController.Instance.Player.Character.Dash();
            }
        }
        else
        {
            if (moveFill.rectTransform.sizeDelta.x > 120)
                moveFill.rectTransform.sizeDelta = Vector2.Lerp(moveFill.rectTransform.sizeDelta, new Vector2(120, 120), FILL_SMOOTH);
        }

        if (Input.GetKeyDown(KeyCode.Q))
            player.Rigidbody.AddForce(new Vector3(0, 500f, 0));

        Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 viewDir = (Input.mousePosition - playerPos).normalized;

        float viewAngle = Mathf.Atan2(viewDir.x, viewDir.y) * Mathf.Rad2Deg;

        if (viewAngle < 0)
            viewAngle += 360;

        // smooth code 추가 필요
        player.AngleY = viewAngle;

        if (mIsLockAttack)
        {
            if (Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta.x < 340)
            {
                Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta = Vector2.Lerp(Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta, new Vector2(340, 340), 0.15f);
            }
            if (!mIsLockSkill)
            {
                GameController.Instance.Player.Character.TryAttack();
            }
        }
        else
        {
            if (Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta.x > 120)
            {
                Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta = Vector2.Lerp(Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta, new Vector2(120, 120), 0.15f);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!mIsLockAttack) 
                mIsLockAttack = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!mIsLockSkill)
            {
                mIsLockAttack = false;
                GameController.Instance.Player.Character.AttackEnded();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Time[(int)JoystickType.ATTACK].fillAmount >= 1)
            {
                if (!mIsLockAttack) mIsLockAttack = true;
                mIsLockSkill = true;
                GameController.Instance.Player.Character.BeginSkill();
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (mIsLockSkill)
            {
                GameController.Instance.Player.Character.SkillFire();
                mIsLockAttack = false;
                mIsLockSkill = false;
            }
        }
#else
        if (mMoveTouchCount > 0)
        {
            mMoveTouchCountTime += UnityEngine.Time.deltaTime;
            if (mMoveTouchCountTime > 0.2f)
            {
                mMoveTouchCount = 0;
                mMoveTouchCountTime = 0;
            }
        }

        if (mAttackTouchCount > 0)
        {
            mAttackTouchCountTime += UnityEngine.Time.deltaTime;
            if (mAttackTouchCountTime > 0.2f)
            {
                mAttackTouchCount = 0;
                mAttackTouchCountTime = 0;
            }
        }

        if (mAttackTouches.Count == 0)
        {
            GameController.Instance.Player.Character.AttackEnded();
        }

        int count = Input.touchCount;

        //멀티 터치
        if (count == 0) 
            return;

        GameState gameState = GameController.Instance.GameState;

        for (int i = 0; i < count; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                if (gameState == GameState.PAUSE || gameState == GameState.OVER)
                    continue;

                if (touch.position.x < mCanvasCenterX)
                {
                    mMoveTouches.Add(touch.fingerId);

                }
                else if (touch.position.x >= mCanvasCenterX)
                {
                    mAttackTouches.Add(touch.fingerId);
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (gameState == GameState.PAUSE || gameState == GameState.OVER)
                    continue;

                if (mMoveTouches.Count > 0)
                {
                    if (mMoveTouches[0] == touch.fingerId)
                    {
                        Vector2 joystickCenter = new Vector2(Time[(int)JoystickType.MOVE].rectTransform.position.x,
                                                            Time[(int)JoystickType.MOVE].rectTransform.position.y);
                        Vector2 dir = touch.position - joystickCenter;
                        float distance = dir.magnitude;
                        dir.Normalize();

                        player.MoveDir = new Vector3(dir.x, 0f, dir.y);

                        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

                        if (angle < 0)
                        {
                            angle += 360;
                        }

                        float radian = angle * Mathf.Deg2Rad;

                        float x = Mathf.Sin(radian);
                        float y = Mathf.Cos(radian);

                        if (distance > mRadius)
                        {
                            Stick[(int)JoystickType.MOVE].transform.position = new Vector3(x * mRadius + joystickCenter.x, y * mRadius + joystickCenter.y, 0);
                        }
                        else
                        {
                            Stick[(int)JoystickType.MOVE].rectTransform.position = new Vector3(touch.position.x, touch.position.y, 0);
                        }

                        if (mMoveTouchCount == 1 && Fill[(int)JoystickType.MOVE].rectTransform.sizeDelta.x > 339)
                        {
                            if (Time[(int)JoystickType.MOVE].fillAmount >= 1)
                            {
                                if (mIsLockDash == false)
                                {
                                    GameController.Instance.Player.Character.Dash();
                                    mIsLockDash = true;
                                }
                            }
                            else
                            {
                                mIsLockDash = true;
                            }
                        }
                    }
                }

                if (mAttackTouches.Count > 0)
                {
                    if (mAttackTouches[0] == touch.fingerId)
                    {
                        Vector2 joystickCenter = new Vector2(Time[(int)JoystickType.ATTACK].rectTransform.position.x,
                                                                Time[(int)JoystickType.ATTACK].rectTransform.position.y);
                        Vector2 dir = touch.position - joystickCenter;
                        float _distance = dir.magnitude;
                        dir.Normalize();

                        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

                        if (angle < 0)
                        {
                            angle += 360;
                        }

                        player.AngleY = angle;

                        float radian = angle * Mathf.Deg2Rad;

                        float x = Mathf.Sin(radian);
                        float y = Mathf.Cos(radian);


                        if (_distance > mRadius)
                        {
                            Stick[(int)JoystickType.ATTACK].transform.position = new Vector3(x * mRadius + joystickCenter.x, y * mRadius + joystickCenter.y, 0);
                        }
                        else
                        {
                            Stick[(int)JoystickType.ATTACK].rectTransform.position = new Vector3(touch.position.x, touch.position.y, 0);
                        }

                        //공격
                        if (mIsLockAttack == false)
                        {
                            GameController.Instance.Player.Character.TryAttack();
                        }

                        //스킬
                        if (mAttackTouchCount == 1 && Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta.x > 339)
                        {
                            if (Time[(int)JoystickType.ATTACK].fillAmount >= 1)
                            {
                                if (mIsLockSkill == false)
                                {
                                    GameController.Instance.Player.Character.BeginSkill();
                                    mIsLockAttack = true;
                                    mIsLockSkill = true;
                                }
                            }
                            else
                            {
                                mIsLockSkill = true;
                            }
                        }
                    }
                }

            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                foreach (int delete in mMoveTouches)
                {
                    if (delete == touch.fingerId)
                    {
                        if (mMoveTouches[0] == touch.fingerId)
                        {
                            mIsLockDash = false;
                            if (mMoveTouchCount < 1) mMoveTouchCount++;
                        }
                        mMoveTouches.Remove(delete);
                        break;
                    }
                }

                foreach (int delete in mAttackTouches)
                {
                    if (delete == touch.fingerId)
                    {
                        if (mAttackTouches[0] == touch.fingerId)
                        {
                            mIsLockSkill = false;
                            if (mAttackTouchCount < 1) mAttackTouchCount++;

                            if (mIsLockAttack)
                            {
                                if (Time[(int)JoystickType.ATTACK].fillAmount >= 1)
                                {
                                    mIsLockAttack = false;
                                    GameController.Instance.Player.Character.SkillFire();
                                }
                            }
                        }

                        mAttackTouches.Remove(delete);

                        break;
                    }
                }
            }
        }
#endif
    }

    void LateUpdate()
    {
        PlayerCharacter player = GameController.Instance.Player.Character;

        Time[(int)JoystickType.MOVE].fillAmount = player.DashPercent;
        Time[(int)JoystickType.ATTACK].fillAmount = player.SkillPercent;

        for (int i = 0; i < (int)JoystickType.END; i++)
        {
            if (Time[i].fillAmount >= 1 &&
                Fill[i].rectTransform.sizeDelta.x > 339)
            {
                if (Effect[i].activeSelf == false)
                    Effect[i].SetActive(true);
            }
            else
            {
                if (Effect[i].activeSelf == true)
                    Effect[i].SetActive(false);
            }
        }

#if !UNITY_EDITOR
        if (mMoveTouches.Count == 0)
        {
            Stick[(int)JoystickType.MOVE].rectTransform.position = Vector3.Lerp(Stick[(int)JoystickType.MOVE].rectTransform.position, 
                                                                                Time[(int)JoystickType.MOVE].rectTransform.position, 0.1f);

            if (mMoveTouchCount == 0 && Fill[(int)JoystickType.MOVE].rectTransform.sizeDelta.x > 120)
                Fill[(int)JoystickType.MOVE].rectTransform.sizeDelta = Vector2.Lerp(Fill[(int)JoystickType.MOVE].rectTransform.sizeDelta, new Vector2(120, 120), 0.1f);
        }
        else
        {
            if (Fill[(int)JoystickType.MOVE].rectTransform.sizeDelta.x < 340)
            {
                Fill[(int)JoystickType.MOVE].rectTransform.sizeDelta = Vector2.Lerp(Fill[(int)JoystickType.MOVE].rectTransform.sizeDelta, new Vector2(340, 340), 0.6f);
            }
        }


        if (mAttackTouches.Count == 0)
        {
            Stick[(int)JoystickType.ATTACK].rectTransform.position = Vector3.Lerp(Stick[(int)JoystickType.ATTACK].rectTransform.position,
                                                                                Time[(int)JoystickType.ATTACK].rectTransform.position, 0.1f);

            if (mMoveTouchCount == 0 && Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta.x > 120)
                Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta = Vector2.Lerp(Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta, new Vector2(120, 120), 0.1f);
        }
        else
        {
            if (Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta.x < 340)
            {
                Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta = Vector2.Lerp(Fill[(int)JoystickType.ATTACK].rectTransform.sizeDelta, new Vector2(340, 340), 0.6f);
            }
        }
#endif
    }
}
