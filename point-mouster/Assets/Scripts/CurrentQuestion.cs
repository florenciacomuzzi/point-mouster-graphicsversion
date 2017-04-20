using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentQuestion : MonoBehaviour {
	public BossQuestions.Question Instance;
	private int questionInd;
	public static CurrentQuestion currQuestionStaticInstance;


	// Use this for initialization
	void Start () {
		if(Instance == null){
			Debug.Log("CurrentQuestion.ownInstance == null");
		}
		if(iscurrQuestionStaticInstanceNull()){
			Debug.Log("BossQuestions.Question instance == null");
		}
		questionInd = -1;
		Instance = InitQuestion();
	}
	
	void Awake() {
		if (currQuestionStaticInstance == null) {
			Debug.Log("currQuestionStaticInstance == null,do not destroy");
			DontDestroyOnLoad (gameObject);
			currQuestionStaticInstance = this;
		} else {
            Destroy (gameObject);
		}
	}


	// Update is called once per frame
	void Update () {
		
	}

	public bool iscurrQuestionStaticInstanceNull() {
		if (currQuestionStaticInstance == null) {
			Debug.Log("currQuestionStaticInstance in CurrentQuestion is null");
			return true;
		}
		return false;
	}

	//initialize to empty question
	private BossQuestions.Question InitQuestion() {
		BossQuestions.Question q = new BossQuestions.Question();
		return q;
	}

	//class helper
	private BossQuestions.Question getInstance() {
		return Instance;
	}

	//method returns instance of CurrentQuestion's Question member
	public BossQuestions.Question getQuestInstance() {
		return getInstance();
	}

	//return pos of question in BossQuestions.questions zero-indexed List
	public int getQuestionInd() {
		return questionInd;
	}

	//returns pos of question in BossQuestions.questions zero-indexed List as string
	public string getQuestionIndToString() {
		return getQuestionInd().ToString();
	}

	//get current question's answer
	public string getQuestionAnswer() {
		return Instance.getQuestionAnswer();
	}

	public void updateQuestion(BossQuestions.Question currQuestion, int index) {
		Debug.Log("in CurrentQuestion updateQuestion()");
		Instance = currQuestion;
		questionInd = index;
		Debug.Log("Instance.answer after update= " + Instance.answer);
	}

	//returns string representing answer str for current question
	//note that answer strings == pos of answer within Question's zero-indexed choices List
	public string getCurrentAns() {
		return currQuestionStaticInstance.Instance.getQuestionAnswer();
	} 

	//returns true if input parameter == current question's answer 
	public bool isInputAnswer(string input) {
		if(input.Equals(getCurrentAns())) {
			return true;
		}
		return false;
	}
}
