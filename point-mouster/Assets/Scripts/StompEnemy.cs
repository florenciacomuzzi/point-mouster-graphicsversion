using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StompEnemy : MonoBehaviour {
	// Use this for initialization
	public Text questionDisplay;
	public BossQuestions boss;
	public Button choice1;
	public Button choice2;
	public Button choice3;
	public Button choice4;
	public QuestionCanvas qCanvas;
	public QuestionPanel panel;
	public int answer;
	GameObject button1;
	GameObject button2;
	GameObject button3;
	GameObject button4;
	public static string ques;

	private int index;

	void Start () {
		index = setIndex();
		boss = FindObjectOfType<BossQuestions> ();
		qCanvas = FindObjectOfType<QuestionCanvas> ();
		//questionDisplay = FindObjectOfType<Text>();
		panel = FindObjectOfType<QuestionPanel> ();
		button1 = GameObject.FindGameObjectWithTag ("Choice1");
		button2 = GameObject.FindGameObjectWithTag ("Choice2");
		button3 = GameObject.FindGameObjectWithTag ("Choice3");
		button4 = GameObject.FindGameObjectWithTag ("Choice4");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("in StompEnemy's OnTriggerEnter2D(Collider2D other) ");
		if (other.tag == "Enemy") {
			Debug.Log("you've destroyed an enemy");
			Destroy (other.gameObject);
		}
		if (other.tag == "Boss") {
			Debug.Log("you collided with boss");
			int index = boss.pickQuestion();
			if(index != CurrentQuestion.currQuestionStaticInstance.getQuestionInd()) {
				Debug.Log("ERROR:CurrentQuestion was not updated properly in pickQuestion()");
			}
			setIndex(index);
			Debug.Log("index: " + index);
			qCanvas.enableQuestionCanvas ();
			panel.enable ();
			activateButtons();
			Time.timeScale = 0.0f;
			setQuestionDisplayText();
			MC ();
		}
	}

	//set question display text thru Current Question
	private void setQuestionDisplayText() {
		questionDisplay.text = CurrentQuestion.currQuestionStaticInstance.Instance.getQuestionStr();
	}

	void MC(){
		//assign word choices to button texts
		BossQuestions.Question myquestion = CurrentQuestion.currQuestionStaticInstance.Instance;
		choice1.GetComponentInChildren<Text>().text = myquestion.getChoiceStr(0);//boss.choice1;//boss.multiple_choice [0];
		choice2.GetComponentInChildren<Text>().text = myquestion.getChoiceStr(1);//boss.choice2;//boss.multiple_choice [1];
		choice3.GetComponentInChildren<Text>().text = myquestion.getChoiceStr(2);//boss.choice3;//boss.multiple_choice [2];
		choice4.GetComponentInChildren<Text>().text = myquestion.getChoiceStr(0);//boss.choice4;//boss.multiple_choice [3];
	}

	//activate all 4 multiple choice buttons
	private void activateButtons() {
		button1.SetActive (true);
		button2.SetActive (true);
		button3.SetActive (true);
		button4.SetActive (true);
	}

	//set index
	private int setIndex(int val = -1) {
		index = val;
		return index;
	}
}
