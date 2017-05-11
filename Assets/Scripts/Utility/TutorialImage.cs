using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialImage : MonoBehaviour
{
    private Transform _chaseTarget;
    private Image[] _images;

    private bool _tutorialStart = false;
    private bool _init = false;
    private bool _scaled = false;

    private void Awake()
    {
        _images = new Image[3];
        //Get the HUD RawImage for camera rendering
        _images = GetComponentsInChildren<Image>();
        for(int i = 0; i < 3; i++)
        {
            _images[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1);
            _images[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);
        }

        SetPosition();
    }

    private void Update()
    {
        if (!_tutorialStart)
        {
            SetPosition();
        }
        else if(_init)
        {
           for (int i = 0; i < 3; i++)
           {
              BubblePos pos = _images[i].GetComponent<BubblePos>();
              if(pos != null)
              {
                  _images[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(
                                                                        _images[i].rectTransform.rect.width, pos.xScale, 0.2f));
                  _images[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(
                                                                        _images[i].rectTransform.rect.height, pos.yScale, 0.2f));
              }
            }
        }
    }

    private void SetPosition()
    {
        //Set position on hud based on where the character is located
        transform.position = CameraView.GetBorderPositionHUD(_chaseTarget.position);
    }


    public void SetImage()
    {

    }


    public void InitImage(bool left)
    {
        _tutorialStart = true;
        for(int i = 0; i < 3; i++)
        {
            BubblePos pos = _images[i].GetComponent<BubblePos>();
            if(pos != null)
            {
                if (left)
                    _images[i].rectTransform.anchoredPosition = new Vector2(pos.PositionOnLeft.x, pos.PositionOnLeft.y);
                  
                else
                {
                    _images[i].rectTransform.anchoredPosition = new Vector2(pos.PositionOnRight.x, pos.PositionOnRight.y);
                }
            }
        }
        _init = true;
    }
    
    public void DisableImages()
    {
        _tutorialStart = false;
        for (int i = 0; i < 3; i++)
        {
            BubblePos pos = _images[i].GetComponent<BubblePos>();
            if (pos != null)
            {
                _images[i].rectTransform.anchoredPosition = new Vector2(0, 0);
                _images[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(pos.xScale, 1, 1));
                _images[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(pos.yScale, 1, 1));

            }
        }
        _init = false;
    }

    public void SetChaseTarget(Transform transform)
    {
        _chaseTarget = transform;
    }
}