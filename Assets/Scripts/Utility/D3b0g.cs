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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);// new Ray(pos, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Fish fish = hit.collider.GetComponent<Fish>();
                _tempRenderers = hit.collider.gameObject.GetComponentsInChildren<MeshRenderer>();

                if (fish == null)
                    return;

                _current = fish;
                Show();
            }
            else
            {
                Hide();
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
                "Direction: {1}\n" +
                "Speed: {2}\n" +
                "Range to origin: {3}\n" +
                "Range to target: {4}\n" +
                "Can chase target: {5}",
                enemy.GetState,
                enemy.Direction,
                enemy.Body.velocity,
                Vector3.Distance(enemy.transform.position, enemy.OriginPos),
                Vector3.Distance(enemy.Target.transform.position, enemy.transform.position),
                enemy.DetectTarget());
        }
        else if (_current is FishNeutral)
        {

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