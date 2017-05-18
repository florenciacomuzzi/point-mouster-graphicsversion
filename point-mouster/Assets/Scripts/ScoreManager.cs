using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	public static ScoreManager Instance;
	private int score;
	private int timesPushed; //used to check how many attempts a player has taken with a question


	void Awake() {
		if (Instance == null) {
			Debug.Log("Instance in Version is null");
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else {
			Destroy (gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		score = 0;
		timesPushed = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void incrementScore(int points) {
		Debug.Log("in incrementScore of ScoreManager... adding points to score + " + points);
		score += points;
		updateScoreDisplay();
	}

	public void updateScoreDisplay() {
		Debug.Log("in updateScoreDisplay with score = " + score);
		GameObject.FindGameObjectWithTag ("scoretag").GetComponent<Text> ().text = "Score: " + score;
	}

	public int getScore() {
		return score;
	}

	public int getTimesPushed() {
		return timesPushed;
	}

	public void increaseNumAttempts() {
		timesPushed += 1;
	}

	public void resetAttempts() {
		timesPushed = 0;
	}
}
