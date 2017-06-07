using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Id : MonoBehaviour {

    [SerializeField]
    private string _id = "";
    private SubMovement _subMov;
    private Text _text;
    private Button[] _buttons;
    private Canvas _parentCanvas;

	// Use this for initialization
	void Start () {
        _subMov = FindObjectOfType<SubMovement>();
        _text = GetComponent<Text>();
        _buttons = GetComponentsInChildren<Button>();
        _buttons[0].onClick.AddListener(IncreaseValues);
        _buttons[1].onClick.AddListener(DecreaseValues);
        _parentCanvas = GetComponentInParent<Canvas>();
        ShowValues();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _parentCanvas.enabled = !_parentCanvas.enabled;
        }
    }

    private void IncreaseValues()
    {
        Debug.Log("increase");
        ChangeValue(true);
    }

    private void DecreaseValues()
    {
        ChangeValue(false);
    }

    private void ChangeValue(bool increase)
    {
        int value = 0;
        if (increase) { value = 1; }
        else { value = -1; }
        switch (_id)
        {
            case "DragSpeed":
                _subMov.DragSpeed += value;
                break;
            case "MaxSpeed":
                _subMov.MaxSpeed += value;
                break;
            case "DistanceForMax":
                _subMov.DistaceForMax += value;
                break;
            case "ChargeSpeed":
                _subMov.ChargeSpeed += value;
                break;
            case "TapChargeIntrvl":
                _subMov.TapChargeIntrvl += value;
                break;
            case "ChargeCooldwn":
                _subMov.ChargeCooldwn += value;
                break;
            case "StunSlowCool":
                _subMov.StunSlowCooldown += value;
                break;
            default:
                break;
        }
        ShowValues();
    }

    private void ShowValues()
    {
        switch (_id)
        {
            case "DragSpeed":
                _text.text = _subMov.DragSpeed.ToString();
                break;
            case "MaxSpeed":
                _text.text = _subMov.MaxSpeed.ToString();
                break;
            case "DistanceForMax":
                _text.text = _subMov.DistaceForMax.ToString();
                break;
            case "ChargeSpeed":
                _text.text = _subMov.ChargeSpeed.ToString();
                break;
            case "TapChargeIntrvl":
                _text.text = _subMov.TapChargeIntrvl.ToString();
                break;
            case "ChargeCooldwn":
                _text.text = _subMov.ChargeCooldwn.ToString();
                break;
            case "StunSlowCool":
                _text.text = _subMov.StunSlowCooldown.ToString();
                break;
            default:
                break;
        }
    }
}
