using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
	public Text scoreText;
	//public GameObject scoreGO;

	// Use this for initialization
	void Start () {
		scoreText.text = "Score: " + ScoreManager.Instance.getScore();
		//scoreGO = GameObject.FindGameObjectWithTag("scoretag");
		//scoreText =
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateScoreDisplay() {
		Debug.Log("in updateScoreDisplay with score = " + ScoreManager.Instance.getScore());
		scoreText.text = "Score: " + ScoreManager.Instance.getScore();
	}

}
