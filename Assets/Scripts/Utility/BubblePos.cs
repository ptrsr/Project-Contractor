using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblePos : MonoBehaviour {

    [SerializeField]
    private Vector3 _positionOnLeft;
    [SerializeField]
    private Vector3 _positionOnRight;
    [SerializeField]
    private float _xScale = 0;
    [SerializeField]
    private float _yScale = 0;

    public Vector3 PositionOnLeft { get { return _positionOnLeft; } }

    public Vector3 PositionOnRight { get { return _positionOnLeft; } }

    public float xScale { get { return _xScale; } }

    public float yScale { get { return _yScale; } }

}
