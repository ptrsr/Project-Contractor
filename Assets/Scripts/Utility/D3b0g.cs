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
    private LineRenderer _circle;

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
                if (!hit.collider.GetComponent<Fish>())
                    return;

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

        if (_current is Shark)
        {
            Shark shark = (Shark)_current;
            Transform nearest = shark.GetNearestWayPoint();
            _text.text = string.Format(
                "State: {0}\n" +
                "Range: {1}\n" +
                "Direction: {2}\n" +
                "Speed: {3}\n" +
                "Range to waypoint: {4:0.000}\n" +
                "Range to nearest waypoint: {5:0.000}\n" +
                "Range to target: {6:0.000}\n" +
                "Target to nearest waypoint: {7:0.000}\n" +
                "Current waypoint: {8}\n" +
                "Can chase target: {9}",
                shark.GetState,
                shark.Range,
                shark.Direction,
                shark.Body.velocity,
                Vector3.Distance(shark.transform.position, shark.GetCurrentWayPoint().position),
                Vector3.Distance(shark.transform.position, nearest.position),
                Vector3.Distance(shark.Target.position, shark.transform.position),
                Vector3.Distance(shark.Target.position, nearest.position),
                shark.WayId,
                shark.DetectTarget());
            if (shark.DetectTarget())
                Debug.DrawLine(shark.transform.position, shark.Target.position, Color.red);
            AddCircle(shark.gameObject, shark.transform.position, shark.Range);
        }
        else if (_current is FishEnemy)
        {
            FishEnemy enemy = (FishEnemy)_current;
            _text.text = string.Format(
                "State: {0}\n" +
                "Range: {1}\n" +
                "Direction: {2}\n" +
                "Speed: {3}\n" +
                "Range to origin: {4:0.000}\n" +
                "Range to target: {5:0.000}\n" +
                "Target from origin: {6:0.000}\n" +
                "Can chase target: {7}",
                enemy.GetState,
                enemy.Range,
                enemy.Direction,
                enemy.Body.velocity,
                Vector3.Distance(enemy.transform.position, enemy.OriginPos),
                Vector3.Distance(enemy.Target.position, enemy.transform.position),
                Vector3.Distance(enemy.Target.position, enemy.OriginPos),
                enemy.DetectTarget());
            AddCircle(enemy.gameObject, enemy.OriginPos, enemy.Range);
        }
        else if (_current is FishNeutral)
        {
            FishNeutral neutral = (FishNeutral)_current;
            _text.text = string.Format(
                "State: {0}\n" +
                "Range: {1}\n" +
                "Direction: {2}\n" +
                "Speed: {3}\n" +
                "Range to origin: {4:0.000}\n" +
                "Target from origin: {5:0.000}\n" +
                "Can chase target: {6}",
                neutral.GetState,
                neutral.DetectionRange,
                neutral.Direction,
                neutral.Body.velocity,
                Vector3.Distance(neutral.transform.position, neutral.OriginPos),
                Vector3.Distance(neutral.OriginPos, neutral.Target.position),
                (Vector3.Distance(neutral.OriginPos, neutral.Target.position) < neutral.DetectionRange));
            AddCircle(neutral.gameObject, neutral.OriginPos, neutral.DetectionRange);
        }
    }

    private void Hide()
    {
        _image.enabled = false;
        _text.enabled = false;
        _text.text = "";
        _current = null;

        RemoveCircle();

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

    private void AddCircle(GameObject obj, Vector3 center, float radius)
    {
        float scale = 0.1f;
        int size = (int)((2f * Mathf.PI) / scale) + 2;

        if (!obj.GetComponent<LineRenderer>())
        {
            _circle = obj.AddComponent<LineRenderer>();
            _circle.material = new Material(Shader.Find("Particles/Additive"));
            _circle.startColor = Color.red;
            _circle.endColor = Color.red;
            _circle.startWidth = 1f;
            _circle.endWidth = 1f;
            _circle.positionCount = size;
        }

        int i = 0;
        for (float c = 0; c < 2 * Mathf.PI; c += 0.1f)
        {
            float x = radius * Mathf.Cos(c);
            float y = radius * Mathf.Sin(c);

            Vector3 pos = new Vector3(center.x + x, center.y + y, 0);
            _circle.SetPosition(i, pos);
            i++;
        }
        _circle.SetPosition(i, _circle.GetPosition(0));
    }

    private void RemoveCircle()
    {
        Destroy(_circle);
    }
}