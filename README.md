# Point Mouster

This game was developed with Unity Version 5.5.1.1f1 
You may download this software for personal use at no cost: [https://store.unity.com](https://store.unity.com)



## Functionalities
C# scripts are used to define custom functionalities. 

To view scripts, *Assets* > *Scripts*

* BookScript: 
	* This script contains a string array `words` with educational facts used for teaching portions in each level. `words` contains the facts used by all levels.
	* `public void updateBookTracker()` is called to increment the number of facts/scientists collected by the user. This method calls `setBookTrackerDisplay()` to update the display.
	* `private void setBookTrackerDisplay(int curr = 0, int max = 0, bool useDefaultVals = true)` is a helper method called by `BookScript.updateBookTracker()` to set value of `UI.Text numBooksCollected`. Changing the value of numBooksCollected.text changes the value displayed on the canvas.
	* `private string createBookScore(string current, string maximum)` is a helper method used to initialize the display for the number of facts/scientists collected. User starts out with 0 facts collected and can collect a maximum of 5 facts per level.
	* `public string pickWord()` is called by *PlayerController* script when Player component collides with a scientist. Method calls `generateWord()` and checks that index has not been used. If index is unique, it adds the index to a list representing used indices before returning the result to the *PlayerController* script.
	* `private int generateWord()` is a helper method called by `pickWord()` that returns a random `int` to use as index into `words`. The method generates a random number within the specified range for the current level. Note that `Random.Range()` is bottom inclusive and top exclusive such that `Random.Range(0,5)` returns a random number in the range 0-4. 


