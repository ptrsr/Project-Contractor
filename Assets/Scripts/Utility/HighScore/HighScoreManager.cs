using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour {

    [SerializeField]
    private int _highScore = 0;
    [SerializeField]
    private Transform[] _pillarPositions;
    [SerializeField]
    private Transform[] _subPositions;

    private bool _treasure1 = false;
    private bool _treasure2 = false;
    private bool _treasure3 = false;
    private bool _treasure4 = false;

    private GameObject _treasureObj1;
    private GameObject _treasureObj2;
    private GameObject _treasureObj3;
    private GameObject _treasureObj4;

    private Transform _newPos1 = null;
    private Transform _newPos2 = null;
    private Transform _newPos3 = null;
    private Transform _newPos4 = null;

    private bool _update = false;
    private bool _left = true;

    private bool _initialInterpolation1 = false;
    private bool _initialInterpolation2 = false;
    private bool _initialInterpolation3 = false;
    private bool _initialInterpolation4 = false;

    private bool _done1 = false;
    private bool _done2 = false;
    private bool _done3 = false;
    private bool _done4 = false;

    private Canvas _canvas;
    private Text _text1;
    private Text _text2;

    private bool _startedPlacement = false;
    private bool _finished = false;
    private float _finishedAt = 0.0f;


    public int HighScore { get { return _highScore; } }
    
    void Start () {
        for (int i = 0; i < _subPositions.Length; i++)
        {
            _subPositions[i].position = new Vector3(_subPositions[i].position.x, _subPositions[i].position.y, 0);
        }
        _canvas = GetComponentInChildren<Canvas>();
        _text1 = _canvas.GetComponentsInChildren<Text>()[0];
        _text2 = _canvas.GetComponentsInChildren<Text>()[1];
        _canvas.enabled = false;
	}

    private void Update()
    {
        if (_update)
        {
            if(_newPos1 != null && !_initialInterpolation1)
            {
                if (InterpolateWithScale(_treasureObj1, _newPos1.position, new Vector3(12.5f, 12.5f, 12.5f), 0.2f)){
                    Debug.Log("done scaling treasure 1");
                    _newPos1 = _pillarPositions[0];
                    _initialInterpolation1 = true;
                }
            }
            else if (_newPos2 != null && !_initialInterpolation2)
            {
                if (InterpolateWithScale(_treasureObj2, _newPos2.position, new Vector3(12.5f, 12.5f, 12.5f), 1))
                {
                    _newPos2 = _pillarPositions[1];
                    _initialInterpolation2 = true;
                }
            }
            else if (_newPos3 != null && !_initialInterpolation3)
            {
                if (InterpolateWithScale(_treasureObj3, _newPos3.position, new Vector3(12.5f, 12.5f, 12.5f), 1))
                {
                    _newPos3 = _pillarPositions[2];
                    _initialInterpolation3 = true;
                }
            }
            else if (_newPos4 != null && !_initialInterpolation4)
            {
                if (InterpolateWithScale(_treasureObj4, _newPos4.position, new Vector3(12.5f, 12.5f, 12.5f), 1))
                {
                    _newPos4 = _pillarPositions[3];
                    _initialInterpolation4 = true;
                }
            }
            if (_initialInterpolation1)
            {
                if(Interpolate(_treasureObj1, _newPos1.position, 0.2f))
                {
                    _done1 = true;
                }
            }
            if (_initialInterpolation2)
            {
                if(Interpolate(_treasureObj2, _newPos2.position, 0.2f))
                {
                    _done2 = true;
                }
            }
            if (_initialInterpolation3)
            {
                if(Interpolate(_treasureObj3, _newPos3.position, 0.2f))
                {
                    _done3 = true;
                }
            }
            if (_initialInterpolation4)
            {
                if(Interpolate(_treasureObj4, _newPos4.position, 0.2f))
                {
                    _done4 = true;
                }
            }
            if(_done1 && _done2 && _done3 && _done4)
            {
                _update = false;
                _finished = true;
                _finishedAt = Time.time;
                ShowEndHUD();
            }
        }
        if (_finished)
        {
            if((_finishedAt + 5.0f) < Time.time)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    //returns if done
    private bool InterpolateWithScale(GameObject obj, Vector3 pos, Vector3 scale, float distanceToDone)
    {
        obj.transform.position = Vector3.Lerp(obj.transform.position, pos, 0.1f);
        obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, scale, 0.3f);
        if(Vector3.Distance(obj.transform.position,pos) < distanceToDone)
        {
            return true;
        }
        else { return false; }
    }
    private bool Interpolate(GameObject obj, Vector3 pos, float distanceToDone)
    {
        obj.transform.position = Vector3.Lerp(obj.transform.position, pos, 0.1f);
        if (Vector3.Distance(obj.transform.position, pos) < distanceToDone)
        {
            return true;
        }
        else { return false; }
    }

    public void StartEndPlacement(SubMovement subPos)
    {
        if (_startedPlacement) return;
        _startedPlacement = true;
        subPos.Freeze(true);
        Debug.Log("StartPlacement");
        if (_treasure1)
        {
            _treasureObj1.GetComponentInChildren<MeshRenderer>().enabled = true;
            _treasureObj1.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            _treasureObj1.transform.position = subPos.transform.position;
            if (_left)
            {
                _newPos1 = _subPositions[1];
                _left = false;
            }
            else
            {
                _newPos1 = _subPositions[0];
                _left = true;
            }
        }else { _done1 = true; }
        if (_treasure2)
        {
            _treasureObj2.GetComponentInChildren<MeshRenderer>().enabled = true;
            _treasureObj2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            _treasureObj2.transform.position = subPos.transform.position;
            if (_left)
            {
                _newPos2 = _subPositions[1];
                _left = false;
            }
            else {
                _newPos2 = _subPositions[0];
                _left = true;
            }
        }else { _done2 = true; }
        if (_treasure3)
        {
            _treasureObj3.GetComponentInChildren<MeshRenderer>().enabled = true;
            _treasureObj3.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            _treasureObj3.transform.position = subPos.transform.position;
            if (_left)
            {
                _newPos3 = _subPositions[1];
                _left = false;
            }
            else
            {
                _newPos3 = _subPositions[0];
                _left = true;
            }
        } else { _done3 = true; }
        if (_treasure4)
        {
            _treasureObj4.GetComponentInChildren<MeshRenderer>().enabled = true;
            _treasureObj4.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            _treasureObj4.transform.position = subPos.transform.position;
            if (_left)
            {
                _newPos4 = _subPositions[1];
                _left = false;
            }
            else
            {
                _newPos4 = _subPositions[0];
                _left = true;
            }
        }else { _done4 = true; }
        _update = true;
    }


    public void Treasure1Pickup(GameObject obj)
    {
        _treasureObj1 = obj;
        _treasure1 = true;
    }
    public void Treasure2Pickup(GameObject obj)
    {
        _treasureObj2 = obj;
        _treasure2 = true;
    }
    public void Treasure3Pickup(GameObject obj)
    {
        _treasureObj3 = obj;
        _treasure3 = true;
    }
    public void Treasure4Pickup(GameObject obj)
    {
        _treasureObj4 = obj;
        _treasure4 = true;
    }


    private void ShowEndHUD()
    {
        _canvas.enabled = true;
        float time = Mathf.Floor(Time.time);
        time = time / 60;
        string timestring = string.Format("{0:0.00}", time);
        _text1.text = "HighScore: " + _highScore;
        _text2.text = "Time to Finish: " + timestring;
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
