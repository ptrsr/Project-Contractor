using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Case
{
    BumbWall = 0,
    HitByShark = 1,
    HitByEel = 2,
    HitByOctopus = 3,
    OxigenFinished = 4
};

public class TutorialManager : MonoBehaviour {

    [SerializeField]
    private int _delayForEachTutorialInSec = 3;

    private TutorialImage _tutorialImage;
    private bool _tutorialOn = true;
    // Use this for initialization
    void Start() {
        _tutorialImage = GetComponentInChildren<TutorialImage>();
    }

    // Update is called once per frame
    void Update() {
        if (_tutorialImage == null) return;
        if (Input.GetKeyDown(KeyCode.T))
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
        if (!_tutorialOn)
        {
            _tutorialImage.InitImage(true);
            _tutorialOn = true;
            StartCoroutine(Delay());
        }
        switch (type)
        {
            case Case.BumbWall:
                //Set that image that the tutorial will use _tutorialImage.SetImage();
                break;
            default:
                break;
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
