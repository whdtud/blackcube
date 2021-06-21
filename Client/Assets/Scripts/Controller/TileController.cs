using UnityEngine;
using System.Collections;

public class TileController : MonoBehaviour {

	[HideInInspector]
	public Color _destColor;
	private Material _thisMaterial;
    private bool _isRed;
    private float _redTime;
    private float _greenTime;
    public ParticleSystem _redParticle;
    public ParticleSystem _greenParticle;
    public ParticleSystem _yellowParticle;

    public static Color _colorWhiteBlue = new Color(0, 0.5f, 1);    // 연파랑
    public static Color _colorWhiteGreen = new Color(0.4f, 1, 0.2f);    // 연초록
    public static Color _colorWhitePurple = new Color(0.9f, 0, 0.9f);     // 연보라


	void Start () {
		_destColor = Color.white;
		_thisMaterial = GetComponent<Renderer>().material;
	}
	
	void Update () {
        if (_destColor == Color.red)
        {
            if (_isRed)
            {
                if (_thisMaterial.color != _destColor)
                {
                    _thisMaterial.color = Color.Lerp(_thisMaterial.color, _destColor, 0.2f);
                }

                _redTime += Time.deltaTime;
                if (_redTime >= 3.0f)
                {
                    _isRed = false;
                    _redTime = 0;
                }
            }
            else
            {
                if (_thisMaterial.color != Color.white)
                {
                    _thisMaterial.color = Color.Lerp(_thisMaterial.color, Color.white, 0.2f);
                }

                _redTime += Time.deltaTime;
                if (_redTime >= 3.0f)
                {
                    _isRed = true;
                    _redTime = 0;
                }
            }
        }
        else if (_destColor == MapController.COLOR_RED)
        {
            if (_isRed)
            {
                if (_thisMaterial.color != _destColor)
            {
                    _thisMaterial.color = Color.Lerp(_thisMaterial.color, _destColor, 0.2f);
                }

                _redTime += Time.deltaTime;
                if (_redTime >= 3.0f)
                {
                    _isRed = false;
                    _redTime = 0;
                }
            }
            else
            {
                if (_thisMaterial.color != Color.white)
                {
                    _thisMaterial.color = Color.Lerp(_thisMaterial.color, Color.white, 0.2f);
                }

                _redTime += Time.deltaTime;
                if (_redTime >= 3.0f)
                {
                    _isRed = true;
                    _redTime = 0;
                }
            }
		        }
        else if (_destColor == _colorWhiteGreen)
        {
            _greenTime += Time.deltaTime;

            if (_greenTime >= 3.0f)
            {
                _destColor = Color.white;
	        }
        }
        else
        {
            if (_thisMaterial.color != _destColor)
            {
		    	_thisMaterial.color = Color.Lerp(_thisMaterial.color, _destColor, 0.2f);
		    }
	    }
	}

	public void OnCollisionEnter(Collision collision)
	{
	}

	public void OnCollisionStay(Collision collision)
	{
        GameState gameState = GameController.Instance.CurrentState;
        PlayerController player = GameController.Instance.Player;

        if(gameState == GameState.PAUSE || gameState == GameState.OVER)
        {
            return;
        }

		if (collision.gameObject.tag == "Player")
		{
            if (_thisMaterial.color == Color.white)
			{
                player.Character.BuffMoveSpeed = 1.0f;
            }
			else if(_thisMaterial.color == Color.red
                || _thisMaterial.color == MapController.COLOR_RED) 
			{
                player.Character.Jump(Vector3.up, 3f);
                player.Character.OnDamaged(GameController.Instance.Stage);
                Instantiate(_redParticle, transform.position, Quaternion.identity);
                _thisMaterial.color = Color.white;
			}
			else if(_thisMaterial.color == Color.yellow) 
			{
				if (collision.rigidbody.velocity.y < 0)
				{
					player.Character.JumpOnTile();
                    Instantiate(_yellowParticle, transform.position, Quaternion.identity);
                    _thisMaterial.color = Color.white;
				}
			}
			else if (_destColor == Color.blue)
			{
                player.Character.BuffMoveSpeed = 2f;
                _thisMaterial.color = _colorWhiteBlue;
                _destColor = _colorWhiteBlue;
			}
            else if (_destColor == _colorWhiteBlue)
			{
				player.Character.BuffMoveSpeed = 2f;
			}
			else if(_thisMaterial.color == Color.green) 
			{
                Instantiate(_greenParticle, transform.position, Quaternion.identity);
                
                _thisMaterial.color = _colorWhiteGreen;
                _destColor = _colorWhiteGreen;
            }
            else if (_destColor == _colorWhiteGreen)
                {
                player.Data.Hp += player.Data.MaxHp * 0.1f * Time.deltaTime;
			}
            else if (_destColor == MapController.COLOR_PURPLE) 
			{
                player.Character.BuffMoveSpeed = 0.5f;
                _thisMaterial.color = _colorWhitePurple;
                _destColor = _colorWhitePurple;
			}
            else if (_destColor == _colorWhitePurple)
			{
				player.Character.BuffMoveSpeed = 0.7f;
			}
		}
	}
	
	public void OnCollisionExit(Collision collision)
	{
		GameController.Instance.Player.Character.BuffMoveSpeed = 1.0f;

        if (_destColor == _colorWhiteBlue)
        {
            _thisMaterial.color = Color.blue;
            _destColor = Color.blue;
        }
        else if (_destColor == _colorWhitePurple)
        {
            _thisMaterial.color = MapController.COLOR_PURPLE;
            _destColor = MapController.COLOR_PURPLE;
        }
	}

    public void ChangeTile(Color destColor)
    {
        _destColor = destColor;

        if (destColor == Color.red)
        {
            _isRed = true;
            _redTime = 0f;
        }
        else if (destColor == MapController.COLOR_RED)
        {
            _isRed = false;
            _redTime = 0f;
        }
        else if(destColor == Color.green)
        {
            _greenTime = 0f;
        }
	}
}
