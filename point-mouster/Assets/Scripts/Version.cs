using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //to get scene name

public class Version : MonoBehaviour {
	public static Version Instance;
	private int version;

	// Use this for initialization
	void Start () {
		Debug.Log("in Version's Start and SceneName= " + SceneManager.GetActiveScene().name);
		version = InitVersion();
		
	}

	//called before component is enabled
	//ensure that this instance is not destroyed on load
	void Awake() {
		Debug.Log("Version is awake");
		if (Instance == null) {
			Debug.Log("Instance in Version is null");
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//returns true if param passed represents illegal range
	public bool isVerOutOfRange(int verNum) {
		if(verNum > 4 || verNum < 1) {
			Debug.Log("version num is out of range");
			return true;
		}
		return false;
	}

	//private method used to initialize
	private int InitVersion() {
		version = Random.Range(1,5); //Random.Range[inclusive, exclusive)
		Debug.Log("in InitVersion: version = " + version);
		return version;
	}

	//getter method
	public int getVersion() {
		Debug.Log("in Version.getVersion = " + version);
		Debug.Log("SceneName= " + SceneManager.GetActiveScene().name);
		return version;
	}
}