using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum Case
{
    BumbWall = 0
};

public class TutorialManager : MonoBehaviour {

    [SerializeField]
    private int _delayForEachTutorialInSec = 3;

    private TutorialImage _tutorialImage;
    private bool _tutorialOn = true;
    // Use this for initialization
    void Start() {
        _tutorialImage = GetComponentInChildren<TutorialImage>();
        //Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>("TutorialTextures/Bubbles.png");
        //Debug.Log(texture.name);
    }

    // Update is called once per frame
    void Update() {
        if (_tutorialImage == null) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TutorialEvent(Case.BumbWall);
        }
        if (Input.GetMouseButtonDown(0))
        {
            _tutorialImage.DisableImages();
            _tutorialOn = false;
        }
    } 

    public void TutorialEvent(Case type)
    {
        switch (type)
        {
            case Case.BumbWall:
                break;
            default:
                break;
        }
        if (!_tutorialOn)
        {
            _tutorialImage.InitImage(true);
            _tutorialOn = true;
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delayForEachTutorialInSec);
        if (_tutorialOn)
        {
            _tutorialImage.DisableImages();
            _tutorialOn = false;
        }
    }
}
