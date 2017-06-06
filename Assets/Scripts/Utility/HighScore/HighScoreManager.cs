using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {

    [SerializeField]
    private int _highScore = 0;


    public int HighScore { get { return _highScore; } }
	void Start () {
		
	}
	

    public void AddScore(int score)
    {
        if(score > 0)
        {
            _highScore += score;
        }
    }
    public void SubtractScore(int score)
    {
        if(score < 0)
        {
            _highScore -= score;
        }
    }
}
