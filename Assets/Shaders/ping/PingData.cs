using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingData : MonoBehaviour {

	public List<PingInter> PingInter;
	public List<PingHostile> PingHostile;

	private float[] _pingInteractable;
	private Vector4[] _originInteractable;

	private float[] _pingHostile;
	private Vector4[] _originHostile;

	private underwaterFX underwater;
	private int pingsInter;
	private int pingsHostile;

	void Start () {
		underwater = Camera.main.GetComponent<underwaterFX> (); 

		pingsInter = PingInter.Count;
		pingsHostile = PingHostile.Count;

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
		for (int i = 0; i < PingInter.Count; i++) {
			_originInteractable [i] = PingInter [i].getOrigin;
		}
	}

	void UpdatePings () {
		for (int i = 0; i < PingInter.Count; i++) {
			_pingInteractable [i] = PingInter [i].getPing;
		}

		for (int i = 0; i < PingInter.Count; i++) {
			_pingHostile [i] = PingHostile [i].getPing;
			_originHostile [i] = PingHostile [i].getOrigin;
		}
	}

	void UpdateShader () {
		underwater._mat.SetFloatArray ("_pingInter", _pingInteractable);
		underwater._mat.SetVectorArray ("_originInter", _originInteractable);
		underwater._mat.SetInt ("_pingsInter", pingsInter);

		underwater._mat.SetFloatArray ("_pingHostile", _pingHostile);
		underwater._mat.SetVectorArray ("_originHostile", _originHostile);
		underwater._mat.SetInt ("_pingsHostile", pingsHostile);
	}
}