using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingData : MonoBehaviour
{

	public List<PingInter> _interactibles;
	public List<PingHostile> _hostiles;

	private float[] _pingInteractable;
	private Vector4[] _originInteractable;

	private float[] _pingHostile;
	private Vector4[] _originHostile;

	private underwaterFX underwater;
	private int pingsInter;
	private int pingsHostile;

	void Start () {
		underwater = Camera.main.GetComponent<underwaterFX> (); 

		pingsInter = _interactibles.Count;
		pingsHostile = _hostiles.Count;

		_pingInteractable = new float[pingsInter];
		_originInteractable = new Vector4[pingsInter];

		_pingHostile = new float[pingsHostile];
		_originHostile = new Vector4[pingsHostile]; 

		SetupOrigins ();
		UpdatePings ();
	}

	void Update () {
		UpdatePings ();
		UpdateShader ();
	}

	void SetupOrigins () {
		for (int i = 0; i < _interactibles.Count; i++) {
			_originInteractable [i] = _interactibles [i].getOrigin;
		}

        if (_originInteractable.Length != 0)
            underwater._mat.SetVectorArray("_originInter", _originInteractable);
    }

    void UpdatePings () {
		for (int i = 0; i < _interactibles.Count; i++) {
			_pingInteractable [i] = _interactibles [i].getPing;
		}

		for (int i = 0; i < _hostiles.Count; i++) {
			_pingHostile [i] = _hostiles [i].getPing;
			_originHostile [i] = _hostiles [i].getOrigin;
		}
	}

	void UpdateShader ()
    {
        SetInteractibles();
        SetHostiles();

        if (_pingInteractable.Length != 0)
            underwater._mat.SetFloatArray ("_pingInter", _pingInteractable);

        if (_pingHostile.Length != 0)
        {
            underwater._mat.SetFloatArray("_pingHostile", _pingHostile);
            underwater._mat.SetVectorArray("_originHostile", _originHostile);
        }
	}

    void SetInteractibles()
    {
        int current = 0;

        for (int i = 0; i < _interactibles.Count; i++)
        {
            PingInter ping = _interactibles[i];

            if (ping.CheckOnScreen())
            {
                underwater._mat.SetFloat("_interactibles" + current.ToString(), i);
                current++;
            }
        }
        underwater._mat.SetFloat("_interactibles" + current.ToString(), -1);

        print(underwater._mat.GetFloat("_interactibles0"));
        print(underwater._mat.GetFloat("_interactibles1"));
        print(underwater._mat.GetFloat("_interactibles2"));

    }

    void SetHostiles()
    {
        int current = 0;

        for (int i = 0; i < _hostiles.Count; i++)
        {
            PingHostile ping = _hostiles[i];

            if (ping.CheckOnScreen())
            {
                underwater._mat.SetInt("_hostiles" + current.ToString(), i);
                current++;
            }
        }
        underwater._mat.SetInt("_hostiles" + current.ToString(), -1);
    }

}