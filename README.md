# Point Mouster

This game was developed with Unity Version 5.5.1.1f1 
You may download this software for personal use at no cost: [https://store.unity.com](https://store.unity.com)


## Requirements
* Every scene that logs a Google Analytics event must contain a copy of the `GAv4` prefab. Go to *Assets > Plugins > GoogleAnalyticsV4* and drag the prefab into the Hierarchy of the scene logging events. 
	* A script that logs an event must include `public GoogleAnalyticsV4 googleAnalytics;` member. To initialize, drag that scene's `GoogleAnalyticsV4` instance into the appropriate field in the Inspector.

* Static instances of classes within scripts can be used to access the script instance from anywhere in the Hierarchy. For example, ClassName.nameofStaticInstance.



##FAQs
* **What is the game flow?** The first scene asks the user to enter their email. After clicking on the CONTINUE button, the Level 1 scene is loaded. First, there are two message pop-ups with instructions. Then, the user must move forward and collide with every scientist to collect a fact. Once all facts have been collected, the user moves to a Review scene. After that, a Boss Battle scene is loaded. The user gets unlimited tries for every question. The feedback is determined by the version number the user is playing. After answering all questions the user moves to a Level 2 scene for more facts with a Boss Battle scene immediately after. 

* **How can I change the questions?** All questions are stored within one list in the BossQuestions script. Random questions are chosen from a range corresponding to the current level. Modify the list of questions and ensure the ranges align. Each Question object has a string member for: 
	** the question text 
	** individual text for each multiple choice 
	** a text representing the position of the correct multiple choice answer i.e. Question foo has 4 multiple choice options numbered from 0-3 and the correct answer is at position <some num>

* **How can I change the graphic facts?** All fact graphic files are stored in the directory *Assets > FactImages*. In the Hierarchy, go to the appropriate level scene and find the Canvas object. The drop-down for this object will show a generic `LevelOneFacts` or `LevelTwoFacts` components. The child components represent specific facts. Drag the graphic file into the Sprite-Renderer.








## Functionalities
C# scripts are used to define custom functionalities. 

To view scripts, *Assets* > *Scripts*

* **BookScript** 
	* This script contains a string array `words` with educational facts used for teaching portions in each level. `words` contains the facts used by all levels.

	* `public void updateBookTracker()` is called to increment the number of facts/scientists collected by the user. This method calls `setBookTrackerDisplay()` to update the display.

	* `private void setBookTrackerDisplay(int curr = 0, int max = 0, bool useDefaultVals = true)` is a helper method called by `BookScript.updateBookTracker()` to set value of `UI.Text numBooksCollected`. Changing the value of numBooksCollected.text changes the value displayed on the canvas.

	* `private string createBookScore(string current, string maximum)` is a helper method used to initialize the display for the number of facts/scientists collected. User starts out with 0 facts collected and can collect a maximum of 5 facts per level.

	* `public string pickWord()` is called by *PlayerController* script when Player component collides with a scientist. Method calls `generateWord()` and checks that index has not been used. If index is unique, it adds the index to a list representing used indices before returning the result to the *PlayerController* script.

	* `private int generateWord()` is a helper method called by `pickWord()` that returns a random `int` to use as index into `words`. The method generates a random number within the specified range for the current level. Note that `Random.Range()` is bottom inclusive and top exclusive such that `Random.Range(0,5)` returns a random number in the range 0-4. 

	* `public bool numBooksCheck()` returns true if all facts have been collected in the current level. 

* **BossHealthBar**
	* This script is attached to a child component of `Canvas` in BossBattle, BossBattle2, and BossBattle3 scenes.
	* `public void changeBar(int change)` is used to decrement the health bar e.g. when a question is answered correctly. The boss is destroyed when health reaches 0.

* **BossQuestions**
	* This script is used to select questions for display in Boss Battle scenes.
	* This script is attached to `QuestionCanvas` component in Boss Battle scenes.
	* Contains inner class `Question` with *string* members representing a question text, choices 1-4, and the answer.
	* `questions` is a *List* of type `Question` whose members represent the list of all questions available in the game.

	* `public int pickQuestion()` returns index of randomly chosen question to be used as key in `questions` *List*. Method calls helper method `generateNum()` and verifies that index returned has not been used.

	* *private int generateNum()* is a helper method called by `pickQuestion()` to return a random `int` index into `questions`. Random number is generated from a range specific to the current level.

	* `public bool isQuesUsed(int idx)` returns true if parameter `idx` represents a question that has already been used.

* **ButtonPushed**
	* Script is attached to `QuestionCanvas` component and to every single *choice* child component of e.g. `0,1,2,3,` in Boss Battle scenes.
	* Used for `on Click()` handling. For every choice component, select the handler in the Inspector.
	* Script contains string arrays `correctFB` and `wrongFB` and string `feedback`.

	* `public void Pushed()` is the handler used when the user clicks on a question choice. Method checks answer, sets feedback display based on the version the user is playing, and closes question display after brief pause. 
		* Input is checked for correctness against the answer member of `static CurrentQuestion`. Feedback is generated randomly.
		* If player chooses correct answer, boss loses health display text color is set to green and display text is set to a feedback string determined by the version being played. A Google Analytics event *Answer_Correct* is logged. Display is disabled after a brief pause.
		* For incorrect input, color of feedback text is set to red and feedback text is set based on version. A Google Analytics event `Answer_Incorrectly` is logged. **Question display is not disabled.** The user has unlimited chances to answer the question correctly and must do so to make forward progress. 

* **CurrentQuestion**
	* Contains instance of `BossQuestions.Question` representing the current question being asked. Class is used by `ButtonPushed` script to determine whether user entered correct answer.
	* Static member `instance` of type *EnterNameScript* persists throughout entire game.

* **EnterNameScript** 
	* Script is attached to *Canvas* component in EnterName scene.
	* `public void Continue()` stores user inputted email address, generates a random version number to be used for the rest of the game, logs Google Analytics event `ModeOfPlay`, and loads Level1 scene.
	* ***GoogleAnalyticsV4* instance must be initialized.** 

* **FeedbackPanel**
	* Script is attached to *FBPanel*, a child component of *Canvas*, in every Boss Battle scene.
	* `public void enableFBPanel(string feedback, Color color)` is called by *ButtonPushed* script to enable feedback panel with text and text color specified by *feedback* and *color* parameters respectively.
	* `public void disableFBPanel ()` sets feedback text to empty string and disables the component.

* **GameButtons**
	* Script handles functionality for pausing/resuming game and clearing displays for both facts and questions.
	* Attached to all *Canvas* components in all scenes.
	* `public void ClearQuestionDisplay()` is called by `ButtonPushed.Pushed()` after correct answer input and disables display of multiple choice buttons, disables entire question display, and resumes the game before returning.

* **HealthBar** 
	* Equivalent of *BossHealthBar* for *Player* component.

* **JITScript** 



	
	


    


		
	

	
	



