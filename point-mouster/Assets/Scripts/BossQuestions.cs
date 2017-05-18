using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class BossQuestions : MonoBehaviour {

	public GoogleAnalyticsV4 googleAnalytics;
	
	public class Question
	{
		public string question;
		public List<string> choices;
		public string answer; //pos of correct choice in zero-indexed choices List

		public Question (string q, string ch1, string ch2, string ch3, string ch4, string a)
		{
			question = q;
			choices = new List<string>();
			choices.Add(ch1);
			choices.Add(ch2);
			choices.Add(ch3);
			choices.Add(ch4);
			answer = a;
		}
		
		//default constructor
		public Question ()
		{
			question  = "defaultQuestion";
			choices = new List<string>();
			choices.Add("choice1");
			choices.Add("choice2");
			choices.Add("choice3");
			choices.Add("choice 4");
			answer = "-1";
		}

		//copy constructor
		public Question (Question obj)
		{
			this.setQuestionStr(obj.getQuestionStr());
			for(int pos = 0; pos < 4; pos++){
				this.setChoiceStr(pos, obj.getChoiceStr(pos));
			}
			this.setAnswerStr(obj.getQuestionAnswer());
		}
					
		//get choice -- choices List is zero-indexed
		public string setChoiceStr(int pos = -1, string str = "default choice str"){
			return choices[pos] = str;
		}

		private string setAnswerStr(string str = "default answer string") {
			answer = str;
			return answer;
		}

		//get choice -- choices List is zero-indexed
		public string getChoiceStr(int pos){
			return choices[pos];
		}

		//get question str
		public string getQuestionStr() {
			return question;
		}

		//public void setChoices(string ) 

		//set question str
		public void setQuestionStr(string question = "default question str") {
			this.question = question;
		}

		//get answer 
		public string getQuestionAnswer() {
			return answer;
		}

		//get answer 
		public int getQuestionAnswerInInt() {
			return int.Parse(answer);
		}
	}

	const int NumOptions = 13;
	const int NumChoices = 4;

	//const int NumOptions = 13;
	//public Text Question, Ans1, Ans2, Ans3;

	public int numWords;//20;//BookScript.bookControl.words.Length;
	//public char delim, delim2;
	//public char delim1, delim2, delim3, delim4, delim5;


	//public string wrdTmp, defTmp, currQuestion;
	public string questionTemp, choice1, choice2, choice3, choice4, answer, currQuestion;
	public static SortedDictionary<string,string> questionsAnswers; // map of questions and answers. Q is key, A is value

	public List<string> answerOptions; //holds words to test on
	public List<string> keyList;
	public static List<string> currAnswers; //Array used to make sure answers aren't repeated
	public string[] multiple_choice; //Array of multiple choice options
	public List<Question> questions;
	
	//public string[] negFB;

	/*
	questionsUsed has items added by ButtonPushed script
	whenever a player is done answering a question
	*/
	//public static List<Question> questionsUsed;
	public static List<int> indexUsed; // stores int positions of questions used
	
	// Use this for initialization
	void Start () {
		//googleAnalytics.StartSession(); //must have before logging in every object's Start

		questions = new List<Question>();

		/* Level 1 Questions */

		//question 1 
		questions.Add (new Question ("Choose the correct statement to define a 1d array of pointers to double with 10 elements. ",
			"*double ptrarr[10]", "double *[10] ptrarr", "double int *ptrarr[10]", "double *prtarr[10]", "3"));
		//question 2
		questions.Add (new Question ("Which of the following are pointer variables in the following definition?\n\tString *pName, name, address ",
			"pName, name, and address", "Name", "pName", "Name", "2"));
		//question 3 
		questions.Add (new Question ("What problem/error will likely result from the following code? int *p;\nfor (int i=0; i < 5; i++)\n\tp = new int[10];",
			"Dangling pointer", "Memory leak", "Type mismatch", "Segmentation fault", "1"));
		//question 4
		questions.Add (new Question ("Pointers are used to dynamically allocate memory space. This memory space must be freed once you are done using it to avoid memory leaks. Choose the correct statement to free a 1d array of pointers, ptrarr to double with 10 elements. \n",
			"free(ptrarr)", "delete[] ptrarr", "delete *ptrarr", " ~ *ptrarr", "1"));  
		//question 5
		questions.Add (new Question ("What problem/error will likely result from the following code?\n\tint * ptr = NULL;\n\t{ \nint x; \t\nptr = &x;\n}\n",
			"Memory Leak", "Type Mismatch", "Dangling Pointer", "Segmentation Fault", "2"));

		/* Level 2 Questions */

		//question 1 
		questions.Add (new Question ("Which symbol represents the indirection operator? ",
			"*", ".", "->", "&", "0"));
		//question 2
		questions.Add (new Question ("What would the following print?\nint num = 77;\nint *ptr = &num;\ncout << *ptr;",
			"Memory address of num", "Memory address of ptr", "77", "8566", "2"));
		//question 3 
		questions.Add (new Question ("Name an issue with the following code: \nint *ptr;\ncout << ptr;",
			"ptr var has not been initialized", "ptr is should be a pointer to double", "Cannot print variables", "There are no issues.", "0"));
		//question 4
		questions.Add (new Question ("You want make changes to a variable in a function. How would you pass the variable?",
			"Pass-by-value", "Pass-by-Reference", "Pass-by-const", "It is impossible.", "1"));  //end of level 2 questions
		//question 5
		questions.Add (new Question ("What symbol represents the address-of operator? ",
			"&", "*", "->", "None of the above", "0"));
		
		/* Level 3 Questions */

		//question 1
		questions.Add (new Question ("You declare an array, arr, of size 10. How would you access the value at the 4th index?",
			"arr[10]", "*(arr+4)", "It is impossible to access the value at the 4th position.", "*arr", "1")); 
		//question 2
		questions.Add (new Question ("Choose the correct statement to declare a pointer that points to another pointer ? ",
			"int *ptr", "int ptr**", "int *&ptr", "int ** ptr", "3")); 
		//question 3
		questions.Add (new Question ("Given the following declarations:\n\tint * ptr1;\n\tint * ptr2;\n\tptr1 = new int[5];\n\twhat statement will produce a shallow copy ? ",
			"&ptr2 = ptr1", "ptr2 = ptr1", "ptr2 = new int[5];\nfor(int i = 0; i<5; i++)\n\t{ptr2[i] = ptr1[i]};", "ptr2 = new int[5];\nfor(int i = 0; i<5; i++)\n\t{*ptr1[i] = *ptr2[i]}", "1")); 
		//question 4
		questions.Add (new Question ("Given the following declarations:\n\tint * ptr1;\n\tint * ptr2;\n\tptr1 = new int[5];\n\twhat statement will produce a deep copy ? ",
			"&ptr2 = ptr1", "ptr2 = ptr1", "ptr2 = new int[5];\nfor(int i = 0; i<5; i++)\n\t{ptr2[i] = ptr1[i];}", "'ptr2 = new int[5];\nfor(int i = 0; i<5; i++)\n\t{*ptr1[i] = *ptr2[i]}", "2")); 
		//question 5
		questions.Add (new Question (" Given the function: \nbool isFound(const char *chrPtr, char c){\n}\nWhich of the following would be the correct declaration for declaring a variable which will traverse 'const chrPtr *someText' ?\n",
			"const *ptr;", "const char ptr;", "const de *ptr;", "char *ptr;", "2"));

		/* Unused Questions */

//		questions.Add (new Question ("Choose the correct statement to free a 1d array of pointers, ptrarr to double with 10 elements.",
//			"free(ptrarr)", "delete ptrarr", "delete *ptrarr", " ~ *ptrarr", "1"));
//		questions.Add (new Question ("You write the following code:int n = 5; \nint *ptrToN = &n;\nWhat is the name of the operator in this fragment &n ?",
//			"The and operator", "Pointer", "int pointer operator ", "address-of operator", "3"));		
//		questions.Add (new Question ("You write the following code: \nint n = 5;\n int *ptrToN = &n;\nptrToN is of type ? ", 
//			"pointer to int", "int", "Std::string",  "char *",  "0")); 
//		questions.Add (new Question ("You write the following code: \nint n = 5; \nint *ptrToN = &n;\nWhat is the value of *ptrToN after the code finishes executing ? ",
//			"5", "hexadecimal address of n", "binary address of 5", "address of 5",  "0"));
//		questions.Add (new Question ("You write the following code: \nint n = 5; \nint *ptrToN = n;\nWhat is the value of ptrToN after the code finishes executing ? ",
//			"5", "hexadecimal address of n", "binary address of 5", "address of 5", "1")); 
//		questions.Add (new Question ("In the following code, baz = *foo \n'*' is called the ",
//			"dot operator",  "star operator", "dereference operator", "Asterisk operator", "2")); 
//		questions.Add (new Question ("* is the _________ operator, and can be read as 'value pointed to by' ? ",
//			"dereference", "value at start", "reference to malloc", "address-of", "0")); 
//		questions.Add (new Question ("Given the following declaration of a 2D integer array within a class-\n\tint ** nums;\n\tnums = new int*[5]\n\tfor(int i=0; i<5; i++)\n\t\tnums[i] = new int[10];\n\t How would you implement the destructor ? ",
//			"delete [ ] nums", "delete [ ][ ] nums", "delete *nums", "None of the above", "3"));

		indexUsed = new List<int>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public static int getQuestionID() {
		int size = indexUsed.Count;
		return indexUsed[size-1];
	}


	//checks if submitted correct answer for current question
	public bool checkInput(string btnPushed) {
		if(CurrentQuestion.currQuestionStaticInstance != null) {
			if(btnPushed.Equals(CurrentQuestion.currQuestionStaticInstance.getQuestInstance().getQuestionAnswer())) {
				return true; //user inputted correct answer
			}
		}
		return false; //instance not set or wrong answer inputted
	}

	

	//checks if index is in reviewIndicies
	public bool isInRevInd(int check){ 
		foreach (int i in BookScript.bookControl.reviewIndices) {
			if(i == check) { 
				return true;
			}
		}
		return false;
	}

	//checks if index is in reviewIndicies
	public bool isRevWord(string check){ 
		foreach (string i in BookScript.bookControl.reviewWords) {
			if(check == i) { 
				return true;
			}
		}
		return false;
	}

	public string getQuestionTempStr(){
		return questionTemp;
	}

	// returns index of randomly chosen question to be used as key in questions List
	public int pickQuestion(){
		Debug.Log("inside pickQuestion");
		isListEmpty(questions);
		Scene scene = SceneManager.GetActiveScene();
		Debug.Log("Active scene is " + scene.name);
		int chosenQuestIndex = -1;
		int loops = 0;
		do {
			chosenQuestIndex = generateNum();
			loops += 1;
		} while(loops < questions.Count && (isIndOutOfBounds(chosenQuestIndex) || isQuesUsed(chosenQuestIndex)) ); //generate another number if question already used or quest ind out of bounds
		indexUsed.Add(chosenQuestIndex); //add to questions used so it is not used again
		Debug.Log("chosen question Index: " + chosenQuestIndex);
		updateCurrentQuestion(chosenQuestIndex);	
		return chosenQuestIndex;
	}

	// updates current question with new question chosen
	private void updateCurrentQuestion(int chosenQuestIndex) {
		if(CurrentQuestion.currQuestionStaticInstance == null ){
			Debug.Log("cannot update CurrentQuestion due to error");
		}
		if(isIndOutOfBounds(chosenQuestIndex) ) {
			Debug.Log("ERROR in updateCurrentQuestion(), out of bounds index");
		}
		CurrentQuestion.currQuestionStaticInstance.updateQuestion(questions[chosenQuestIndex], chosenQuestIndex);
	}


	//returns random number based on level
	private int generateNum() {
		//check level 
		Scene scene = SceneManager.GetActiveScene();
		Debug.Log("Active scene is " + scene.name);
		int num = -1;
		switch(scene.name) {
			case "Boss Battle"	 : if(MenuButtons.currLevel ==2) goto case "L2";
								   num = Random.Range(0,5);
								   break;
			case "L2"		  	 : num = Random.Range(5,10);
								   break;
			case "Boss Battle 2" : num = Random.Range(10, 14);
								   break;
			case "Boss Battle 3" : num = Random.Range(10,14);
								   break;
			default              : Debug.Log ("behavior for this scene is not set, scene.name =" + scene.name);
								   num = Random.Range(0,5);
								   break;
		}
		Debug.Log("num generated= "+ num);
		return num;
	}

	/*
    return Question object found at questions[index] or empty question if questions List is empty or index out of bounds
	*/
	public Question getQuestAtInd(int index) {
		if(isIndOutOfBounds(index)) {
			return new Question(); 
		}
		return questions[index];
	} 

	//returns true if chosen question ind out of bounds
	public bool isIndOutOfBounds(int index) {
		if(index > questions.Count -1 || index < 0) {
			Debug.Log("index is out of bounds, idx= " + index);
			return true;
		}
		return false;
	}

	//returns true if list is empty
	public bool isListEmpty(List<Question> list) {
		if(list.Count < 1){
			return true;
		}
		return false;
	}

	//returns true if question has been used
	public bool isQuesUsed(int idx) {
		if(indexUsed.Contains(idx)){
			Debug.Log("question already used, quest idx= " + idx);
			return true;
		}
		return false;
	}

	
	//checks if player got question correct
	public bool checkAnswer(string playerAnswer){
		//print("answer is "+ questions[indexUsed[indexUsed.Count-1]].answer);
		string answer = CurrentQuestion.currQuestionStaticInstance.getQuestionAnswer();
		Debug.Log("answer received from CurrentQuestion= " + answer);
		if (answer.Equals (playerAnswer)) {
			return true;
		}
		return false;
	}

	//returns index representing pos of correct answer
	public int getAnswerIndex(int questionIndex){
		return questions[questionIndex].getQuestionAnswerInInt();
	}

	public void setDisplay() {

	}


	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("component with BossQuestions attached collided with other,gameObject.name= " + gameObject.name);
		if (other.tag == "Player") {
			Debug.Log("other.tag = " + other.tag);
			//parseCorrectWords ();
			//pickQuestion ();
		}
	}
	
}
