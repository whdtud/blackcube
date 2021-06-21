using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class LoadingController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
		SceneManager.LoadScene("GameScene");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
