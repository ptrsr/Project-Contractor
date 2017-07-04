using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class DBconnection : MonoBehaviour {
	private Arguments argumentsScript;
	private string connectionURL;
	private string scoreURL = "insertScore.php?";

	void Awake() {
		argumentsScript = GetComponent<Arguments> (); 
		//GameObject.Find("SceneManager").GetComponent<Arguments>();
	}

	void Start() {
		connectionURL = argumentsScript.getConURL();
	}


	// When the game is over and you're ready to upload the score, call this method, e.g. as follows:
	// 		StartCoroutine( UploadScore (argumentsScript.getUserID (), argumentsScript.getGameID (), score) );

	public IEnumerator UploadScore(int userID, int gameID, int score) {
		string full_url = connectionURL + scoreURL + "userID=" + userID + "&gameID=" + gameID + "&score=" + score;
		print ("WWW: "+full_url);
		WWW post = new WWW(full_url);
		yield return post;
        SceneManager.LoadScene(0);

	}
}
