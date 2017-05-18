using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D3b0g : MonoBehaviour
{
    [SerializeField]
    private Color _selectColor = Color.red;

    private Image _image;
    private Text _text;
    private Fish _current;
    private MeshRenderer[] _tempRenderers = null;
    private List<Color> _tempColors = new List<Color>();

    public void Start()
    {
        Image[] images = FindObjectsOfType<Image>();
        foreach (Image img in images)
        {
            if (img.tag == "D3b0g")
            {
                _image = img;
                _text = _image.GetComponentInChildren<Text>();
                Hide();
                break;
            }
        }
    }

    public void Update()
    {
        UpdateText();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(pos);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Hide();

                Fish fish = hit.collider.GetComponent<Fish>();
                _tempRenderers = hit.collider.gameObject.GetComponentsInChildren<MeshRenderer>();

                if (fish == null)
                    return;

                _current = fish;
                Show();
            }
        }
    }

    private void UpdateText()
    {
        if (_current == null)
            return;

        if (_current is FishEnemy)
        {
            FishEnemy enemy = (FishEnemy)_current;
            _text.text = string.Format(
                "State: {0}\n" +
                "Range: {1}\n" +
                "Direction: {2}\n" +
                "Speed: {3}\n" +
                "Range to origin: {4}\n" +
                "Range to target: {5}\n" +
                "Target from origin: {6}\n" +
                "Can chase target: {7}",
                enemy.GetState,
                enemy.Range,
                enemy.Direction,
                enemy.Body.velocity,
                Vector3.Distance(enemy.transform.position, enemy.OriginPos),
                Vector3.Distance(enemy.Target.position, enemy.transform.position),
                Vector3.Distance(enemy.Target.position, enemy.OriginPos),
                enemy.DetectTarget());
        }
        else if (_current is FishNeutral)
        {
            FishNeutral neutral = (FishNeutral)_current;
            _text.text = string.Format(
                "State: {0}\n" +
                "Range: {1}\n" +
                "Direction: {2}\n" +
                "Speed: {3}\n" +
                "Range to origin: {4}\n" +
                "Target from origin: {5}\n" +
                "Can chase target: {6}",
                neutral.GetState,
                neutral.DetectionRange,
                neutral.Direction,
                neutral.Body.velocity,
                Vector3.Distance(neutral.transform.position, neutral.OriginPos),
                Vector3.Distance(neutral.OriginPos, neutral.Target.position),
                (Vector3.Distance(neutral.OriginPos, neutral.Target.position) < neutral.DetectionRange));
        }
        else if (_current is FishFriendly)
        {

        }
    }

    private void Hide()
    {
        _image.enabled = false;
        _text.enabled = false;
        _text.text = "";
        _current = null;

        if (_tempRenderers == null)
            return;

        for (int i = 0; i < _tempRenderers.Length; i++)
        {
            _tempRenderers[i].material.color = _tempColors[i];
        }

        _tempRenderers = null;
    }

    private void Show()
    {
        _image.enabled = true;
        _text.enabled = true;

        foreach (MeshRenderer rend in _tempRenderers)
        {
            _tempColors.Add(rend.material.color);
            rend.material.color = _selectColor;
        }
    }
}