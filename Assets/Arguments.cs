using UnityEngine;
using System.Collections;
using System;

public enum PlayerGender {Male, Female};

public class Arguments : MonoBehaviour {
	
	private int userID;
	private int gameID;
	private string username;
	private int gametime;
	private string conURL;
	private PlayerGender gender;
	private int age;
    private TimeManager _timeManager;
    private DBconnection _upload;

	
	void Awake() {
        _timeManager = FindObjectOfType<TimeManager>();
        _upload = GetComponent<DBconnection>();
		// For testing, you can insert your default values here:
		userID = 1;
		gameID = 2;
		username = "TRUUS";
		gametime = 180;
		conURL = "http://www.serellyn.net/HEIM/php/";
		gender = PlayerGender.Female;
		age = 80;

		string[] arguments = Environment.GetCommandLineArgs();

		try {
			userID = Convert.ToInt32(arguments[2]);
			gameID = Convert.ToInt32(arguments[3]);
			username = arguments[4];
			gametime = Convert.ToInt32(arguments[5]);
            _timeManager.TimeFromMuseum = gametime;
			conURL = arguments[6];
			if (arguments[8]=="M")
				gender = PlayerGender.Male;
			else
				gender = PlayerGender.Female;
			age = Convert.ToInt32(arguments[9]);
		} catch (Exception error) {
			print ("Exception while parsing arguments: "+error.Message);
			print("Number of arguments: "+arguments.Length);
		}
	}
	
	public int getUserID() {
		return userID;	
	}
	
	public int getGameID() {
		return gameID;	
	}
	
	public string getUsername() {
		return username;
	}

	/// <summary>
	/// Returns the target game time in seconds. Typical values: 120, 180, or 360.
	/// </summary>
	public int getGameTime() {
		return gametime;
	}

	public string getConURL() {
		return conURL;
	}

	public PlayerGender getPlayerGender() {
		return gender;
	}

	/// <summary>
	/// Returns the player age in years.
	/// </summary>
	public int getPlayerAge() {
		return age;
	}

    public void UploadScore(int score)
    {
        StartCoroutine(_upload.UploadScore(getUserID(), getGameID(), score));
    }
}
