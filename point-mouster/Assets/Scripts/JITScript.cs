using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JITScript : MonoBehaviour { 
	// JUST IN TIME INSTRUCTIONS

	public Text wordDisplay;

	public GoogleAnalyticsV4 googleAnalytics;
	private string playerName;

	// Use this for initialization
	void Start () {
		playerName = "error with name"; //should be updated with PlayerPrefs
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			switch (this.tag) {
				case "BeginLevel":
					wordDisplay.text = "Use the left and right arrow keys to move the player. \n To close a fact display press the x keystroke " +
						"or click on the x in the top-right corner. \n Use spacebar to jump.";
					Time.timeScale = 0.0f;
					Destroy (this.gameObject);
					break;
				case "Objective":
					wordDisplay.text = "Use the left and right arrow keys to move the player. \nTo close a fact display press the x keystroke " +
						"or click on the x in the top-right corner. \nUse spacebar to jump.\nPress the 'p' key to pause/resume.";
					Destroy (this.gameObject);
					break;
				case "Controls":
					//wordDisplay.text = "Press the 'p' key to pause/resume.";
					wordDisplay.text = "You may encounter some helpful people along your journey...\nMake sure you take their advice.";
					Time.timeScale = 0.0f;
					Destroy (this.gameObject);
					break;
				case "BeforeReview":
					wordDisplay.text = "Press the up key over the door to continue to the review section.";
					Time.timeScale = 0.0f;
					Destroy (this.gameObject);
					break;
				case "BossJIT":
					wordDisplay.text = "Get ready to battle the boss.\n\nJump on the boss to receive a question. Answer 5 questions right to kill the boss.";
					Time.timeScale = 0.0f;
					Destroy (this.gameObject);
					break;
				case "Level2JIT":
					BookScript.bookControl.ResetBooks ();
					Debug.Log("Level 2 reached");

					/*
        			PlayerPrefs.GetString() returns the value corresponding to key (set in 1st param) in the preference file if it exists
        			get player name stored in PlayerPrefs by key or return string with error message
       				*/
        			string keyNotFoundHandler = "No name";
        			playerName = PlayerPrefs.GetString ("CurrentPlayer", keyNotFoundHandler);
        			Debug.Log("player name = " + playerName);
					
					/*
					This should be added just before a level is loaded. 
					*/
					googleAnalytics.LogEvent (new EventHitBuilder ()
						.SetEventCategory ("LEVELREACHED")
						.SetEventAction (playerName)
						.SetEventLabel ("LEVELREACHED")
						.SetEventValue (2)	//level #
					); 
					SceneManager.LoadScene ("Level2");
					break;
			}
		} else {
			Debug.Log ("some error occurred when colliding - JITScript");
		}
	}



}
