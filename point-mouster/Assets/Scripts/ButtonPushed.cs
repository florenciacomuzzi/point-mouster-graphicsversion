using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonPushed : MonoBehaviour {
	public string btnPushed;
	public HealthBar Health;
	public BossHealthBar bossHealth;
	public PlayerController player;
	public GameButtons clear;
	public FeedbackPanel fbPanel;
	public BossQuestions bossQ;

	private string playerName;

	public GoogleAnalyticsV4 googleAnalytics;

	float timer = 0;

	public string feedback;

	//for feedback
	public string[] correctFB;
	public string[] wrongFB;

	public enum POINTVALUE {
			FIRSTATTEMPT = 2, 
			SECONDATTEMPT = 1,
    		TOOMANYATTEMPTS = 0
	};

	// Use this for initialization
	void Start () {
		playerName = "error with name"; //should be updated with PlayerPrefs
		btnPushed = "-1"; //error code= -1 
		feedback = "";
		bossQ = FindObjectOfType<BossQuestions> ();
		fbPanel = FindObjectOfType<FeedbackPanel> ();
		Health = FindObjectOfType<HealthBar> ();
		bossHealth = FindObjectOfType<BossHealthBar> ();
		player = FindObjectOfType<PlayerController> ();
		clear = FindObjectOfType<GameButtons> ();

		correctFB = new string[] {
			"Way to go!",
			"You rock at this!",
			"Lookin’ good!",
			"Wow! I’m impressed!",
			"Keep at it!",
			"Good going!",
			"You were born a winner!",
			"Victory is yours!",
			"That was awesome!",
			"Great Job!",
			"Knew you could do it",
			"Great Job!",
			"You’re so good at this!",
			"That answer was perfect!"
		};

		wrongFB = new string[] {
			"That was terrible",
			"Better luck next time!", 
			"You really went with that answer?", 
			"FAIL"
		};

		

	}

	/*
	Pushed () checks answer, sets feedback display, and closes display after brief pause
	*/
	public void Pushed(){
		btnPushed = gameObject.name.ToString();
		Debug.Log ("user pressed on btn = " + btnPushed);

		ScoreManager.Instance.increaseNumAttempts(); //record attempt
		
		/*
        PlayerPrefs.GetString() returns the value corresponding to key (set in 1st param) in the preference file if it exists
        get player name stored in PlayerPrefs by key or return string with error message
        */
        string keyNotFoundHandler = "No name";
        playerName = PlayerPrefs.GetString ("CurrentPlayer", keyNotFoundHandler);
        Debug.Log("player name = " + playerName);

        // refer to unique index for custom metric given by GA
        int rightMetricIdx = 5; 
        int wrongMetricIdx = 4;
        int answerMetricIdx = 2;
        int successStatus = -1; //error if issue with checking answer
        int errorIdx = 6;
        const string errorStr = "-1";
        const int errorCode = -1;


        Color color = Color.yellow; //yellow = error
        
		/*
		if player chooses correct answer, 
		boss loses health and the question just answered is added to used questions arr
		*/
		int version = Version.Instance.getVersion();
		string questionInd = "-1";
		if(isBtnOutOfBounds(btnPushed) || Version.Instance.isVerOutOfRange(version) ) {
			color = errorProc();
			version = errorCode;      
			btnPushed = errorStr;
			questionInd = errorStr;
			successStatus = errorCode;
        } else {
        	questionInd = CurrentQuestion.currQuestionStaticInstance.getQuestionIndToString();
        	bool correctInput = CurrentQuestion.currQuestionStaticInstance.isInputAnswer(btnPushed);
			if (correctInput) {
				/*
				update score
				*/
				if(ScoreManager.Instance.getTimesPushed() == 1) //correct on the first try!
					ScoreManager.Instance.incrementScore((int)POINTVALUE.FIRSTATTEMPT);
				else if(ScoreManager.Instance.getTimesPushed() == 2)
					ScoreManager.Instance.incrementScore((int)POINTVALUE.SECONDATTEMPT);
				else 
					ScoreManager.Instance.incrementScore((int)POINTVALUE.TOOMANYATTEMPTS);


				Debug.Log("chose correct answer");
				player.rightSound.Play ();
				bossHealth.changeBar (10);
			
				color = Color.green;
				setCorrectFeedback(version);
				//BossQuestions.questionsUsed.Add (StompEnemy.ques);
				successStatus = rightMetricIdx;

				// record player name, version,  button number pushed, right or wrong, question number
				googleAnalytics.LogEvent( new EventHitBuilder() 
					.SetEventCategory ("Answer_Correct") //string
					.SetEventAction(playerName) //string
					.SetEventLabel("version") //string
					.SetEventValue(version) //int
					.SetCustomMetric(successStatus, CurrentQuestion.currQuestionStaticInstance.getQuestionIndToString()) //unique index for right or wrong answer metric in GA, question index
					.SetCustomMetric(answerMetricIdx, btnPushed) // unique index for playerAnswer metric in GA, btn number pushed
				);

        		fbPanel.enableFBPanel(feedback, color); 
        		clear.ClearQuestionDisplay ();
				Invoke("closePanel", 1);
				ScoreManager.Instance.resetAttempts(); 
			} else {
        		Debug.Log("chose wrong answer");
				player.wrongSound.Play ();
				//Health.changeBar (10);
			
				color = Color.red;
				setIncorrFeedback(version);
				successStatus = wrongMetricIdx;

				Debug.Log("feedback set: " + feedback);
        		// record player name, version,  button number pushed, right or wrong, question number
				googleAnalytics.LogEvent( new EventHitBuilder() 
					.SetEventCategory ("Answer_Incorrectly") //string
					.SetEventAction(playerName) //string
					.SetEventLabel("version") //string
					.SetEventValue(version) //int
					.SetCustomMetric(successStatus, CurrentQuestion.currQuestionStaticInstance.getQuestionIndToString()) //unique index for right or wrong answer metric in GA, question index
					.SetCustomMetric(answerMetricIdx, btnPushed) // unique index for playerAnswer metric in GA, btn number pushed
				);

        		fbPanel.enableFBPanel(feedback, color); 
        	}
        }
    }


    private Color errorProc() {
        	Debug.Log("some error occurred");
        	getErrorFB();
        	return Color.yellow;
    }

    //returns true if button number out of bounds
	private bool isBtnOutOfBounds(string button) {
		int btn = int.Parse(button);
		if(btn < 0 || btn > 3) {
			Debug.Log("error, button pressed out of bounds");
			return true;
		}
			return false;
	}


    //get string to display as feedback when error occurs
    private string getErrorFB(){
        return "setting error display";
    }

    //called by Pushed() to close FBPanel
	private void closePanel()
	{
		fbPanel.disableFBPanel ();
	}


	/*
	returns feedback phrase when player answers correctly
	is called by Pushed()
	*/
	private void setCorrectFeedback(int version){
		switch (version){
			case 1 : goto case 3; //positive when correct + neutral when incorr ver 
			case 3 : feedback = getPositiveFeedback(); //positive when correct & neg when incorrect ver
				     break;
			default: feedback = "correct"; //neutral only when correct or neutral when correct & incorrect
					 Debug.Log("getting neutral feedback for correct answer");
				     break;
		}
	}

	//gets random positive feedback -- helper method for setCorrectFeedback()
	private string getPositiveFeedback(){
			int size = correctFB.Length;
			int pos = Random.Range(0,size-1);
			return correctFB[pos];
	}


	private IEnumerator Pause(int p)
	{
		Debug.Log ("In pause");
		Time.timeScale = 0.1f;
		yield return new WaitForSeconds(p);
		Time.timeScale = 1;
		print ("End pause");
	}


	/*
	returns feedback phrase when player answers correctly
	is called by Pushed()
	*/
	private void setIncorrFeedback(int version){
		switch (version){
			case 2 : goto case 3; //negative when incorrect ver & neutral when correct
			case 3 : feedback = getNegFeedback();  //positive when correct & neg when incorrect ver
					 break;
			default: feedback = "incorrect"; //neutral only when correct or neutral when correct & incorrect
				 	 Debug.Log("getting neutral feedback for incorrect answer");
				 	 break;
		}
	}


	/*
	returns random negative feedback phrase when player answers incorrectly
	helper method for setIncorrFeedback()
	*/
	private string getNegFeedback(){
		int size = wrongFB.Length;
		int pos = Random.Range(0,size-1);
		return wrongFB[pos];
	}


		
}
