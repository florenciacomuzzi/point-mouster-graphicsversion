using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterNameScript : MonoBehaviour {

	public GoogleAnalyticsV4 googleAnalytics;

    public EnterNameScript instance;

    public Text NameInputFieldText;
    public Text RequiredText;

	void Start () {
        googleAnalytics = GoogleAnalyticsV4.getInstance();
        if(googleAnalytics == null) {
            Debug.Log("googleAnalytics == null");
        }
        //googleAnalytics.StartSession(); //must have before logging in every object's Start
	}

    void Awake() {
        

    }
	
	// Update is called once per frame
	void Update () {

	}

    //Checks to see if Input is empty and shows warning sign if it is
    //Saves it in PlayerPrefs and continues to Level1 scene otherwise
    public void Continue()
    {   
        string Name = getUserName ();
        Debug.Log("Name is " + Name);
        if (string.IsNullOrEmpty(Name))
        {
            RequiredText.text = "*Required";
            return;
        }
        /* 
        PlayerPrefs is class in UnityEngine
        Stores and accesses player preferences between game sessions
        SetString method sets the value of the preference identified by key
        key = "CurrentPlayer" value = Name
        */
        PlayerPrefs.SetString("CurrentPlayer", Name);
        Debug.Log("setting user name in PlayerPrefs...");

        int ver = Version.Instance.getVersion();
        Debug.Log("in EnterNameScript: version = " + ver);
        
		googleAnalytics.LogEvent (new EventHitBuilder ()
			.SetEventCategory ("ModeOfPlay")   //is shown in Analytics
			.SetEventAction (Name)             //is shown in Analytics
			.SetEventLabel (ver.ToString())    //is shown in Analytics
			.SetEventValue (ver) //When we create mode for game, it should be entered HERE
            .SetCustomMetric (1, ver.ToString()) //unique index for ModeOfPlay metric in GA, version number
        );
        Debug.Log("loading Level1 scene");
        SceneManager.LoadScene("Level1");
    }


    private string getUserName() {
        Debug.Log("getting input in NameInputFieldText");
        return NameInputFieldText.text.Trim();
    }

    public void BackToTitle()
    {   Debug.Log("in BackToTitle() loading TitleScreen...");
        SceneManager.LoadScene("TitleScreen");
    }
}
