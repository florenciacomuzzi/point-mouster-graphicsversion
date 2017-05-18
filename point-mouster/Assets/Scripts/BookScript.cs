 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BookScript : MonoBehaviour {
	
	const int NUM_REVIEW_WORDS = 5;

	public static BookScript bookControl;
	// list of all the words
	public string[] words;
	public string[] facts;
	public List<string> reviewWords;
	
	// indices of already picked books so they aren't reused and can be accessed for review
	public List<int> reviewIndices; 
	public static List<int> indexUsed;

	public int numBooks;
	public int maxBooks;
	public Text numBooksCollected;

	void Awake(){
		if (bookControl == null) {
			Debug.Log("bookControl == null,do not destroy");
			DontDestroyOnLoad (gameObject);
			bookControl = this;
		} else {
            //updateBookTracker();

            Destroy (gameObject);
		}
		
	}

	// Use this for initialization
	void Start () {
		facts = new string[] {
			/*level 1 facts*/

			//fact 1
			"A pointer variable stores a memory address.", 
			//fact 2
			"Pointers must be declared before they can be used, just like a normal variable. The syntax of declaring a pointer is to place a * in front of the name. " +
			"A pointer is associated with a type too. For example, int *ptr;",
			//fact 3
			"All pointers in a program are likely going to occupy the same amount of space in memory (the size in memory of a pointer depends on the platform where the program runs).", 
			//fact 4
			"The delete[] operator \tdeallocates the memory block pointed to by ptr (if not null), releasing the storage space previously allocated to it by a call to operator new[] and rendering that pointer location invalid. For example, delete ptr1;",
			//fact 5
			"Memory leaks occur when new memory is allocated dynamically and never deallocated. " +
			"A dangling pointer is a pointer whose value is the address of memory that the program no longer owns).", 

			/*level 2 facts*/

			//fact 1
			"The & is an operator that returns the memory address of its operand. For example, if var is an integer variable, then &var is its address.", 
			//fact 2
			"When you declare a pointer variable, its content is not initialized. You need to initialize a pointer by assigning it a valid address. This is normally done via the address-of operator (&). " +
			"For example, if pNumber is an int pointer, *pNumber returns the int value 'pointed to' by pNumber. For example,\n\nint number = 88;\n\nint * pNumber;\n\npNumber = &number;", 
			//fact 3
			"The indirection operator is *. This operator returns the value stored in the address kept in the pointer variable. For example, the following would print “99”:\n\nint *pNumber = 99;\n\ncout << *pNumber << endl;", 
			//fact 4
			"In C/C++, by default, arguments are passed into functions by value (except arrays). That is, a clone copy of the argument is made and passed into the function. Changes to the clone copy inside the function do not affect the original argument.", 
			//fact 5
			"You may wish to modify the original copy directly. Do this by passing a pointer of the object into the function, known as pass-by-reference. For example,\n\nint number = 8;\n\nsquare(&number);\n\n…{\n\nvoid square(int * pNumber)}", 

			/*level 3 facts*/

			//fact 1
			"In C/C++, an array's name is a pointer, pointing to the first element (index 0) of the array. For example, suppose that numbers is an int array, numbers is a also an int pointer, pointing at the first element of the array. " +
			"That is, numbers is the same as &numbers[0]. Consequently, *numbers is number[0]; *(numbers+i) is numbers[i].\n", 
			//fact 2
			"As we know, the value of a pointer is an address, so we can have a pointer that holds the address of another pointer. In other words, we can have a pointer that points to another pointer. " +
			"This is helpful when iterating through arrays and working with dynamic 2D arrays. To declare a pointer to another pointer, simply add a second asterisk: int ** ptr.\n", 
			//fact 3
			"Given a pointer, ptr, that points to an array of five elements: int * ptr;  ptr = new int[5]; We want another pointer to point to an array of the same five elements: int * copy_ptr;  A shallow copy does not create an new array in memory. " +
			"Instead, it creates another pointer that points to the pre-existing array. This is accomplished by a simple assignment statement: copy_ptr = ptr;\n", 
			//fact 4
			"Given a pointer, ptr, that points to an array of five elements: int * ptr;  ptr = new int[5]; We want another pointer, copy_ptr, to point to an array of the same five elements. " +
			"int * copy_ptr; A “deep copy” of a dynamic array creates an identical array in a different part of memory. To do this, first you allocate new memory space (copy_ptr = new int[5];), then copy each element in one at a time: for(int i=0; i<5; i++){copy_ptr[i] = ptr[i];}\n", 
			//fact 5
			"The keyword const can be used on pointer parameters, like we do with references. It is used for a similar situation -- it allows parameter passing without copying anything but an address, but protects against changing the data (for functions that should not change the original). " +
			"For example, \n\nconst double * v\n\nThis establishes v as a pointer to an object that cannot be changed through the pointer v.\n"
		};

		maxBooks = InitMaxBooksVal(); //max num of books to collect	
		numBooks = InitNumBooks(); //init to 0 books collected

						
		reviewIndices = new List<int>();
		reviewWords = new List<string> ();
		numBooksCollected.text = createBookScore("0","5");
		if (GameObject.Find("BookScore") != null)
			numBooksCollected = GameObject.Find("BookScore").GetComponent<Text>();
		else
			Debug.Log("Find bookscore is null");
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//setter method - maxBooks 5 by default
	private int InitMaxBooksVal(int val = 5) {
		Debug.Log("in InitMaxBooksVal,setting maxBooks=" + val);
		maxBooks = val;
		return maxBooks;
	}

	//getter method
	private int getMaxBooksVal() {
		return maxBooks;
	}

	//initialize number of books collected to 0
	private int InitNumBooks() {
		Debug.Log("in InitNumBooks(), initial num books collected= 0");
		return 0;
	}

	//default increment = 0 
	private void setNumBooksCollected(int increment = 0) {
		numBooks += increment;
	}

	private int getNumBooksCollected() {
		return numBooks;
	}

	//increment book count and change the text
	public void updateBookTracker(){
		Debug.Log("in updateBookTracker");
		setNumBooksCollected(1); //increment count by 1
		Debug.Log("numBooks" + getNumBooksCollected());
		setBookTrackerDisplay(-1, -1, false); //set to false so dummy params not used!!
	}

	/*
	called by updateBookTracker
	initialize to 0/0 by setting useDefaultVals= true 
	will use default args if none provided
	set useDefaultVals to false if do not want dummy params to set score display!
	*/
	private void setBookTrackerDisplay(int curr = 0, int max = 0, bool useDefaultVals = true) {
		Debug.Log("in setBookTrackerDisplay");
		int current = curr;
		int maximum = max;
		if(!useDefaultVals) {
			Debug.Log("score display not set custom");
			current = getNumBooksCollected();
			maximum = getMaxBooksVal();
		}
		numBooksCollected = GameObject.Find("BookScore").GetComponent<Text>();
        numBooksCollected.text = createBookScore(current.ToString(), maximum.ToString());
	}

	//returns num of books collected to display as string
	private string createBookScore(string current, string maximum) {
		Debug.Log("creating book score with x/y, " + current + maximum);
		// In local variables (i.e. within a method body)
        // you can use implicit typing.
		var temp = "Facts: " + current + "/" +  maximum;
		return temp;
	}

	//returns a random number index based on level
	private int generateWord() {
		Scene scene = SceneManager.GetActiveScene();
		Debug.Log("Active scene is " + scene.name);
		int chosenWordIndex = -1;
		switch (scene.name) {
			case "Level1" : chosenWordIndex = Random.Range(0,5);
			    			break;
			case "Level2" : chosenWordIndex = Random.Range(10,15);
			    			break;
			case "Level3" : chosenWordIndex = Random.Range(10,15);
			    			break;
			default		  : Debug.Log("behavior is not set for this level/scene");
			    			chosenWordIndex = Random.Range(0,5);
			    			break;
		}
		if(chosenWordIndex == -1) {
			Debug.Log("some error occurred in generating word number=" + -1);
		} 
		return chosenWordIndex;
	}


	/*
	pickWord is called by PlayerController script's prepNextWord() 
	when player collides with a scientist/book
	method generates random number 
	*/
	public string pickWord(){
		Debug.Log("in pickWord after colliding with book");

		int pos = -1;
		pos = generateWord();
		int loops = 0;
		while(isWordUsed (pos) && (loops < (facts.Length -1))) {
			loops++;
			pos = generateWord();
		}
		Debug.Log("in pickWord, total while loops= " + loops);
		reviewIndices.Add (pos); // add index to the list so it won't be picked more than once
		reviewWords.Add (facts[pos]); // add only the words that were picked;
		return facts[pos];
	}


	public void ResetBooks() {
		Debug.Log ("in ResetBooks...");
		numBooks = InitNumBooks(); //set numBooks = 0
		reviewWords.Clear ();  //clears list of review words used
		//reviewIndices.Clear ();
	}

	//checks if word has been used with pos key
	public bool isWordUsed (int wordIndex){
		foreach (int i in reviewIndices) {
			if ( wordIndex == i ) {
				Debug.Log("word with idx= " + i + " is used");
				return true;
			}
		}
		Debug.Log("word has not been used");
		return false;
	}

	
	public List<string> getReviewWords(){
		Debug.Log("in getReviewWords(),returns List<string>");
		return reviewWords;
	}

	//returns true if all books have been collected
	public bool numBooksCheck(){
		Debug.Log("in numBooksCheck()");
		if(getNumBooksCollected() == 0) {
			return false;
		}
		return ((getNumBooksCollected() % getMaxBooksVal()) == 0 ) ? true : false;
	}



}
