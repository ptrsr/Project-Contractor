using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreManager : MonoBehaviour {
    
    private int _highScore = 0;
    
    public int SetHighScore { set { _highScore = value; } }
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



    private bool _startedPlacement = false;
    private bool _finished = false;
    private float _finishedAt = 0.0f;


    private HUDWin[] _huds;


    public int HighScore { get { return _highScore; } }
    
    void Start () {
        for (int i = 0; i < _subPositions.Length; i++)
        {
            _subPositions[i].position = new Vector3(_subPositions[i].position.x, _subPositions[i].position.y, 0);
        }
        _huds = GetComponentsInChildren<HUDWin>();
	}

    private void FixedUpdate()
    {
        
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            ShowEndHUD(true);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            ShowEndHUD(false);
        }
        if (_update)
        {
            if(_newPos1 != null && !_initialInterpolation1)
            {
                if (InterpolateWithScale(_treasureObj1, _newPos1.position, new Vector3(12.5f, 12.5f, 12.5f), 1)){
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
                    _done1 = true;
            }
            if (_initialInterpolation2)
            {
                if(Interpolate(_treasureObj2, _newPos2.position, 0.2f))
                    _done2 = true;
            }
            if (_initialInterpolation3)
            {
                if(Interpolate(_treasureObj3, _newPos3.position, 0.2f))
                    _done3 = true;
            }
            if (_initialInterpolation4)
            {
                if(Interpolate(_treasureObj4, _newPos4.position, 0.2f))
                    _done4 = true;
            }
            if(_done1 && _done2 && _done3 && _done4)
            {
                _update = false;
                ShowEndHUD(true);
            }
        }
    }
    //returns if done
    private bool InterpolateWithScale(GameObject obj, Vector3 pos, Vector3 scale, float distanceToDone)
    {
        obj.transform.position = Vector3.Lerp(obj.transform.position, pos, Time.deltaTime * 4);
        obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, scale, Time.deltaTime * 4);
        if(Vector3.Distance(obj.transform.position,pos) < distanceToDone)
            return true;
        else { return false; }
    }
    private bool Interpolate(GameObject obj, Vector3 pos, float distanceToDone)
    {
        obj.transform.position = Vector3.Lerp(obj.transform.position, pos, Time.deltaTime * 4);
        if (Vector3.Distance(obj.transform.position, pos) < distanceToDone)
            return true;
        else { return false; }
    }

    public void StartEndPlacement(SubMovement subPos)
    {
        if (_startedPlacement) return;
        _startedPlacement = true;
        subPos.Freeze(true);
        if (_treasure1)
        {
            PrepareForPlacement(_treasureObj1, subPos);
            _newPos1 = SideToSpawnOn();
        }else { _done1 = true; }
        if (_treasure2)
        {
            PrepareForPlacement(_treasureObj2, subPos);
            _newPos2 = SideToSpawnOn();
        }else { _done2 = true; }
        if (_treasure3)
        {
            PrepareForPlacement(_treasureObj3, subPos);
            _newPos3 = SideToSpawnOn();
        } else { _done3 = true; }
        if (_treasure4)
        {
            PrepareForPlacement(_treasureObj4, subPos);
            _newPos4 = SideToSpawnOn();
        }else { _done4 = true; }
        FindObjectOfType<FinalGem>().PlaceFinalGem();
        _update = true;
    }

    private void PrepareForPlacement(GameObject obj, SubMovement subPos)
    {
        obj.GetComponentInChildren<MeshRenderer>().enabled = true;
        obj.GetComponentInChildren<Light>().enabled = true;
        obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        obj.transform.position = subPos.transform.position;
    }

    private Transform SideToSpawnOn(){
        if (_left){
            _left = false;
            return _subPositions[1];
        }
        else{
            _left = true;
            return _subPositions[0];
        }
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
    


    public void ShowEndHUD(bool win)
    {
        HUDWin hud;
        if (win) { hud = _huds[0]; }
        else { hud = _huds[1]; }
        if (_treasureObj1)
            hud.Score1 = _treasureObj1.GetComponent<AddScoreOnCollision>().Score;
        if (_treasureObj2)
            hud.Score2 = _treasureObj2.GetComponent<AddScoreOnCollision>().Score;
        if (_treasureObj3)
            hud.Score3 = _treasureObj3.GetComponent<AddScoreOnCollision>().Score;
        if (_treasureObj4)
            hud.Score4 = _treasureObj4.GetComponent<AddScoreOnCollision>().Score;
        hud.ShowHud();
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
