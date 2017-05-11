using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [SerializeField]
    private int _delayForEachTutorialInSec = 1;

    private TutorialImage _tutorialImage;
    private bool _tutorialOn = true;
    // Use this for initialization
    void Start() {
        _tutorialImage = GetComponentInChildren<TutorialImage>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BumbIntoWall();
        }
        if (Input.GetMouseButtonDown(0))
        {
            _tutorialImage.DisableImages();
            _tutorialOn = false;
        }
    }

    public void BumbIntoWall()
    {
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
