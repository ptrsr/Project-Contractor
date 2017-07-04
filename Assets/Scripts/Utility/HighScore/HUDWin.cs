using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Singleton;

public class HUDWin : MonoBehaviour {



    private int _highScoreToAdd = 0;
    private int _highScore = 0;
    private HighScoreManager _highscoreManager;
    private TimeManager _timeManager;
    private Arguments _museumData;

    private Canvas _canvas;
    private Text _text1;
    private Text _text2;
    private Text _text3;
    private Text _text4;
    private int _scoreTreasure1 = 0;
    private int _scoreTreasure2 = 0;
    private int _scoreTreasure3 = 0;
    private int _scoreTreasure4 = 0;
    public int Score1 { set { _scoreTreasure1 = value; } }
    public int Score2 { set { _scoreTreasure2 = value; } }
    public int Score3 { set { _scoreTreasure3 = value; } }
    public int Score4 { set { _scoreTreasure4 = value; } }
    private int _totalTreasureKind1 = 0;
    private int _totalTreasureKind2 = 0;

    private int _treasureKind1 = 0;
    private int _treasureKind2 = 0;
    private bool _animateHUD = false;

    private float _finishedAt = 0;
    private bool _finished = false;
    
    void Start () {

        _museumData = FindObjectOfType<Arguments>();
        _canvas = GetComponentInParent<Canvas>();
        _highscoreManager = _canvas.GetComponentInParent<HighScoreManager>();
        _timeManager = FindObjectOfType<TimeManager>();
        _text1 = GetComponentsInChildren<Text>()[0];
        _text2 = GetComponentsInChildren<Text>()[1];
        _text3 = GetComponentsInChildren<Text>()[2];
        _text4 = GetComponentsInChildren<Text>()[3];
        _canvas.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (_animateHUD)
        {
            if (_totalTreasureKind1 != 0) { _totalTreasureKind1 -= 10; _highScore += 10; if (_totalTreasureKind1 <= 0) _totalTreasureKind1 = 0; }
            if (_totalTreasureKind2 != 0) { _totalTreasureKind2 -= 10; _highScore += 10; if (_totalTreasureKind2 <= 0) _totalTreasureKind2 = 0; }
            _text2.text = " x " + _treasureKind1 + " \n    " + _totalTreasureKind1 + " punten";
            _text3.text = " x " + _treasureKind2 + " \n    " + _totalTreasureKind2 + " punten";
            if (_highScore == _highScoreToAdd)
            {
                _finished = true;
                _animateHUD = false;
                _highScore = _highScoreToAdd;
                _finishedAt = Time.timeSinceLevelLoad;
            }
            else if (_highScore > _highScoreToAdd)
            {
                _finished = true;
                _animateHUD = false;
                _highScore = _highScoreToAdd;
                _finishedAt = Time.timeSinceLevelLoad;
            }
            _text4.text = _highScore.ToString();
        }
        if (_finished)
        {
            if ((_finishedAt + 5.0f) < Time.timeSinceLevelLoad)
            {
                _museumData.UploadScore(_highScore);
                Volumes.Reset();
                Pings.Reset();
                DarkZones.Reset();
                _finished = false;
            }
        }
       
    }

    public void ShowHud()
    {
        _canvas.enabled = true;
        float time = Mathf.Floor(Time.timeSinceLevelLoad);
        int munites = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        if (_scoreTreasure1 != 0) { _treasureKind1++; }
        if (_scoreTreasure2 != 0) { _treasureKind1++; }
        if (_scoreTreasure3 != 0) { _treasureKind2++; }
        if (_scoreTreasure4 != 0) { _treasureKind2++; }
        string timestring = string.Format("{0}:{1:00}", munites, seconds);
        int remainingTime = _timeManager.TimeFromMuseum - Mathf.FloorToInt(time);
        int timeBonus = Mathf.FloorToInt(remainingTime * 100);
        _text1.text = "Jouw tijd: " + timestring + " seconden" +"\nTijd Bonus: " + timeBonus + " punten";
        _totalTreasureKind1 = _scoreTreasure1 + _scoreTreasure2;
        _totalTreasureKind2 = _scoreTreasure3 + _scoreTreasure4;
        _highScore = timeBonus;
        _highScoreToAdd = _totalTreasureKind1 + _totalTreasureKind2 + timeBonus;
        _text2.text = " x " + _treasureKind1 + " \n   " + _totalTreasureKind1 + " punten";
        _text3.text = " x " + _treasureKind2 + " \n   " + _totalTreasureKind2 + " punten";

        _highscoreManager.SetHighScore = _highScoreToAdd;
        _text4.text = _highScore.ToString();
        _animateHUD = true;
    }
}
