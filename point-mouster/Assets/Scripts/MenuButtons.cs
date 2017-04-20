using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

	public static int currLevel;

	public static MenuButtons getLevel;

	public GoogleAnalyticsV4 googleAnalytics;

	//private static MenuButtons instance = null;


	public void Start()
	{
		currLevel = 1;
		//googleAnalytics.StartSession();
	}
	
	//must initialize all variables!!
	void Awake () 
	{
     /*	if(instance == null)
     	{
          	instance = this;
         	 DontDestroyOnLoad(gameObject);
     	}
     	else if(instance != this){
          Destroy(this.gameObject);
          return;
     	}*/
     	currLevel = 1;
 	}

	

	public void PlayGame()
	{	
		Debug.Log("in PlayGame() going to EnterName scene");
		SceneManager.LoadScene ("EnterName");
	}

    public void ResumeGame()
    {
        Debug.Log("in PlayGame() going to Level1 scene");
        SceneManager.LoadScene("Level1");
    }

    //return curr level
	public int returnLevel()
	{
		return currLevel;
	}


	public void PickBossToGoTo(){
		Debug.Log ("in PickBossToGoTo");
		switch(currLevel) {
			case 1  : Debug.Log ("curr level "  + currLevel);
					  currLevel++;
					  GoToBoss ();
					  break;
			case 2  : currLevel++;
					  GoToBoss2 ();
				      //GoToLevel3();
					  //GoToBoss2 ();
					  break;
			case 3	: Debug.Log ("curr level "  + currLevel);
					  currLevel++;
					  GoToBoss2 ();
					  break;
			default : Debug.Log ("ERROR curr level "  + currLevel);
					  GoToBoss ();
					  break;
		}	
	}

	public  void GoToBoss(){
		Debug.Log ("in GoToBoss");
		SceneManager.LoadScene ("Boss Battle");
	}

	public void GoToBoss2(){
		Debug.Log ("in GoToBoss2");
		SceneManager.LoadScene ("Boss Battle 2");
	}

	public void GoToBoss3(){
		Debug.Log ("in GoToBoss3");
		SceneManager.LoadScene ("Boss Battle 3");
	}

	public void GoToInstructions(){
		Debug.Log ("in GoToInstructions");
		SceneManager.LoadScene ("Instructions");
		// continue animation for instructions scene
		Time.timeScale = 1f;
	}

	public void StartOver(){
		Debug.Log ("in StartOver, going to TitleScreen");
		SceneManager.LoadScene ("TitleScreen");
	}

	public void Settings(){
		Debug.Log ("in Settings(), going to load Settings scene");
		SceneManager.LoadScene ("Settings");
	}

	public void QuitGame()
	{
		Debug.Log ("in QuitGame()");
        PlayerPrefs.DeleteKey("CurrentPlayer");
		Application.Quit ();
	}

	public void GoToWelcome(){
		Debug.Log ("in GoToWelcome(), going to load Welcome scene");
		SceneManager.LoadScene ("Welcome");
	}

	public void GoToEnterName(){
		Debug.Log ("in GoToEnterName(), going to load EnterName scene");
		SceneManager.LoadScene ("EnterName");
	}

	public void GoToReview1(){
		Debug.Log ("in GoToReview1(), going to load Review1 scene");
		SceneManager.LoadScene ("Review1");
	}

	private string getPlayerName() {
			/*
        	PlayerPrefs.GetString() returns the value corresponding to key (set in 1st param) in the preference file if it exists
        	get player name stored in PlayerPrefs by key or return string with error message
        	*/
        	string keyNotFoundHandler = "No name";
        	string playerName = PlayerPrefs.GetString ("CurrentPlayer", keyNotFoundHandler);
        	Debug.Log("player name = " + playerName);
        	return playerName;
	}

	//log that a new level has been reached
	private void LogLevelReached(int levelNumReached) {
		googleAnalytics.LogEvent (new EventHitBuilder ()
			.SetEventCategory ("LevelReached")
			.SetEventAction (getPlayerName())
			.SetEventLabel ("LevelReached")
			.SetEventValue (levelNumReached)
		); 
	}

	public void GoToLevel2(){
		Debug.Log ("in GoToLevel2()");
		LogLevelReached(2); 
		SceneManager.LoadScene ("Level2");
	}

	public void GoToLevel3(){
		Debug.Log ("in GoToLevel3()");
		LogLevelReached(3); 
		SceneManager.LoadScene ("Level3");
	}

	public void HomeScreen(){
		Debug.Log ("in HomeScreen()");
		SceneManager.LoadScene ("Home");
	}

	public void GoToTitle(){
		Debug.Log ("in GoToTitle()");
		SceneManager.LoadScene ("TitleScreen");
	}
	public void GoToCredits(){
		Debug.Log ("in GoToCredits()");
		SceneManager.LoadScene ("Credits");
	}

	//go to next scene
	public void GoToNextLevel() {
		Debug.Log ("in GoToNextLevel()");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // go to next level in game
	}

	public void TryAgain(){
		Debug.Log ("in TryAgain()");
		Debug.Log ("resetting books...");
		BookScript.bookControl.ResetBooks ();
		BookScript.bookControl.reviewIndices.Clear ();
		SceneManager.LoadScene ("EnterName");
	}
}
