using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Oxygen : MonoBehaviour {

    private Slider _slider;
    private Image _image;
    private Color _currentColor;
    private SubMovement _submarine;
    [SerializeField]
    private Image _sliderImage = null;
    private bool _surface = false;
    private float _alpha = 0;
	private void Start () {
        _slider = GetComponent<Slider>();
        _image = GetComponentInParent<Image>();
        _currentColor = _image.color;
        _submarine = FindObjectOfType<SubMovement>();
        AudioSource s = FindObjectOfType<AudioSource>();
    }

    private void Update()
    {
        UpdateColor();
        if(_slider.value == 0)
        {
            _surface = true;
        }
        if (_surface)
        {
            _alpha += 5.0f;
            _image.color = new Color(_currentColor.r,_currentColor.g,_currentColor.b,_alpha/255.0f);
            if(_alpha >= 255.0f)
            {
                _submarine.Surface();
                _slider.gameObject.SetActive(true);
                _slider.value = _slider.maxValue;
                _surface = false;
                _alpha = 0;
                _image.color = new Color(_currentColor.r, _currentColor.g, _currentColor.b, 0);
                
            }
        }
    }

    private void UpdateColor()
    {
        float max = _slider.maxValue;
        _sliderImage.color = Color.Lerp(Color.red, Color.green, _slider.value / max);
    }

    public void Add(int value)
    { 
        _slider.value += value;
    }
    public void Remove(int value)
    {
        _slider.value -= value;
    }
}
