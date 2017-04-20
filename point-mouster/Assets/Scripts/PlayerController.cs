using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	/*
	 * Controls the Player's movement, and jumping ability
	 * Deals with booleans for jumping, falling, and being grounded
	 * Also handles killing and respawning the player by delegating to the Level Manager
	 * 
	*/

	public GameButtons gameButton;// object so player can pause game
	//public BookScript currBook;

	public GoogleAnalyticsV4 googleAnalytics;

	public float moveSpeed; // how fast the player moves 
	public float jumpSpeed; // how high player jumps

	public int numBooks;
	public int maxBooks;

	public Text numBooksCollected;
	public Text wordDisplay;

	private Rigidbody2D myRigidBody; // rigid body used for moving and jumping
	private Animator myAnim; // animator to set values to cause animations

    private bool facingRight;

	public Vector3 respawnPosition; // position where player respawns

	//variables for checking if player is on ground or not
	public Transform groundCheck;
	public float groundCheckRadius; // radius of ground check space
	public LayerMask whatIsGround;

	public bool isGrounded; // know if player is on ground
	public bool isJumping; // know if player is jumping

	public AudioSource jumpSound; //sound of player jumping
	public AudioSource hurtSound; 
	public AudioSource rightSound;
	public AudioSource wrongSound;
	public AudioSource collectSound;
	public bool isPaused;

	public HealthBar health;

	public List<string> bookNames = new List<string> ();

	public enum GAMetricIdx {
			MODEOFPLAY = 1, 
			ANSWERINPUT = 2,
    		LEVELREACHED = 3,
    		WRONG_ANS = 4 ,
    		CORRECT_ANS = 5,
    		ERROR = 6, 
    		WON //not a metric but using for method
	};


	// Use this for initialization
	void Start () {
		//googleAnalytics.StartSession(); //must have before logging in every object's Start
        facingRight = true;
		health = FindObjectOfType<HealthBar> ();
		myRigidBody = GetComponent<Rigidbody2D> (); // rigid body for physics
		myAnim = GetComponent<Animator> (); // animator for anim changes
		//numBooksCollected.text = "Books: " + numBooks + "/" + maxBooks;
		isPaused = false;
		wordDisplay.text = "What will you learn today?";
		respawnPosition = new Vector3 (-9.42f, 0.56f, 0);

		if(gameButton == null){
			Debug.Log("gameButton == null");
			//gameButton = GameObject.FindGameObjectsWithTag("CanvasTag");
		}
		//googleAnalytics.StartSession();
	}

    void Flip(float horizontal)//flip to go backward
    {
        if((horizontal > 0 && !facingRight) || horizontal < 0 && facingRight)
           {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
	// Update is called once per frame
	void Update () {
		//pressing x closes canvas 
		if (Input.GetKeyDown (KeyCode.X)) {
			Debug.Log ("keydown, Clear WordDisplay");
			gameButton.ClearWordDisplay ();
		}


        float horizontal = Input.GetAxis("Horizontal");
        Flip(horizontal);
		isGrounded = Physics2D.OverlapCircle (groundCheck.position,groundCheckRadius,whatIsGround);

		//HORIZONTAL INPUT is either 0(no input), 1(going right), or -1(going left)
		float virtualAxisVal = Input.GetAxisRaw ("Horizontal");
		if (virtualAxisVal > 0f) {         //don't change y value     //checking RIGHT input
			myRigidBody.velocity = new Vector3 (moveSpeed, myRigidBody.velocity.y, 0f);
			//transform.localScale = new Vector3 (3f, 3f, 1f); // 3 b/c that's the sprites scale
		} else if (virtualAxisVal < 0f) {			//Checking LEFT input
			myRigidBody.velocity = new Vector3 (-moveSpeed, myRigidBody.velocity.y, 0f);
			//transform.localScale = new Vector3 (-3f, 3f, 1f);
		} else {			//NO INPUT
			myRigidBody.velocity = new Vector3 (0f, myRigidBody.velocity.y, 0f);
		}
        //Debug.Log(isGrounded);

		// checking jump input(space or up)
		if (Input.GetButtonDown ("Jump") && isGrounded) {
            //Debug.Log("JUMP");
			// put jumpSpeed in y to move up by moveSpeed
			myAnim.SetBool ("Jumping", true);
            jumpSound.Play();
            myRigidBody.AddForce(Vector3.up * jumpSpeed);
			//myRigidBody.velocity = new Vector3 (myRigidBody.velocity.x, jumpSpeed, 0f);
			isGrounded = true;
			
		}
		// if on the ground, set falling and jumping to false
		else if(isGrounded){ 
			myAnim.SetBool ("Jumping", false);
			isJumping = false;

		}

		//if p is pressed, pause the game
		if(Input.GetKeyDown (KeyCode.P)){
			if (!isPaused) {
				isPaused = true;
				gameButton.PauseGame ();
			} else {
				isPaused = false;
				gameButton.ResumeGame ();
			}

		}


		//Sets variables in order to change animations
		myAnim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
		myAnim.SetBool ("Grounded", isGrounded);

//		if (BookScript.bookControl.numBooks == BookScript.bookControl.maxBooks) {
//
//				BookScript.bookControl.setReviewWords ();
//				SceneManager.LoadScene ("Review1");
//		}

	}
	
	//method used to disable book
	private void disableComponent(Collider2D component) {
		Debug.Log("in disableComponent");
		component.GetComponent<SpriteRenderer> ().enabled = false;
		component.GetComponent<BoxCollider2D> ().enabled = false;
	}		

	//helper method: after user collects a book, another book is prepped for display
	private void prepNextWord() {
		Debug.Log("in prepNextWord, setting text of wordDisplay");
		wordDisplay.text = BookScript.bookControl.pickWord (); 
	}

	//Sent when another object enters a trigger collider attached to this object
	void OnTriggerEnter2D(Collider2D other){
		// handle respawn
		if (other.tag == "Book") {
			Debug.Log("player collided with book");
			BookScript.bookControl.updateBookTracker();
			collectSound.Play ();
			disableComponent(other);
			prepNextWord();
			Destroy (other.gameObject, 1);  //destroy book
			Time.timeScale = 0.0f;
		} else if (other.tag == "Door") {
			Debug.Log("user collided with door");
			if (BookScript.bookControl.numBooksCheck ()) {
				Debug.Log("user collected all facts");
//				if (Input.GetKeyDown(KeyCode.UpArrow)) {
				//ReviewScript.updateReviewNum();
				//ReviewScript.reviewNum++;
				//BookScript.bookControl.setReviewWords ();
				Debug.Log("Going to review for Level 1");
				SceneManager.LoadScene ("Review1");
//				}
			} else {
				Debug.Log ("ERROR User is at door but did not collect all books");
			}
		} else if (other.tag == "Door2") { //boss 1 door
			Debug.Log("collided with door2...boss 1 door");
			if (BossHealthBar.current == 0) {
				Debug.Log ("boss health=0... log event and load scene 2");
				recordLevelReached(GAMetricIdx.LEVELREACHED, GAMetricIdx.LEVELREACHED.ToString(), "2", (int)GAMetricIdx.LEVELREACHED);
				//Changing this so that we can get something to work
				BookScript.bookControl.ResetBooks ();
				SceneManager.LoadScene("Level2");
			} 
			Debug.Log("boss health != 0.... cannot load level2");
		} else if (other.tag == "level2Door") {
			Debug.Log("collided with level 2 door");
			if (BookScript.bookControl.numBooksCheck ()) {
				Debug.Log("user collected all facts");
//				if (Input.GetKeyDown(KeyCode.UpArrow)) {
				//ReviewScript.updateReviewNum();
				//ReviewScript.reviewNum++;
				//BookScript.bookControl.setReviewWords ();
				//Debug.Log("Going to review for Level 2");
				//SceneManager.LoadScene ("Review1");
				Debug.Log("Going to Boss Battle 2 scene");
				SceneManager.LoadScene("Boss Battle 2");
//				}
			} else {
				Debug.Log ("ERROR User is at door but did not collect all books");
			}
		} else if (other.tag == "Door3") {
			if (BossHealthBar.current == 0){
				recordLevelReached(GAMetricIdx.LEVELREACHED, GAMetricIdx.LEVELREACHED.ToString(), "0", 2);  //send 2 as metric to show only 3 levels in game
				SceneManager.LoadScene ("Win");
				//SceneManager.LoadScene ("Level3");
			}
			Debug.Log("boss health != 0, cannot go to win scene");
		} else if (other.tag == "Door4") {
			if (BossHealthBar.current == 0){
				recordLevelReached(GAMetricIdx.LEVELREACHED, GAMetricIdx.LEVELREACHED.ToString(), "0", 3);  //send 3 as metric to show only 3 levels in game
				SceneManager.LoadScene ("Win");
			}
			Debug.Log("boss health != 0, cannot go to win scene");
		} else if (other.tag == "KillPlane") {
			//	gameObject.SetActive (false);
			transform.position = respawnPosition;
			health.changeBar (5);
		} else if (other.tag == "Checkpoint") {
			respawnPosition = other.transform.position;
		} else {
			Debug.Log("collided with something that has no set behavior...tag= " + other.tag);
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		//handle player on moving platforms, so it doesn't slide off
		if (other.gameObject.tag == "MovingPlatform") {
			//make player's parent the platform to move player at same speed
			transform.parent = other.transform;
		}
	}

	void OnCollisionExit2D(Collision2D other){

		if (other.gameObject.tag == "MovingPlatform") {
			transform.parent = null; // stop making the platform a parent
		}
	}

	public void addBook(string bName){
		print("adding bookname to list");
		bookNames.Add (bName);
	}



	private void recordLevelReached(GAMetricIdx metric, string label, string value, int gaIdx) {
		string playerName = getPlayerName();

			googleAnalytics.LogEvent (new EventHitBuilder ()
			.SetEventCategory (label)   //is shown in Analytics
			.SetEventAction (playerName)             //is shown in Analytics
			.SetEventLabel (label)    //is shown in Analytics
			.SetEventValue (int.Parse(value)) //When we create mode for game, it should be entered HERE
            .SetCustomMetric (gaIdx, value) //unique index for ModeOfPlay metric in GA, version number
        );
	}



	//helper method for recordLevelReached
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




}
