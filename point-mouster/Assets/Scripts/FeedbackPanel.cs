using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FeedbackPanel : MonoBehaviour {
	public Text fbDisplay;
	public GameObject p;
	//public ButtonPushed btnPushed;
	//public GameObject button;

	void Awake() {
		print("in Awake for FeedbackPanel");
		p = GameObject.FindGameObjectWithTag("FeedbackDisplay");
		if(p == null){
			print("p is null");
		} else {
			print("p != null");
		}
	}

	// Use this for initialization
	void Start () {
		print("in Start of FeedbackPanel script");
		//btnPushed = FindObjectOfType<ButtonPushed> ();
		fbDisplay.text = "";
		this.GetComponent<Image>().enabled = false;
		//button.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		//print("in update in Feedback Panel");
		//if (!string.IsNullOrEmpty (WordDisplay.text.ToString ().Trim ())) {
			//this.GetComponent<SpriteRenderer>().enabled = true;
			//button.SetActive(true);
		//} else {
		//	this.GetComponent<SpriteRenderer>().enabled = false;
		//	button.SetActive(false);
		//}
		
	}


	//called by ButtonPushed script to enable feedback panel
	public void enableFBPanel(string feedback, Color color) {
		Debug.Log("in enableFBPanel()");
		fbDisplay.text = feedback; //get feedback text to display
		fbDisplay.color = color;
		this.GetComponent<Image>().enabled = true;
	}


	public void disableFBPanel (){
		print("in disableFBPanel of FeedbackPanel script");
		fbDisplay.text = "";
		this.GetComponent<Image> ().enabled = false;
	}
}
