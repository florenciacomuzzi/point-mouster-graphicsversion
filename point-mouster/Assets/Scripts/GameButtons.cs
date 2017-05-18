using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour {
	public GameObject[] showIfPaused;
	public GameObject[] showIfResumed;

	public QuestionPanel panel;
	public QuestionCanvas qCanvas;

	/*
	used for fact display -- graphics version
	*/
	Scene currentLevel;
	string levelName;

	// Use this for initialization
	void Start () {
		qCanvas = FindObjectOfType<QuestionCanvas> ();
		panel = FindObjectOfType<QuestionPanel> ();

		//populate array with relevant objects to show when game is paused
		showIfPaused = GameObject.FindGameObjectsWithTag("WhenPaused");
		foreach (GameObject psed in showIfPaused)
			psed.SetActive (false);

		//populate array with relevant objects to show when game is in play/resumed
		showIfResumed = GameObject.FindGameObjectsWithTag("WhenResumed");
		foreach (GameObject resmd in showIfResumed)
			resmd.SetActive (true);

		currentLevel = SceneManager.GetActiveScene ();
		levelName = currentLevel.name;
	}

	// Update is called once per frame
	void Update () {
//        if (GameObject.FindGameObjectWithTag("WordDisplay") != null)
//        {
//            if (!string.IsNullOrEmpty(GameObject.FindGameObjectWithTag("WordDisplay").GetComponent<Text>().text.Trim()) &&
//                Input.GetKeyDown("x"))
//            {
//                ClearWordDisplay();
//            }
//        }
	}

	public void PauseGame(){
		Time.timeScale = 0.0f;

		foreach (GameObject psed in showIfPaused)
			psed.SetActive (true);

		foreach (GameObject resmd in showIfResumed)
			resmd.SetActive (false);
	}

	public void ResumeGame(){
		Time.timeScale = 1.0f;

		foreach (GameObject psed in showIfPaused)
			psed.SetActive (false);

		foreach (GameObject resmd in showIfResumed)
			resmd.SetActive (true);
	}

	public void ClearWordDisplay() {
		if (levelName == "Level1") {
			Debug.Log("in ClearWordDisplay.. it's level 1 so should clear display");	
			GameObject go = GameObject.FindGameObjectWithTag ("level1factTAG");
			SpriteRenderer sr = go.GetComponent<SpriteRenderer> ();
			if (sr.enabled)
				sr.enabled = false;
		} else if (levelName == "Level2") {
			Debug.Log("in ClearWordDisplay.. it's level 2 so should clear display");	
			GameObject go = GameObject.FindGameObjectWithTag ("level2factTAG");
			SpriteRenderer sr = go.GetComponent<SpriteRenderer> ();
			if (sr.enabled)
				sr.enabled = false;
		}
		GameObject.FindGameObjectWithTag ("WordDisplay").GetComponent<Text> ().text = "";//to indicate no fact there
		ResumeGame ();
	}
	

	//called by ButtonPushed.Pushed to clear display after answered + feedback is given
	public void ClearQuestionDisplay() {
		GameObject.FindGameObjectWithTag ("Choice1").SetActive (false);
		GameObject.FindGameObjectWithTag ("Choice2").SetActive (false);
		GameObject.FindGameObjectWithTag ("Choice3").SetActive (false);
		GameObject.FindGameObjectWithTag ("Choice4").SetActive (false);

		qCanvas.disableQuestionCanvas ();
		panel.disable ();
		ResumeGame ();
	}

	public void Menu()
	{
		SceneManager.LoadScene ("Menu");
	}


}
