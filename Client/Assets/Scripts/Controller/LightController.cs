using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

	private Light _light;

	private float _red;
	private float _green;
	private float _blue;
	private float _colorChangeTime = 0;
	private float _delay = 3.0f;
	// Use this for initialization
	void Start () {
		_light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {

		if(_colorChangeTime < Time.time) {
			_colorChangeTime = Time.time + _delay;
			do {
				_red = Random.Range(0, 2);
				_green = Random.Range(0, 2);
				_blue = Random.Range(0, 2);
			} while (_red == 0 && _green == 0 && _blue == 0);
		}

		if(_red == 0) _red = 0.5f;
		if(_green== 0) _green = 0.5f;
		if(_blue == 0) _blue = 0.5f;

		float r = Mathf.Lerp(_light.color.r, _red, 0.01f);
		float g = Mathf.Lerp(_light.color.g, _green, 0.01f);
		float b = Mathf.Lerp(_light.color.b, _blue, 0.01f);
		_light.color = new Vector4(r,g,b, 1);
	}
}
