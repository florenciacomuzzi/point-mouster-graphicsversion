using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MessagePanel : MonoBehaviour {
	public Text WordDisplay;
	public GameObject button;

	/*
	used for display of graphic fact
	*/
	Scene currentLevel; 
	string levelName;

	// Use this for initialization
	void Start () {
		this.GetComponent<SpriteRenderer>().enabled = false;
		button.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		//print("in MessagePanel script Update()");
		if (!string.IsNullOrEmpty (WordDisplay.text.ToString ().Trim ())) {
			this.GetComponent<SpriteRenderer>().enabled = true;
			Debug.Log ("in MessagePanel - something to display");
			GameObject go2 = GameObject.FindGameObjectWithTag ("level1factTAG");
			button.SetActive(true);
		} else {
			this.GetComponent<SpriteRenderer>().enabled = false;
			button.SetActive(false);
		}
	}
}
