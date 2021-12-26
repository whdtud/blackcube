using UnityEngine;

public class Tile : MonoBehaviour 
{
    private Material mMaterial;
    private Color mFixedColor;
    private Color mDestColor;
    private float mNextChangeColorTime;

    private const float LERP_AMOUNT = 0.2f;
    private const float COLOR_KEEP_TIME = 3f;

    void Awake()
    {
        mMaterial = GetComponent<Renderer>().material;

        mFixedColor = Color.white;
        mDestColor = Color.white;
        mNextChangeColorTime = float.MaxValue;
    }

	void Update ()
    {
        if (mMaterial.color != mDestColor)
            mMaterial.color = Color.Lerp(mMaterial.color, mDestColor, LERP_AMOUNT);

        if (Time.time < mNextChangeColorTime)
            return;

        if (mFixedColor == Color.red)
        {
            if (mDestColor == Color.white)
                mDestColor = mFixedColor;
            else
                mDestColor = Color.white;

            mNextChangeColorTime = Time.time + COLOR_KEEP_TIME;
        }
        else if (mDestColor == Defines.TILE_WHITE_GREEN)
        {
            mDestColor = Color.white;

            mNextChangeColorTime = float.MaxValue;
        }
        else
        {
            mNextChangeColorTime = float.MaxValue;
        }
    }

    public void SetFixedColor(Color fixedColor)
    {
        mFixedColor = fixedColor;
        mDestColor = fixedColor;
        mNextChangeColorTime = Time.time + COLOR_KEEP_TIME;
    }

    private float GetBuffMoveSpeedFromColor()
    {
        if (mFixedColor == Color.blue)
            return 2f;
        else if (mFixedColor == Defines.TILE_PURPLE)
            return 0.7f;
        else
            return 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameState gameState = GameController.Instance.CurrentState;
        if (gameState == GameState.PAUSE || gameState == GameState.OVER)
            return;

        if (collision.gameObject.tag != "Player")
            return;

        PlayerController player = PlayerController.Instance;

        if (mMaterial.color == Color.red)
        {
            mMaterial.color = Color.white;

            player.Character.Jump(Vector3.up, 3f);
            player.Character.OnDamaged(GameController.Instance.Stage);

            var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_EXPLOSION);
            effect.Init(transform.position, Quaternion.identity);
        }
        else if (mMaterial.color == Color.yellow)
        {
            if (collision.rigidbody.velocity.y >= 0)
                return;

            mMaterial.color = Color.white;

            player.Character.JumpOnTile();

            var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_TILE_YELLOW);
            effect.Init(transform.position, Quaternion.identity);
        }
        else if (mMaterial.color == Color.blue)
        {
            mDestColor = Defines.TILE_WHITE_BLUE;
            mMaterial.color = Defines.TILE_WHITE_BLUE;
        }
        else if (mMaterial.color == Color.green)
        {
            mMaterial.color = Defines.TILE_WHITE_GREEN;
            mDestColor = Defines.TILE_WHITE_GREEN;
            mNextChangeColorTime = Time.time + COLOR_KEEP_TIME;

            var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_TILE_GREEN);
            effect.Init(transform.position, Quaternion.identity);
        }
        else if (mMaterial.color == Defines.TILE_PURPLE)
        {
            mDestColor = Defines.TILE_WHITE_PURPLE;
            mMaterial.color = Defines.TILE_WHITE_PURPLE;
        }
    }

    public void OnCollisionStay(Collision collision)
	{
        GameState gameState = GameController.Instance.CurrentState;
        if(gameState == GameState.PAUSE || gameState == GameState.OVER)
            return;

        if (collision.gameObject.tag != "Player")
            return;

        PlayerController player = PlayerController.Instance;

        player.Character.BuffMoveSpeed = GetBuffMoveSpeedFromColor();

        if (mDestColor == Defines.TILE_WHITE_GREEN)
            player.Data.Hp += player.Data.MaxHp * 0.1f * Time.deltaTime;
    }
	
	public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.Instance.Character.BuffMoveSpeed = 1f;
            return;
        }

        if (mDestColor == Defines.TILE_WHITE_BLUE ||
            mDestColor == Defines.TILE_WHITE_PURPLE)
        {
            mMaterial.color = mFixedColor;
            mDestColor = mFixedColor;
        }
	}
}
